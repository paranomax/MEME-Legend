using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace JeuxVideo_MemeLegend
{
    public partial class ficCombatOnline : Form
    {
        public NewCreature creaJ1, creaJ2;
        private Socket sServeur, sClient;
        private byte[] bBuffer;
        private string[] Message;
        private string Messagecomplet;
        private int j1, j2;
        NewCreature[] joueur1 = new NewCreature[3];
        NewCreature[] joueur2 = new NewCreature[3];
        bool tour = true; //true -> j1 false => j2
        bool Status, debut;
        private string sFichier = "";
        public ficCombatOnline(NewCreature[] equipeJ1, bool Status, string NomServeur)//debut complet
        {
            InitializeComponent();
            debut = true;
            joueur1 = equipeJ1;
            sServeur = null;
            sClient = null;
            
            this.Status = Status;//vrai serveur //faux client
            foreach(NewCreature c in joueur1){
                lbJ1.Items.Add(c.Nom);
            }
            creaJ1 = joueur1[0];

            bBuffer = new byte[256];

            if (Status == true) //serveur
            {
                
                tour = true;
                tbTexte.Text = "En Attente d'un Joueur";
                bSauvegarder.Enabled = true;
                ConnectionServeur();
                bSauvegarder.Enabled = true;
                
            }
            else //client
            {
                tour = false;
                tbTexte.Text = "Recherche de joueur";
                bSauvegarder.Enabled = false;
                ConnectionClient(NomServeur);
                
            }

        }

        public ficCombatOnline(NewCreature[] equipeJ1, NewCreature[] equipeJ2, bool start, int j1, int j2)//debut charger en tant que serveur
        {
            InitializeComponent();
            sServeur = null;
            sClient = null;
            debut = false;
            bBuffer = new byte[256];
            this.j1 = j1;
            this.j2 = j2;
            joueur1 = equipeJ1;
            bSauvegarder.Enabled = true;
            Status = true;
            tour = start;
            foreach (NewCreature c in joueur1)
            {
                lbJ1.Items.Add(c.Nom);
            }
            creaJ1 = joueur1[j1];

            joueur2 = equipeJ2;
            foreach (NewCreature c in joueur2)
            {
                lbJ2.Items.Add(c.Nom);
            }
            creaJ2 = joueur2[j2];
            ConnectionServeur();
        }

        public ficCombatOnline(string serveur )//debut charger en tant que client
        {
            InitializeComponent();

            sServeur = null;
            sClient = null;

            debut = false;
            Status = false;

            bBuffer = new byte[256];
            tbTexte.Text = "Recherche d'hote";
            bSauvegarder.Enabled = false;
            Thread.Sleep(250);
            ConnectionClient(serveur);
        }

        private void ficCombatOnline_Load(object sender, EventArgs e)
        {
           
        }
        private void ficCombatOnline_FormClosing(object sender, FormClosingEventArgs e)
        {
            Deconnection();
        }

        #region Gestion Connection

        private IPAddress AdresseValide(string nomPC)
        {
            IPAddress ipReponse = null;
            if (nomPC.Length > 0)
            {
                IPAddress[] ipsMachine = Dns.GetHostEntry(nomPC).AddressList;
                for (int i = 0; i < ipsMachine.Length; i++)
                {
                    Ping ping = new Ping();
                    PingReply pingReponse = ping.Send(ipsMachine[i]);
                    if (pingReponse.Status == IPStatus.Success)
                        if (ipsMachine[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipReponse = ipsMachine[i];
                            break;
                        }
                }
            }
            return ipReponse;
        }

        private void ConnectionServeur()
        {
            sClient = null;
            IPAddress ipServeur = AdresseValide(Dns.GetHostName());
            sServeur = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sServeur.Bind(new IPEndPoint(ipServeur, 8001));
            sServeur.Listen(1);
            sServeur.BeginAccept(new AsyncCallback(SurDemandeConnexion), sServeur);
        }

        private void SurDemandeConnexion(IAsyncResult iAR) //etabli connection
        {
            if (sServeur != null)
            {
                Socket sTmp = (Socket)iAR.AsyncState;
                sClient = sTmp.EndAccept(iAR);
                sClient.Send(Encoding.Unicode.GetBytes("a/"));
                //sClient.Send(Encoding.Unicode.GetBytes("a/"));
                InitialiserReception(sClient);
            }
        }

        private void InitialiserReception(Socket soc)
        {
            
            soc.BeginReceive(bBuffer, 0, bBuffer.Length, SocketFlags.None, new AsyncCallback(Reception), soc); 

        }

        private void Reception(IAsyncResult iAR)
        {

            //MessageBox.Show("reception");
            if (sClient != null)
            {
                Socket tmp = (Socket)iAR.AsyncState;
                if (tmp.EndReceive(iAR) > 0)
                {
                    //MessageBox.Show(Encoding.Unicode.GetString(bBuffer));
                    //InsererItemThread(Encoding.Unicode.GetString(bBuffer));
                    //lbEchange.Items.Insert(0, Encoding.Unicode.GetString(bBuffer));
                    //bBuffer = new byte[256];//manuellement le vider dans initialiserrecept
                    //InitialiserReception(tmp);
                    Messagecomplet = Encoding.Unicode.GetString(bBuffer);
                    Message = Messagecomplet.Split('/');
                    GestionDonnes(null, null);
                   // this.Invoke(new EventHandler(GestionDonnes));
                    for (int i = 0; i < bBuffer.Length; i++) //nettoyage
                        bBuffer[i] = 0;
                   
                    InitialiserReception(tmp);

                }
                else
                {
                    tmp.Disconnect(true);
                    tmp.Close();
                    if (sServeur != null)
                        sServeur.BeginAccept(new AsyncCallback(SurDemandeConnexion), sServeur);
                    sClient = null;
                }
            }
        }

        private void GestionDonnes(object sender, EventArgs e)
        {
            
            //MessageBox.Show(Messagecomplet);
            if (Status == true)
            {
                //this.Invoke(new EventHandler(GestionServeur));
                GestionServeur(/*null, null*/);
            }
            else if (Status == false)
            {
                //this.Invoke(new EventHandler(GestionClient));
                GestionClient(/*null, null*/);
            }
        }

        private void ConnectionClient(string NomServeur)
        {
            if (NomServeur.Length > 0)
            {
                sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sClient.Blocking = false;
                IPAddress IPServeur = AdresseValide(NomServeur);
                sClient.BeginConnect(new IPEndPoint(IPServeur, 8001), new AsyncCallback(SurConnexion), sClient);
            }
            else
            {
                MessageBox.Show("renseigner le serveur --'");
            }
        }
        private void SurConnexion(IAsyncResult iAR)
        {
            Socket Tmp = (Socket)iAR.AsyncState;
            if (Tmp.Connected)
            {
                InitialiserReception(Tmp);
            }
            else
            {
                MessageBox.Show("serveur inaccessible...");
            }
        }

        private void Deconnection()
        {
            if (sServeur == null)
            {
                sClient.Send(Encoding.Unicode.GetBytes("m/ deconnection/"));
                MajBoite();
                sClient.Send(Encoding.Unicode.GetBytes("close/"));
                sClient.Shutdown(SocketShutdown.Both);
                sClient.BeginDisconnect(false, new AsyncCallback(SurDemandeDeconnexion), sClient);
            }
            else if (sClient == null)
            {
                sServeur.Close();
                sServeur = null;
            }
        }

        private void SurDemandeDeconnexion(IAsyncResult iAR)
        {
            Socket tmp = (Socket)iAR.AsyncState;
            tmp.EndDisconnect(iAR);
        }

       //delegate void RenvoiVersInserer(string sTexte0);
        /*private void LancerThreadConnection()
        {
            Thread ThreadConnection = new Thread(new ParameterizedThreadStart(GestionConnection));
            ThreadConnection.Start();
        }
        #region test
        private void GestionConnection(object oTexte)
        {
            if (Status == true) //serveur
                GestionServeur();
            else if (Status == false) //client
                GestionClient();
        }*/

        /*private void InsererItem(object oTexte)
        {
            if (lbEchange.InvokeRequired)
            {
                RenvoiVersInserer f = new RenvoiVersInserer(InsererItem);
                Invoke(f, new object[] { (string)oTexte });
            }
            else
            {
                lbEchange.Items.Insert(0, (String)oTexte);
            }
        }*/
       #endregion

        #region gestion combat serveur side
        private void GestionServeur(/*object sender, EventArgs e*/)
        {
            /*if(Status == true)
            MessageBox.Show("cote serveur");
            else
                MessageBox.Show("cote serveur consider client");*/
            string reponse;
            switch (Message[0])
            {
                case "a"://debut comm
                    if (debut)
                    {
                        reponse = CreaToString(joueur1[0]);
                        tbTexte.Text = "chargement";
                        sClient.Send(Encoding.Unicode.GetBytes("c1/" + reponse));
                    }
                    if(debut == false)
                    {
                        reponse = CreaToString(joueur1[0]);
                        sClient.Send(Encoding.Unicode.GetBytes("b1/" + reponse));
                    }
                    break;
                case "close":
                    Close();
                    break;
                case "m": //message
                    tbTexte.Text = Message[1];
                    break;
                case "c1": //reception creature
                    joueur2[0] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[0].Nom);
                    creaJ2 = joueur2[0];
                    reponse = CreaToString(joueur1[1]);
                    sClient.Send(Encoding.Unicode.GetBytes("c2/" + reponse));
                    break;
                case "c2": //reception creature
                    joueur2[1] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[1].Nom);
                    reponse = CreaToString(joueur1[2]);
                    sClient.Send(Encoding.Unicode.GetBytes("c3/" + reponse));
                    break;
                case "c3": //reception creature  derniere
                    joueur2[2] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[2].Nom);
                    //reponse = CreaToString(joueur1[1]); //mettre fin au chargement
                    MajComplete();
                    changetour();
                    break;
                case "b1":
                    reponse = CreaToString(joueur1[1]);
                    sClient.Send(Encoding.Unicode.GetBytes("b2/" +reponse));
                    break;
                case "b2":
                    reponse = CreaToString(joueur1[2]);
                    sClient.Send(Encoding.Unicode.GetBytes("b3/" + reponse));
                    break;
                case "b3":
                    reponse = CreaToString(joueur2[0]);
                    sClient.Send(Encoding.Unicode.GetBytes("b4/" + reponse));
                    break;
                case "b4":
                    reponse = CreaToString(joueur2[1]);
                    sClient.Send(Encoding.Unicode.GetBytes("b5/" + reponse));
                    break;
                case "b5":
                    reponse = CreaToString(joueur2[2]);
                    sClient.Send(Encoding.Unicode.GetBytes("b6/" + reponse));
                    break;
                case "b6":
                    reponse = j1.ToString() + "/" + j2.ToString() + "/";
                    sClient.Send(Encoding.Unicode.GetBytes("b7/" + reponse));
                    //creaJ2 = joueur2[Int32.Parse(Message[1])];
                    //creaJ1 = joueur1[Int32.Parse(Message[2])];
                    if (tour)
                    {
                        sClient.Send(Encoding.Unicode.GetBytes("tourprep/false/"));
                    }
                    else
                    {
                        sClient.Send(Encoding.Unicode.GetBytes("tourprep/true/"));
                    }
                    //changetour();
                    MajComplete();
                    break;
                case "atta":
                    pause();
                    attaqueonline(Int32.Parse(Message[1]), creaJ2, creaJ1, false);
                    MajHp();
                    CheckVie();
                    break;
                case "hello":
                    MessageBox.Show("hello identifie cote serveur");
                    break;
                case "change":
                    checkChange();
                    break;
                case "okSauv":
                    changetour();
                    changetour();
                    break;
                default:
                    MessageBox.Show("Impossible à decode cote serveur");
                    for(int i = 0;i< Message.Length; i++)
                    {
                        MessageBox.Show(Message[i] +" dans message["+i.ToString()+"]");
                    }
                    /*foreach(string s in Message)
                    MessageBox.Show(s + "in m");*/
                    break;
            }
        }

        #endregion

        #region gestion combat client side

        private void GestionClient(/*object sender, EventArgs e*/)
        {
           /* if (Status == false)
                MessageBox.Show("cote client");
            else
                MessageBox.Show("cote client consider serveur");*/
            string reponse;
            switch (Message[0])
            {
                case "a":
                    tbTexte.Text = "chargement";
                    sClient.Send(Encoding.Unicode.GetBytes("a/"));
                    break;
                case "m": //message a afficher
                    tbTexte.Text = Message[1];
                    MajBoite();
                    break;
                case "box":
                    MessageBox.Show(Message[1]);
                    break;
                case "b1":
                    joueur2[0] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[0].Nom);
                    sClient.Send(Encoding.Unicode.GetBytes("b1/"));
                    break;
                case "b2":
                    joueur2[1] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[1].Nom);
                    sClient.Send(Encoding.Unicode.GetBytes("b2/"));
                    break;
                case "b3":
                    joueur2[2] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[2].Nom);
                    sClient.Send(Encoding.Unicode.GetBytes("b3/"));
                    break;
                case "b4":
                    joueur1[0] = StringToCrea(Message);
                    lbJ1.Items.Add(joueur1[0].Nom);
                    sClient.Send(Encoding.Unicode.GetBytes("b4/"));
                    break;
                case "b5":
                    joueur1[1] = StringToCrea(Message);
                    lbJ1.Items.Add(joueur1[1].Nom);
                    sClient.Send(Encoding.Unicode.GetBytes("b5/"));
                    break;
                case "b6":
                    joueur1[2] = StringToCrea(Message);
                    lbJ1.Items.Add(joueur1[2].Nom);
                    sClient.Send(Encoding.Unicode.GetBytes("b6/"));
                    break;
                case "b7":
                    creaJ2 = joueur2[Int32.Parse(Message[1])];
                    creaJ1 = joueur1[Int32.Parse(Message[2])];
                    sClient.Send(Encoding.Unicode.GetBytes("okSauv/"));
                    //MajComplete();
                    break;
                case "c1": //reception creature renvoie c1
                    joueur2[0] = StringToCrea(Message);
                    creaJ2 = joueur2[0];
                    lbJ2.Items.Add(joueur2[0].Nom);
                    reponse = CreaToString(joueur1[0]);
                    sClient.Send(Encoding.Unicode.GetBytes("c1/" + reponse));
                    break;
                case "c2": //reception creature
                    joueur2[1] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[1].Nom);
                    reponse = CreaToString(joueur1[1]);
                    sClient.Send(Encoding.Unicode.GetBytes("c2/" + reponse));
                    break;
                case "c3": //reception creature
                    joueur2[2] = StringToCrea(Message);
                    lbJ2.Items.Add(joueur2[2].Nom);
                    reponse = CreaToString(joueur1[2]);
                    sClient.Send(Encoding.Unicode.GetBytes("c3/" + reponse));
                    MajComplete();
                    break;
                case "deg"://reception degat
                    if (Message[1] == "1")
                    {
                        creaJ1.HP -= Int32.Parse(Message[2]);
                    }
                    if(Message[1] == "2")
                    {
                        creaJ2.HP -= Int32.Parse(Message[2]);
                    }
                    break;
                case "stat"://appliquer effet
                    if (Message[1] == "2")
                    {
                        creaJ1.techniques[0].ModifierStat(creaJ1, Message[2], Int32.Parse(Message[3]));
                    }
                    if(Message[1] == "1")
                    {
                        creaJ1.techniques[0].ModifierStat(creaJ2, Message[2], Int32.Parse(Message[3]));
                    }
                    break;
                case "tour":
                    changetour();
                    break;
                case "tourprep":
                    if (Message[1] == "true")
                        tour = true;
                    else
                        tour = false;
                    break;
                case "stop":
                    pause();
                    break;
                case "majboite":
                    MajBoite();                        
                    break;
                case "MajHp":
                    MajHp();
                    break;
                case "Changeimage":
                    ChangeImage(pbJ1, creaJ1 );
                    ChangeImage(pbJ2, creaJ2);
                    break;
                case "MajComplete":
                    MajComplete();
                    break;
                case "changeMeme":
                    creaJ2 = joueur2[Int32.Parse(Message[1])];
                    break;
                case "OKChange":
                    creaJ1 = joueur1[lbJ1.SelectedIndex];
                    bCap1J1.Enabled = true;
                    bCap2J1.Enabled = true;
                    bCap3J1.Enabled = true;
                    bCap4J1.Enabled = true;
                    break;
                case "doitchange":
                    BoxJ1.Enabled = true;
                    bCap1J1.Enabled = false;
                    bCap2J1.Enabled = false;
                    bCap3J1.Enabled = false;
                    bCap4J1.Enabled = false;
                    break;
                case "close":
                    Close();
                    break;
                case "hello":
                    MessageBox.Show("hello identifie cote client");
                    break;
                default:
                    MessageBox.Show("Impossible à decode cote client");
                    for (int i = 0; i < Message.Length; i++)
                    {
                        MessageBox.Show(Message[i] + " dans message[" + i.ToString() + "]");
                    }
                    /*foreach (string s in Message)
                        MessageBox.Show(s);*/ 

                    break;
            }
        }


        #endregion

        #region gestion bouton

        private void bQuitter_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bSauvegarder_Click(object sender, EventArgs e)
        {
            if (sfdEnregistrer.ShowDialog() == DialogResult.OK)
            {
                sFichier = sfdEnregistrer.FileName;
                StreamWriter sw = new StreamWriter(sFichier);

                //on parcours les equipes
                for (int i = 0; i < 3; i++)
                {
                    sw.WriteLine(joueur1[i].Nom + ";" +
                                 joueur1[i].Type + ";" +
                                 joueur1[i].HP.ToString() + ";" +
                                 joueur1[i].HPMax.ToString() + ";" +
                                 joueur1[i].Attaque.ToString() + ";" +
                                 joueur1[i].AttaqueSpec.ToString() + ";" +
                                 joueur1[i].Defense.ToString() + ";" +
                                 joueur1[i].DefenseSpec.ToString() + ";" +
                                 joueur1[i].capacite[0] + ";" +
                                 joueur1[i].capacite[1] + ";" +
                                 joueur1[i].capacite[2] + ";" +
                                 joueur1[i].capacite[3]);
                }
                for (int i = 0; i < 3; i++)
                {
                    sw.WriteLine(joueur2[i].Nom + ";" +
                                 joueur2[i].Type + ";" +
                                 joueur2[i].HP.ToString() + ";" +
                                 joueur2[i].HPMax.ToString() + ";" +
                                 joueur2[i].Attaque.ToString() + ";" +
                                 joueur2[i].AttaqueSpec.ToString() + ";" +
                                 joueur2[i].Defense.ToString() + ";" +
                                 joueur2[i].DefenseSpec.ToString() + ";" +
                                 joueur2[i].capacite[0] + ";" +
                                 joueur2[i].capacite[1] + ";" +
                                 joueur2[i].capacite[2] + ";" +
                                 joueur2[i].capacite[3]);
                }
                if (tour == true)
                    sw.WriteLine("1");
                else
                    sw.WriteLine("2");
                sw.WriteLine(ScanCreature(true).ToString());
                sw.WriteLine(ScanCreature(false).ToString());

                sw.Close();
            }
        }
        private void bChanger1_Click(object sender, EventArgs e)
        {
            if (Status)
            {
                //applique de son cote et envoie l info
                if (lbJ1.SelectedIndex >= 0)
                {
                    if (creaJ1 == joueur1[lbJ1.SelectedIndex])
                    {
                        MessageBox.Show("attention il s'agit du même meme");
                    }
                    else if (lbJ1.Items[lbJ1.SelectedIndex].ToString() == "RIP")
                    {
                        MessageBox.Show("Vous voulez envoyer un cadavre ??");
                    }
                    else
                    {
                        creaJ1 = joueur1[lbJ1.SelectedIndex];
                        sClient.Send(Encoding.Unicode.GetBytes("changeMeme/" + lbJ1.SelectedIndex + "/"));
                        tbTexte.Text = creaJ1.Nom + " !! en Avant !!";
                        sClient.Send(Encoding.Unicode.GetBytes("m/" + tbTexte.Text + "/"));
                        MajBoite();
                        MajComplete();
                        // MenuJ1_Paint(null, null); //maj des boutons
                        bCap1J1.Enabled = true;
                        bCap2J1.Enabled = true;
                        bCap3J1.Enabled = true;
                        bCap4J1.Enabled = true;
                        //MenuJ1.Enabled = true;
                        //bCap1J1.Enabled = bCap2J1.Enabled = bCap3J1.Enabled = bCap4J1.Enabled = true;
                        changetour();
                    }
                }
                else
                    MessageBox.Show("vous devez d'abord choisir le meme à échanger !!");
            }
            else
            {
                sClient.Send(Encoding.Unicode.GetBytes("change/"+lbJ1.SelectedIndex));
                //envoie ordre, gere une fonction de l autre cote
                //faire une fonction qui dit si ok
            }
            

        }

        public void checkChange()
        {
            int num = Int32.Parse(Message[1]);
            if(num >= 0)
            {
                if(joueur2[num] == creaJ2)
                {
                    sClient.Send(Encoding.Unicode.GetBytes("box/attention il s'agit du meme"));
                }
                else if (lbJ2.Items[num].ToString() == "RIP")
                {
                    sClient.Send(Encoding.Unicode.GetBytes("box/Vous voulez envoyer un cadavre ??/"));
                }
                else
                {
                    creaJ2 = joueur2[num];
                    sClient.Send(Encoding.Unicode.GetBytes("OKChange/"));
                    tbTexte.Text = creaJ2.Nom + " !! en Avant !!";
                    sClient.Send(Encoding.Unicode.GetBytes("m/" + tbTexte.Text + "/"));
                    MajBoite();
                    MajComplete();
                    changetour();
                }
            }
            else
            {
                sClient.Send(Encoding.Unicode.GetBytes("box/ Veullier choisir un meme à echanger/"));
            }
        }
        private void bCap1J1_Click(object sender, EventArgs e)
        {
            if(Status)
            {
                pause();
                attaqueonline(0, creaJ1, creaJ2, true);
                MajHp();
                CheckVie();
            }
            else
            {
                sClient.Send(Encoding.Unicode.GetBytes("atta/0/"));
            }
                
        }

        private void bCap2J1_Click(object sender, EventArgs e)
        {
            if (Status)
            {
                pause();
                attaqueonline(1, creaJ1, creaJ2, true);
                MajHp();
                CheckVie();
            }
            else
            {
                sClient.Send(Encoding.Unicode.GetBytes("atta/1/"));
            }
        }

        private void bCap3J1_Click(object sender, EventArgs e)
        {
            if (Status)
            {
                pause();
                attaqueonline(2, creaJ1, creaJ2, true);
                MajHp();
                CheckVie();
            }
            else
            {
                sClient.Send(Encoding.Unicode.GetBytes("atta/2/"));
            }
        }

        private void bCap4J1_Click(object sender, EventArgs e)
        {
            if (Status)
            {
                pause();
                attaqueonline(3, creaJ1, creaJ2, true);
                MajHp();
                CheckVie();
            }
            else
            {
                sClient.Send(Encoding.Unicode.GetBytes("atta/3/"));
            }
        }

        private void lbJ1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                majCreature(joueur1[lbJ1.SelectedIndex]);
            }
            catch(Exception err)
            {

            }
            
        }

        private void lbJ2_SelectedIndexChanged(object sender, EventArgs e)
        {
            majCreature(joueur2[lbJ2.SelectedIndex]);
        }

        public void majCreature( NewCreature crea)
        {
            lNoma.Text = crea.Nom;
            lTypea.Text = crea.Type;
            lPVa.Text = crea.HPMax.ToString();
            lAtta.Text = crea.Attaque.ToString();
            lAttSpeca.Text = crea.AttaqueSpec.ToString();
            lDefa.Text = crea.Defense.ToString();
            lDefSpeca.Text = crea.DefenseSpec.ToString();
            lCapacite1.Text = crea.techniques[0].Nom;
            lCapacite2.Text = crea.techniques[1].Nom;
            lCapacite3.Text = crea.techniques[2].Nom;
            lCapacite4.Text = crea.techniques[3].Nom;
        }

        #endregion

        #region gestion arriere

        private string CreaToString(NewCreature crea)
        {
            string message;
            message = crea.Nom + "/" +
                crea.Type + "/" +
                crea.HP.ToString() + "/" +
                crea.HPMax.ToString() + "/" +
                crea.Attaque.ToString() + "/" +
                crea.AttaqueSpec.ToString() + "/" +
                crea.Defense.ToString() + "/" +
                crea.DefenseSpec.ToString() + "/";

            for (int i = 0; i < 4; i++)
            {
                    message += crea.capacite[i].ToString() + "/";
            }
            return message;
        }

        private NewCreature StringToCrea(string[] text)
        {
            int[] idcap = new int[4]
            {
                 Int32.Parse(text[9]) ,
                 Int32.Parse(text[10]) ,
                 Int32.Parse(text[11]) ,
                 Int32.Parse(text[12]) 
            };
            NewCreature crea = new NewCreature(text[1], //nom
                                                text[2],//type
                                                Int32.Parse(text[3]),//hp
                                                Int32.Parse(text[4]),//hpmax
                                                Int32.Parse(text[5]),//att
                                                Int32.Parse(text[6]),//attspec
                                                Int32.Parse(text[7]),//def
                                                Int32.Parse(text[8]),//defspec
                                                idcap) ;//technique
            return crea;
        }

        private void ChangeImage(PictureBox photo, NewCreature creature)
        {
            try
            {
                switch (creature.Nom)
                {
                    case "Kermit":
                        photo.Image = Properties.Resources.Kermitph;
                        break;
                    case "Donald Duck":
                        photo.Image = Properties.Resources.donaldduckph;
                        break;
                    case "Doggo":
                        photo.Image = Properties.Resources.doggoph;
                        break;
                    case "Elon Musk":
                        photo.Image = Properties.Resources.Elonmuskph;
                        break;
                    case "Snoop Dogg":
                        photo.Image = Properties.Resources.snoopdoggph;
                        break;
                    case "Elmo":
                        photo.Image = Properties.Resources.Elmoph;
                        break;
                    case "Ricardo":
                        photo.Image = Properties.Resources.Ricardoph;
                        break;
                    case "Pepe":
                        photo.Image = Properties.Resources.pepethefrogph;
                        break;
                    case "Waluigi":
                        photo.Image = Properties.Resources.Waluigiph;
                        break;
                    default:
                        break;
                }
            }
            catch 
            {
                tbTexte.Text = "erreur de chargement d image";
            }
            if (Status)
                sClient.Send(Encoding.Unicode.GetBytes("Changeimage/"));
            
        }
        private void changetour()
        {
            if (tour == true)
            {

                BoxJ1.Enabled = true;
                if(Status)
                    tbTexte.Text = "au tour du Serveur";
                else
                    tbTexte.Text = "au tour du Client";
                tour = false;
                
            }
            else
            {
                BoxJ1.Enabled = false;
                if (Status)
                    tbTexte.Text = "au tour du Client";
                else
                    tbTexte.Text = "au tour du Serveur";
                tour = true;
            }
            if (Status)
                sClient.Send(Encoding.Unicode.GetBytes("tour/"));
            tbTexte.Refresh();
            Application.DoEvents();
        }
        void pause()
        {
            BoxJ1.Enabled = false;
            if (Status)
            {
                sClient.Send(Encoding.Unicode.GetBytes("stop/"));
                Thread.Sleep(20);
            }
                
        }

        private void attaqueonline(int i, NewCreature J1, NewCreature J2, bool SC) //si SC true, attaque dait par serveur sinon vient du client
        {
            int degat;

            pause();
            tbTexte.Text = (J1.Nom + " utilise " + J1.techniques[i].Nom + " !!!");
            //sClient.Send(Encoding.Unicode.GetBytes("m/" + J1.Nom + " utilise " + J1.techniques[i].Nom + " !!!"));
            sClient.Send(Encoding.Unicode.GetBytes("m/"+ tbTexte.Text+"/"));
            MajBoite();

            degat = J1.techniques[i].genererdegat(J1, J2);
            J2.HP -= degat;
            if(SC == true)//vient du serv
            {
                sClient.Send(Encoding.Unicode.GetBytes("stat/2/HP/-" + degat.ToString() + "/"));
                sClient.Send(Encoding.Unicode.GetBytes("MajHp/"));
            }
            else
            {
                sClient.Send(Encoding.Unicode.GetBytes("stat/1/HP/-" + degat.ToString() + "/"));
                sClient.Send(Encoding.Unicode.GetBytes("MajHp/"));
            }
            //sClient.Send(Encoding.Unicode.GetBytes("stat/1/HP/"+degat.ToString()+"/")); 

            tbTexte.Text = J1.Nom + " inflige " + degat.ToString() + " points de dégats";
            //sClient.Send(Encoding.Unicode.GetBytes("m/"+J1.Nom + " inflige " + degat.ToString() + " points de dégats/"));
            sClient.Send(Encoding.Unicode.GetBytes("m/" + tbTexte.Text + "/"));
            MajBoite();

            if (J1.techniques[i].tableType(J1.techniques[i].Type, J2.Type) == 2)
            {
                tbTexte.Text = "C'est super efficace !";
                sClient.Send(Encoding.Unicode.GetBytes("m/" + tbTexte.Text + "/"));
                MajBoite();
            }
            else if (J1.techniques[i].tableType(J1.techniques[i].Type, J2.Type) == 0.5)
            {
                tbTexte.Text = "Ce n'est pas très efficace";
                sClient.Send(Encoding.Unicode.GetBytes("m/" + tbTexte.Text + "/"));

                MajBoite();
            }
            if(SC == true)
            {
                J1.techniques[i].AppliquerEffet(J1, J2, tbTexte, sClient);
            }
            else
            {
                J1.techniques[i].AppliquerEffet(J2, J1, tbTexte, sClient);
            }
            
        }

        private void MajBoite()
        {
            tbTexte.Refresh();
            Application.DoEvents();

            if (Status == true)
            {
                sClient.Send(Encoding.Unicode.GetBytes("majboite/"));
                Thread.Sleep(2000);   
            }
        }
        private void MajHp()
        {
            try
            {
                //LifeJ1.Value = (creaJ1.HP * 100 / creaJ1.HPMax);
                LifeJ1.Invoke(new MethodInvoker(delegate
                {
                    LifeJ1.Value = (creaJ1.HP * 100 / creaJ1.HPMax);
                }));
                //LifeJ2.Value = (creaJ2.HP * 100 / creaJ2.HPMax);
                LifeJ2.Invoke(new MethodInvoker(delegate
                {
                    LifeJ2.Value = (creaJ1.HP * 100 / creaJ1.HPMax);
                }));
                if (Status == true)
                {
                    sClient.Send(Encoding.Unicode.GetBytes("MajHp/"));
                    Thread.Sleep(20);
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            
                
        }

        private void CheckVie() //return bool si vrai ou pas
        {

            if (creaJ1.HP == 0)
            {
                tbTexte.Text = creaJ1.Nom + " est KO !!";
                sClient.Send(Encoding.Unicode.GetBytes("m/" + creaJ1.Nom + " est KO !!/"));
                MajBoite();
                
                for (int i = 0; i < 3; i++)
                {
                    if (creaJ1 == joueur1[i])
                    {
                        lbJ1.Items[i] = "RIP";
                        joueur1[i].Nom = "RIP";
                    }
                }
                if (joueur1[0].Nom == "RIP" && joueur1[1].Nom == "RIP" && joueur1[2].Nom == "RIP")
                    FinPartie(2);
                else
                {
                    tbTexte.Text = "Serveur, veuillez choisir un nouveau meme !";
                    sClient.Send(Encoding.Unicode.GetBytes("m/Serveur choisi un meme/"));
                    MajBoite();
                    BoxJ1.Enabled = true;
                    bCap1J1.Enabled = false;
                    bCap2J1.Enabled = false;
                    bCap3J1.Enabled = false;
                    bCap4J1.Enabled = false;
                    //BoxJ2.Enabled = false;
                    //MenuJ1.Enabled = false; bouton
                    // bCap1J1.Enabled = bCap2J1.Enabled = bCap3J1.Enabled = bCap4J1.Enabled = false;
                }
                //on corrige dans changer meme
            }
            else if (creaJ2.HP == 0)
            {
                tbTexte.Text = creaJ2.Nom + " est KO !!";
                sClient.Send(Encoding.Unicode.GetBytes("m/" + creaJ2.Nom + " est KO !!/"));
                MajBoite();
                for (int i = 0; i < 3; i++)
                {
                    if (creaJ2 == joueur2[i])
                    {
                        lbJ2.Items[i] = "RIP";
                        joueur2[i].Nom = "RIP";
                    }
                }
                if (joueur2[0].Nom == "RIP" && joueur2[1].Nom == "RIP" && joueur2[2].Nom == "RIP")
                    FinPartie(1);
                else
                {
                    tbTexte.Text = "Client, veuillez choisir un nouveau meme !";
                    sClient.Send(Encoding.Unicode.GetBytes("doitchange/"));
                    MajBoite();
                    sClient.Send(Encoding.Unicode.GetBytes("m/" + tbTexte.Text + "/"));
                    //BoxJ1.Enabled = false;
                    // bCap1J2.Enabled = bCap2J2.Enabled = bCap3J2.Enabled = bCap4J2.Enabled = false;
                }
            }
            else
                changetour();
        }

        private void MajComplete()
        {
            /*Thread t = new Thread(MajCompletThread);
            t.Start();*/
            /*labelSolution.Invoke(new MethodInvoker(delegate
            {
                labelSolution.Text = laSolution.ToString();
            }));*/
            ChangeImage(pbJ1, creaJ1);
            ChangeImage(pbJ2, creaJ2);
            //bCap1J1.Text = creaJ1.techniques[0].Nom;
            bCap1J1.Invoke(new MethodInvoker(delegate
            {
                bCap1J1.Text = creaJ1.techniques[0].Nom;
            }));
            //bCap2J1.Text = creaJ1.techniques[1].Nom;
            bCap2J1.Invoke(new MethodInvoker(delegate
            {
                bCap2J1.Text = creaJ1.techniques[1].Nom;
            }));
           // bCap3J1.Text = creaJ1.techniques[2].Nom;
            bCap3J1.Invoke(new MethodInvoker(delegate
            {
                bCap3J1.Text = creaJ1.techniques[2].Nom;
            }));
           // bCap4J1.Text = creaJ1.techniques[3].Nom;
            bCap4J1.Invoke(new MethodInvoker(delegate
            {
                bCap4J1.Text = creaJ1.techniques[3].Nom;
            }));
            MajHp();
            if (Status)
                sClient.Send(Encoding.Unicode.GetBytes("MajComplete/"));
        }



        /*private void MajCompletThread()
        {
            ChangeImage(pbJ1, creaJ1);
            ChangeImage(pbJ2, creaJ2);
            bCap1J1.Text = creaJ1.techniques[0].Nom;
            bCap2J1.Text = creaJ1.techniques[1].Nom;
            bCap3J1.Text = creaJ1.techniques[2].Nom;
            bCap4J1.Text = creaJ1.techniques[3].Nom;
            MajHp();
            if (Status)
                sClient.Send(Encoding.Unicode.GetBytes("MajComplete/"));
        }*/
        

        private int ScanCreature(bool Joueur)
        {
            int i;
            if (Joueur == true)
            {
                for (i = 0; i < 3; i++)
                {
                    if (joueur1[i] == creaJ1)
                        break;
                }
            }
            else
            {
                for (i = 0; i < 3; i++)
                {
                    if (joueur2[i] == creaJ2)
                        break;
                }
            }
            return i;
        }
        private void FinPartie(int J) //false j1 true j2
        {
            sClient.Send(Encoding.Unicode.GetBytes("box/Le joueur" + J + " a gagné !!!/"));
            MessageBox.Show("Le joueur" + J + " a gagné !!!");
            Close();
        }

        #endregion
    }
}

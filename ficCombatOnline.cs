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
        NewCreature[] joueur1 = new NewCreature[3];
        NewCreature[] joueur2 = new NewCreature[3];
        bool tour = true; //true -> j1 false => j2
        bool Status, debut;
        private string sFichier = "";
        public ficCombatOnline(NewCreature[] equipeJ1, bool Status, string NomServeur)
        {
            InitializeComponent();
            debut = false;
            joueur1 = equipeJ1;
            sServeur = null;
            sClient = null;
            
            this.Status = Status;
            foreach(NewCreature c in joueur1){
                lbJ1.Items.Add(c.Nom);
            }
            creaJ1 = joueur1[0];

            bBuffer = new byte[256];

            if (Status == true) //serveur
            {
                debut = true;
                tour = true;
                tbTexte.Text = "Vous etes serveur";
                ConnectionServeur();
                
            }
            else //client
            {
                debut = false;
                tour = false;
                tbTexte.Text = "Vous etes client";
                ConnectionClient(NomServeur);
            }

        }
        private void ficCombatOnline_Load(object sender, EventArgs e)
        {

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

        private void SurDemandeConnexion(IAsyncResult iAR)
        {
            if (sServeur != null)
            {
                Socket sTmp = (Socket)iAR.AsyncState;
                sClient = sTmp.EndAccept(iAR);
                //sClient.Send(Encoding.Unicode.GetBytes("connexion effectué par " + ((IPEndPoint)sClient.RemoteEndPoint).Address.ToString()));
                sClient.Send(Encoding.Unicode.GetBytes("a"));
                InitialiserReception(sClient);
            }
        }

        private void InitialiserReception(Socket soc)
        {
            
            soc.BeginReceive(bBuffer, 0, bBuffer.Length, SocketFlags.None, new AsyncCallback(Reception), soc);


        }

        private void Reception(IAsyncResult iAR)
        {
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
                    string Messagecomplet = Encoding.Unicode.GetString(bBuffer);
                    Message = Messagecomplet.Split('/');

                    for (int i = 0; i < bBuffer.Length; i++)
                        bBuffer[i] = 0;

                    if (Status == true)
                    {
                        GestionServeur();
                    }
                    else if(Status == false)
                    {
                        GestionClient();
                    }
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

        private void Deconnection(object sender, EventArgs e)
        {
            /*if (sServeur == null)
            {
                sClient.Send(Encoding.Unicode.GetBytes("Déconnection (client)"));
                sClient.Shutdown(SocketShutdown.Both);
                sClient.BeginDisconnect(false, new AsyncCallback(SurDemandeDeconnexion), sClient);
                mcsConnecter.Enabled = mcsEcouter.Enabled = true;
                mcsDeconnecter.Enabled = false;
            }
            else if (sClient == null)
            {
                sServeur.Close();
                mcsConnecter.Enabled = mcsEcouter.Enabled = true;
                mcsDeconnecter.Enabled = false;
                sServeur = null;
            }*/
        }

        private void SurDemandeDeconnexion(IAsyncResult iAR)
        {
            Socket tmp = (Socket)iAR.AsyncState;
            tmp.EndDisconnect(iAR);
        }

        delegate void RenvoiVersInserer(string sTexte0);
        private void LancerThreadConnection()
        {
            Thread ThreadConnection = new Thread(new ParameterizedThreadStart(GestionConnection));
            ThreadConnection.Start();
        }

        private void GestionConnection(object oTexte)
        {
            if (Status == true) //serveur
                GestionServeur();
            else if (Status == false) //client
                GestionClient();
        }

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

        #region processus de debut de partie


        #endregion

        #region gestion combat serveur side
        private void GestionServeur()
        {
            string reponse;
            switch (Message[0])
            {
                case "a"://debut comm
                    reponse = CreaToString(joueur1[0]);
                    sClient.Send(Encoding.Unicode.GetBytes("c1/"+reponse));
                    break;
                case "m": //message
                    tbTexte.Text = Message[1];
                    break;
                case "c1": //reception creature
                    joueur2[0] = StringToCrea(Message);
                    creaJ2 = joueur2[1];
                    reponse = CreaToString(joueur1[1]);
                    sClient.Send(Encoding.Unicode.GetBytes("c2/" + reponse));
                    break;
                case "c2": //reception creature
                    joueur2[1] = StringToCrea(Message);
                    reponse = CreaToString(joueur1[1]);
                    sClient.Send(Encoding.Unicode.GetBytes("c3/" + reponse));
                    break;
                case "c3": //reception creature  derniere
                    joueur2[2] = StringToCrea(Message);
                    //reponse = CreaToString(joueur1[1]);
                   // sClient.Send(Encoding.Unicode.GetBytes("c1/" + reponse)); //mettre fin au chargement
                    break;
                case "b":
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region gestion combat client side

        private void GestionClient()
        {
            string reponse;
            switch (Message[0])
            {
                case "1":
                    break;
                case "m": //message a afficher
                    tbTexte.Text = Message[1];
                    break;
                case "c1": //reception creature renvoie c1
                    joueur2[0] = StringToCrea(Message);
                    creaJ2 = joueur2[0];
                    reponse = CreaToString(joueur1[0]);
                    sClient.Send(Encoding.Unicode.GetBytes("c1/" + reponse));
                    break;
                case "c2": //reception creature
                    joueur2[1] = StringToCrea(Message);
                    reponse = CreaToString(joueur1[1]);
                    sClient.Send(Encoding.Unicode.GetBytes("c2/" + reponse));
                    break;
                case "c3": //reception creature
                    joueur2[2] = StringToCrea(Message);
                    reponse = CreaToString(joueur1[2]);
                    sClient.Send(Encoding.Unicode.GetBytes("c3/" + reponse));
                    break;
                default:
                    break;
            }
        }


        #endregion

        #region gestion bouton
        private void bChanger1_Click(object sender, EventArgs e)
        {
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
                    tbTexte.Text = creaJ1.Nom + " !! en Avant !!";
                    tbTexte.Refresh();
                    Application.DoEvents();
                    Thread.Sleep(1500);
                   // MenuJ1_Paint(null, null); //maj des boutons
                    ChangeImage(pbJ1, creaJ1);
                    MajHp();
                    //MenuJ1.Enabled = true;
                    //bCap1J1.Enabled = bCap2J1.Enabled = bCap3J1.Enabled = bCap4J1.Enabled = true;
                    changetour();
                }
            }
            else
                MessageBox.Show("vous devez d'abord choisir le meme à échanger !!");
        }
        private void bCap1J1_Click(object sender, EventArgs e)
        {
            /*pause();
            Attaquer(0, creaJ1, creaJ2);
            MajHp();
            CheckVie();*/
        }

        private void bCap2J1_Click(object sender, EventArgs e)
        {

        }

        private void bCap3J1_Click(object sender, EventArgs e)
        {

        }

        private void bCap4J1_Click(object sender, EventArgs e)
        {

        }

        private void lbJ1_SelectedIndexChanged(object sender, EventArgs e)
        {
            majCreature(joueur1[lbJ1.SelectedIndex]);
        }

        private void lbJ2_SelectedIndexChanged(object sender, EventArgs e)
        {
            majCreature(joueur1[lbJ2.SelectedIndex]);
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
                if (i < 3)
                    message += crea.capacite[i] + "/";
                else
                    message += crea.capacite[i];
            }
            return message;
        }

        private NewCreature StringToCrea(string[] text)
        {
            string[] idcap = new string[4]
            {
                 text[9] ,
                 text[10] ,
                 text[11] ,
                 text[12] 
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
        void changetour()
        {
            if (tour == true)
            {
                BoxJ1.Enabled = false;
                tbTexte.Text = "au tour du J2";
                tour = false;
            }
            else
            {
                BoxJ1.Enabled = true;
                tbTexte.Text = "au tour du J1";
                tour = true;
            }
            tbTexte.Refresh();
            Application.DoEvents();
        }
        void pause()
        {
            BoxJ1.Enabled = false;
        }

        private void Attaquer(int i, Creature J1, Creature J2) // Infliger les degat 
        {
            int degat;
            Random Alea = new Random();
            double dAlea = (double)Alea.Next(80, 120) / 100;

            pause();
            tbTexte.Text = (J1.Nom + " utilise " + J1.techniques[i].Nom + " !!!");
            MajBoite();

            degat = J1.techniques[i].genererdegat(J1, J2);

            J2.HP -= degat;
            tbTexte.Text = J1.Nom + " inflige " + degat.ToString() + " points de dégats";
            MajBoite();

            if (J1.techniques[i].tableType(J1.techniques[i].Type, J2.Type) == 2)
            {
                tbTexte.Text = "C'est super efficace !";
                MajBoite();
            }
            else if (J1.techniques[i].tableType(J1.techniques[i].Type, J2.Type) == 0.5)
            {
                tbTexte.Text = "Ce n'est pas très efficace";
                MajBoite();
            }
            J1.techniques[i].AppliquerEffet(J1, J2, tbTexte);
        }

        private void MajBoite()
        {
            tbTexte.Refresh();
            Application.DoEvents();
            Thread.Sleep(2000);
        }
        private void MajHp()
        {
            LifeJ1.Value = (creaJ1.HP * 100 / creaJ1.HPMax);
            LifeJ2.Value = (creaJ2.HP * 100 / creaJ2.HPMax);
        }

        private void CheckVie() //return bool si vrai ou pas
        {

            if (creaJ1.HP == 0)
            {
                tbTexte.Text = creaJ1.Nom + " est KO !!";
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
                    tbTexte.Text = "J1, veuillez choisir un nouveau meme !";
                    MajBoite();
                    BoxJ1.Enabled = true;
                    //BoxJ2.Enabled = false;
                    //MenuJ1.Enabled = false; bouton
                    // bCap1J1.Enabled = bCap2J1.Enabled = bCap3J1.Enabled = bCap4J1.Enabled = false;
                }
                //on corrige dans changer meme
            }
            else if (creaJ2.HP == 0)
            {
                tbTexte.Text = creaJ2.Nom + " est KO !!";
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
                    tbTexte.Text = "J2, veuillez choisir un nouveau meme !";
                    MajBoite();
                    BoxJ1.Enabled = false;
                    // bCap1J2.Enabled = bCap2J2.Enabled = bCap3J2.Enabled = bCap4J2.Enabled = false;
                }
            }
            else
                changetour();
        }

        

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
            MessageBox.Show("Le joueur" + J + " a gagné !!!");
            Close();
        }

        #endregion
    }
}

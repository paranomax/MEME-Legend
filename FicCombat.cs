using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
//using System.Runtime.InteropServices;

namespace JeuxVideo_MemeLegend
{

    public partial class EcranCombat : Form
    {
        public Creature creaJ1, creaJ2;
        Creature[] joueur1 = new Creature[3];
        Creature[] joueur2 = new Creature[3];
        bool tour = true; //true -> j1 false => j2
        private string sFichier;

        public EcranCombat(Creature[] equipeJ1, Creature[] equipeJ2, bool tour )
        {
            InitializeComponent();
            int i;
            joueur1 = equipeJ1;
            for ( i = 0; i < 3; i++)
                if (joueur1[i].Nom != "RIP")
                {
                    creaJ1 = joueur1[i];
                    break;
                }
                               
            joueur2 = equipeJ2;
            for (i = 0; i < 3; i++)
                if (joueur2[i].Nom != "RIP")
                {
                    creaJ2 = joueur2[i];
                    break;
                }

            this.tour = tour;
        }

        private void EcranCombat_Load(object sender, EventArgs e)
        {
            sFichier = "";
            MajInfo(true);
            MajInfo(false);
            ChangeImage(pbJ1, creaJ1);
            ChangeImage(pbJ2, creaJ2);
            for(int i = 0;i<3;i++)
            {
                lbJ1.Items.Add(joueur1[i].Nom);
                lbJ2.Items.Add(joueur2[i].Nom);
            }
            MajHp();
            changetour();
            changetour();
            tbTexte.Text = "que le combat commence !";
            Thread.Sleep(2000);

        }

        private void bQuitter_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MajInfo(bool joueur)
        {
            if(joueur == true)
            {
                bCap1J1.Text = creaJ1.techniques[0].Nom;
                bCap2J1.Text = creaJ1.techniques[1].Nom;
                bCap3J1.Text = creaJ1.techniques[2].Nom;
                bCap4J1.Text = creaJ1.techniques[3].Nom;
                lJ1.Text = creaJ1.Nom;
            }
            else
            {
                bCap1J2.Text = creaJ2.techniques[0].Nom;
                bCap2J2.Text = creaJ2.techniques[1].Nom;
                bCap3J2.Text = creaJ2.techniques[2].Nom;
                bCap4J2.Text = creaJ2.techniques[3].Nom;
                lJ2.Text = creaJ2.Nom;
            }
        }

        private void ChangeImage(PictureBox photo, Creature creature)
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
                BoxJ2.Enabled = true;
                tbTexte.Text = "au tour du J2";
                tour = false;
            }
            else
            {
                BoxJ1.Enabled = true;
                BoxJ2.Enabled = false;
                tbTexte.Text = "au tour du J1";
                tour = true;
            }
            
            tbTexte.Refresh();
            Application.DoEvents();
            Thread.Sleep(1000);
        }

        void pause()
        {
            BoxJ2.Enabled = false;
            BoxJ1.Enabled = false;
        }

        private void Attaquer(int i, Creature J1, Creature J2) // Infliger les degat 
        {
            int degat;
            pause();
            tbTexte.Text = (J1.Nom + " utilise " + J1.techniques[i].Nom + " !!!");
            MajBoite(tbTexte);

            degat = J1.techniques[i].genererdegat(J1, J2);
            J2.HP -= degat;

            tbTexte.Text = J1.Nom + " inflige " + degat.ToString() + " points de dégats";

            MajBoite(tbTexte);

            if (J1.techniques[i].tableType(J1.techniques[i].Type, J2.Type) == 2)
            {
                tbTexte.Text = "C'est super efficace !";
                MajBoite(tbTexte);
            }
            else if (J1.techniques[i].tableType(J1.techniques[i].Type, J2.Type) == 0.5)
            {
                tbTexte.Text = "Ce n'est pas très efficace";
                MajBoite(tbTexte);
            }              
            J1.techniques[i].AppliquerEffet(J1, J2, tbTexte);
        }

        private void MajBoite(TextBox boite)
        {
            boite.Refresh();
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
                MajBoite(tbTexte);
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
                    MajBoite(tbTexte);
                    BoxJ1.Enabled = true;
                    BoxJ2.Enabled = false;
                    bCap1J1.Enabled = bCap2J1.Enabled = bCap3J1.Enabled = bCap4J1.Enabled = false;
                }
                //on corrige dans changer meme
            }
            else if (creaJ2.HP == 0)
            {
                tbTexte.Text = creaJ2.Nom + " est KO !!";
                MajBoite(tbTexte);
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
                    MajBoite(tbTexte);
                    BoxJ1.Enabled = false;
                    BoxJ2.Enabled = true;
                    bCap1J2.Enabled = bCap2J2.Enabled = bCap3J2.Enabled = bCap4J2.Enabled = false;
                }
            }
            else
                changetour();
        }

        private void FinPartie(int J) //false j1 true j2
        {
            MessageBox.Show("Le joueur" + J + " a gagné !!!");
            Close();
        }

        #region bouton

        private void bInformation1_Click(object sender, EventArgs e)
        {
            if (lbJ1.SelectedIndex >= 0)
            {
                MessageBox.Show("Informations de : " + joueur1[lbJ1.SelectedIndex].Nom +
                                "\nHP : " + joueur1[lbJ1.SelectedIndex].HP.ToString() + "/" + joueur1[lbJ1.SelectedIndex].HPMax.ToString() +
                                "\nAtt : " + joueur1[lbJ1.SelectedIndex].Attaque.ToString()+
                                "\nAttSpec : " + joueur1[lbJ1.SelectedIndex].AttaqueSpec.ToString() +
                                "\nDefense : " + joueur1[lbJ1.SelectedIndex].Defense.ToString() +
                                "\nDefenseSpec : " + joueur1[lbJ1.SelectedIndex].DefenseSpec.ToString() );
            }
            else
                MessageBox.Show("vous devez d'abord choisir \n le meme à consulter!!");
        } // information

        private void bInformation2_Click(object sender, EventArgs e)
        {
            if (lbJ2.SelectedIndex >= 0)
            {
                MessageBox.Show("Informations de : " + joueur2[lbJ2.SelectedIndex].Nom +
                                "\nHP : " + joueur2[lbJ2.SelectedIndex].HP.ToString() + "/" + joueur2[lbJ2.SelectedIndex].HPMax.ToString() +
                                "\nAtt : " + joueur2[lbJ2.SelectedIndex].Attaque.ToString() +
                                "\nAttSpec : " + joueur2[lbJ2.SelectedIndex].AttaqueSpec.ToString() +
                                "\nDefense : " + joueur2[lbJ2.SelectedIndex].Defense.ToString() +
                                "\nDefenseSpec : " + joueur2[lbJ2.SelectedIndex].DefenseSpec.ToString());
            }
            else
                MessageBox.Show("vous devez d'abord choisir le meme à consulter!!");
        }

        private void bChanger1_Click(object sender, EventArgs e)
        {
            if (lbJ1.SelectedIndex >= 0)
            {
                if(creaJ1 == joueur1[lbJ1.SelectedIndex])
                {
                    MessageBox.Show("attention il s'agit du même meme");
                }
                else if(lbJ1.Items[lbJ1.SelectedIndex].ToString() == "RIP")
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
                    MajInfo(true);
                    ChangeImage(pbJ1, creaJ1);
                    MajHp();
                    bCap1J1.Enabled = bCap2J1.Enabled = bCap3J1.Enabled = bCap4J1.Enabled = true;
                    changetour();
                } 
            }
            else
                MessageBox.Show("vous devez d'abord choisir le meme à échanger !!");
        } //changer

        private void bChanger2_Click(object sender, EventArgs e)
        {
            if (lbJ2.SelectedIndex >= 0)
            {
                if(creaJ2 == joueur2[lbJ2.SelectedIndex])
                {
                    MessageBox.Show("attention il s'agit du même meme");
                }
                else if (lbJ2.Items[lbJ2.SelectedIndex].ToString() == "RIP")
                {
                    MessageBox.Show("Vous voulez envoyer un cadavre ??");
                }
                else
                {
                    creaJ2 = joueur2[lbJ2.SelectedIndex];
                    tbTexte.Text = creaJ2.Nom + " !! en Avant !!";
                    tbTexte.Refresh();
                    Application.DoEvents();
                    Thread.Sleep(1500);
                    MajInfo(false);
                    ChangeImage(pbJ2, creaJ2);
                    MajHp();
                    bCap1J2.Enabled = bCap2J2.Enabled = bCap3J2.Enabled = bCap4J2.Enabled = true;
                    changetour();
                }             
            }
            else
                MessageBox.Show("vous devez d'abord choisir le meme à échanger !!");
        }

        private void bCap1J1_Click(object sender, EventArgs e)
        {
            pause();
            Attaquer(0, creaJ1, creaJ2);
            MajHp();
            CheckVie();
        } //capacite J1

        private void bCap2J1_Click(object sender, EventArgs e)
        {
            pause();
            Attaquer(1, creaJ1, creaJ2);
            MajHp();

            CheckVie();
        }

        private void bCap3J1_Click(object sender, EventArgs e)
        {
            Attaquer(2, creaJ1, creaJ2);
            MajHp();

            CheckVie();
        }

        private void bCap4J1_Click(object sender, EventArgs e)
        {
            Attaquer(3, creaJ1, creaJ2);
            MajHp();

            CheckVie();
        }

        private void bCap1J2_Click(object sender, EventArgs e)
        {
            Attaquer(0, creaJ2, creaJ1);
            MajHp();

            CheckVie();
        } //capacite J2

        private void bCap2J2_Click(object sender, EventArgs e)
        {
            Attaquer(1, creaJ2, creaJ1);
            MajHp();

            CheckVie();
        }

        private void bCap3J2_Click(object sender, EventArgs e)
        {
            Attaquer(2, creaJ2, creaJ1);
            MajHp();

            CheckVie();
        } 

        private void bCap4J2_Click(object sender, EventArgs e)
        {
            Attaquer(3, creaJ2, creaJ1);
            MajHp();

            CheckVie();
        }

        private void bSauvegarder_Click(object sender, EventArgs e)
        {
            if(sfdEnregistrer.ShowDialog() == DialogResult.OK)
            {
                sFichier = sfdEnregistrer.FileName;
                StreamWriter sw = new StreamWriter(sFichier);

                //on parcours les equipes
                for(int i=0;i<3;i++)
                {
                    sw.WriteLine(joueur1[i].Nom + ";" +
                                 joueur1[i].Type + ";" +
                                 joueur1[i].HP.ToString() + ";" +
                                 joueur1[i].HPMax.ToString() + ";" +
                                 joueur1[i].Attaque.ToString() + ";" +
                                 joueur1[i].AttaqueSpec.ToString() + ";" +
                                 joueur1[i].Defense.ToString() + ";" +
                                 joueur1[i].DefenseSpec.ToString() + ";" +
                                 joueur1[i].techniques[0].IDtech.ToString() + ";" +
                                 joueur1[i].techniques[1].IDtech.ToString() + ";" +
                                 joueur1[i].techniques[2].IDtech.ToString() + ";" +
                                 joueur1[i].techniques[3].IDtech.ToString()); 
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
                                 joueur2[i].techniques[0].IDtech.ToString() + ";" +
                                 joueur2[i].techniques[1].IDtech.ToString() + ";" +
                                 joueur2[i].techniques[2].IDtech.ToString() + ";" +
                                 joueur2[i].techniques[3].IDtech.ToString());
                }
                if (tour == true)
                    sw.WriteLine("1");
                else
                    sw.WriteLine("2");
                

                sw.Close();
            }
        }
        #endregion




    }
}

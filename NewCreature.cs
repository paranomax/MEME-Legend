using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeuxVideo_MemeLegend
{
    public class NewCreature
    {
        public String Nom, Type;
        public int i;
        public NewCapacite[] techniques = new NewCapacite[4];
        public int[] capacite = new int[4];
        public static Dictionary<int, NewCapacite> ListCapacite = new Dictionary<int, NewCapacite>()
        {
            {1, new NewCapacite("trempette", "Neutre", 0, 1, false, 1) },//1 trempette
            {2, new NewCapacite("Come back des muppets", "Normal", 70, 0, false, 2) },//2 comeback
            {3, new NewCapacite("Showbiz", "Neutre", 0, 2, false, 3) },//3showbiz
            {4, new NewCapacite("Lance-Thé", "Eau", 65, 0, true, 4) },//4lancethe
            {5, new NewCapacite("Rage-On", "Psy", 0, 3, true, 5) },//5rageon
            {6, new NewCapacite("Smug Face", "Neutre", 0, 4, true, 6) },//6smugface
            {7, new NewCapacite("Cyber-harcèlement", "Psy", 60, 0, true, 7) },//7cyberharcel
            {8, new NewCapacite("Investissement", "Neutre", 0, 5, true, 8) },//8invest
            {9, new NewCapacite("Lance-Flamme", "Feu", 65, 0, true, 9) },//9LanceFlam
            {10, new NewCapacite("Météor Tesla", "Milliardaire", 80, 6, true, 10) },//10MeteorTesla
            {11, new NewCapacite("Claque de Liasse", "Milliardaire", 60, 0, false, 11) },//11ClaqueLias
            {12,new NewCapacite("Weed?", "Poison", 75, 7, true, 12) },//12Weed
            {13, new NewCapacite("Nuage Toxique", "Poison", 65, 0, true, 13) },//13NuageTox
            {14, new NewCapacite("Feat", "Neutre", 0, 8, false, 14) },//14Feat
            {15, new NewCapacite("Tu aime ça ?", "Normal", 30, 9, true, 15) },//15TuAime
            {16, new NewCapacite("Puissance", "Combat", 0, 10, false, 16) },//16puissance
            {17, new NewCapacite("Close Combat", "Combat", 80, 11, false, 17) },//17CloseCombat
            {18,new NewCapacite("Séduction", "Neutre", 0, 12, true, 18) },//18Seduction
            {19, new NewCapacite("Silence Wench", "Normal", 70, 0, false, 19) },//19SilenceWench
            {20, new NewCapacite("Angery", "Neutre", 0, 13, false, 20) },//20Angery
            {21, new NewCapacite("Shibe Is King", "Neutre", 0, 14, false, 21) },//21ShibeIs
            {22, new NewCapacite("CopyClaim", "Copyright", 60, 15, true, 22) },//22CopyClaim
            {23, new NewCapacite("No Pants", "Neutre", 0, 13, false, 23) },//23noPants
            {24, new NewCapacite("Claque Bec", "Normal", 65, 0, false, 24) },//24ClaqueBec
            {25, new NewCapacite("World On Fire", "Feu", 30, 16, true, 25)},//25WorldOnFire
            {26, new NewCapacite("Sacrifice For The Lords", "Feu", 80, 0, true, 26)},//26SacrificeForL
            {27, new NewCapacite("Purification In Fire", "Neutre", 0, 17, false, 27) },//27PurificationInFire
            {28, new NewCapacite("Waahaha", "Inconnue", 10, 14, true, 28) },//28Waahaha
            {29, new NewCapacite("Heyeahyeahyeah", "Inconnu", 40, 19, false, 30) },//29Heyeahyeah
            {30, new NewCapacite("Waluigi Time !", "Inconnu", 20, 18, true, 29) },//30WaluigiTime
            {31, new NewCapacite("Smash", "Inconnu", 70, 0, false, 31)},//31Smash
            {32, new NewCapacite("ne rien faire", "Normal", 0, 0, false, 0) }//32NeRienFaire
        };


        private int HP_, HPMax_, Attaque_, AttaqueSpec_, Defense_, DefenseSpec_;

        #region constructeur
        public NewCreature(string Nom, String Type, int HP, int HPMax, int Attaque, int AttaqueSpec, int Defense, int DefenseSpec, int[] IDcapacite)
        {
            this.Nom = Nom;
            this.Type = Type;

            this.HPMax = HPMax;
            this.HP = HP;
            this.Attaque = Attaque;
            this.AttaqueSpec = AttaqueSpec;
            this.Defense = Defense;
            this.DefenseSpec = DefenseSpec;
            for(i=0; i < 4; i++)
            {

                if (ListCapacite.ContainsKey(IDcapacite[i]))
                {
                    techniques[i] = ListCapacite[IDcapacite[i]];
                    capacite[i] = IDcapacite[i];
                }
                else
                {
                    techniques[i] = ListCapacite[32];
                    capacite[i] = 32;
                }
                    
            }

            /*for (i = 0; i < 4; i++)
            {
                switch (IDcapacite[i])
                {
                    case 1:
                        trempette();
                        break;
                    case 2:
                        comeback();
                        break;
                    case 3:
                        Showbiz();
                        break;
                    case 4:
                        Lancethe();
                        break;
                    case 5:
                        RageOn();
                        break;
                    case 6:
                        SmugFace();
                        break;
                    case 7:
                        CyberHarcel();
                        break;
                    case 8:
                        Invest();
                        break;
                    case 9:
                        LanceFlam();
                        break;
                    case 10:
                        MeteorTesla();
                        break;
                    case 11:
                        ClaqueLias();
                        break;
                    case 12:
                        Weed();
                        break;
                    case 13:
                        NuageTox();
                        break;
                    case 14:
                        Feat();
                        break;
                    case 15:
                        TuAime();
                        break;
                    case 16:
                        puissance();
                        break;
                    case 17:
                        CloseCombat();
                        break;
                    case 18:
                        Seduction();
                        break;
                    case 19:
                        SilenceWench();
                        break;
                    case 20:
                        Angery();
                        break;
                    case 21:
                        ShibeIs();
                        break;
                    case 22:
                        CopyClaim();
                        break;
                    case 23:
                        noPants();
                        break;
                    case 24:
                        ClaqueBec();
                        break;
                    case 25:
                        WorldOnFire();
                        break;
                    case 26:
                        SacrificeForL();
                        break;
                    case 27:
                        PurificationInFire();
                        break;
                    case 28:
                        Waahaha();
                        break;
                    case 29:
                        WaluigiTime();
                        break;
                    case 30:
                        Heyeahyeah();
                        break;
                    case 31:
                        Smash();
                        break;
                    default:
                        nerienfaire();
                        break;
                }
            }*/

        }
        #endregion

        #region accesseur

        public int HP
        {
            get { return HP_; }
            set
            {
                if (value < 0)
                    this.HP_ = 0;
                else if (value > HPMax)
                    this.HP_ = HPMax;
                else
                    this.HP_ = value;
            }
        }

        public int HPMax
        {
            get { return HPMax_; }
            set
            {
                if (value < 0) this.HPMax_ = 0;
                else this.HPMax_ = value;
            }
        }

        public int Attaque
        {
            get { return Attaque_; }
            set
            {
                if (value < 1) this.Attaque_ = 1;
                else this.Attaque_ = value;
            }
        }

        public int AttaqueSpec
        {
            get { return AttaqueSpec_; }
            set
            {
                if (value < 1) this.AttaqueSpec_ = 1;
                else this.AttaqueSpec_ = value;
            }
        }

        public int Defense
        {
            get { return Defense_; }
            set
            {
                if (value < 1) this.Defense_ = 1;
                else this.Defense_ = value;
            }
        }

        public int DefenseSpec
        {
            get { return DefenseSpec_; }
            set
            {
                if (value < 1) this.DefenseSpec_ = 1;
                else this.DefenseSpec_ = value;
            }
        }

        #endregion
        /*
        #region capacité
        private void nerienfaire()
        {
            techniques[i] = new Capacite("ne rien faire", "Normal", 0, 0, false, 0);
        }

        private void comeback()
        {
            techniques[i] = new Capacite("Come back des muppets", "Normal", 70, 0, false, 2);
        }

        private void trempette()
        {
            techniques[i] = new Capacite("trempette", "Neutre", 0, 1, false, 1);
        }

        private void Showbiz()
        {
            techniques[i] = new Capacite("Showbiz", "Neutre", 0, 2, false, 3);
        }

        private void Lancethe()
        {
            techniques[i] = new Capacite("Lance-Thé", "Eau", 65, 0, true, 4);
        }

        private void RageOn()
        {
            techniques[i] = new Capacite("Rage-On", "Psy", 0, 3, true, 5);
        }

        private void SmugFace()
        {
            techniques[i] = new Capacite("Smug Face", "Neutre", 0, 4, true, 6);
        }

        private void CyberHarcel()
        {
            techniques[i] = new Capacite("Cyber-harcèlement", "Psy", 60, 0, true, 7);
        }

        private void Invest()
        {
            
        }

        private void LanceFlam()
        {
            techniques[i] = new Capacite("Lance-Flamme", "Feu", 65, 0, true, 9);
        }

        private void MeteorTesla()
        {
            techniques[i] = new Capacite("Météor Tesla", "Milliardaire", 80, 6, true, 10);
        }

        private void ClaqueLias()
        {
            techniques[i] = new Capacite("Claque de Liasse", "Milliardaire", 60, 0, false, 11);
        }

        private void Weed()
        {
            techniques[i] = new Capacite("Weed?", "Poison", 75, 7, true, 12);
        }

        private void NuageTox()
        {
            techniques[i] = new Capacite("Nuage Toxique", "Poison", 65, 0, true, 13);
        }

        private void Feat()
        {
            techniques[i] = new Capacite("Feat", "Neutre", 0, 8, false, 14);
        }

        private void TuAime()
        {
            techniques[i] = new Capacite("Tu aime ça ?", "Normal", 30, 9, true, 15);
        }

        private void puissance()
        {
            techniques[i] = new Capacite("Puissance", "Combat", 0, 10, false, 16);
        }

        private void CloseCombat()
        {
            techniques[i] = new Capacite("Close Combat", "Combat", 80, 11, false, 17);
        }

        private void Seduction()
        {
            techniques[i] = new Capacite("Séduction", "Neutre", 0, 12, true, 18);
        }

        private void SilenceWench()
        {
            techniques[i] = new Capacite("Silence Wench", "Normal", 70, 0, false, 19);
        }

        private void Angery()
        {
            techniques[i] = new Capacite("Angery", "Neutre", 0, 13, false, 20);
        }

        private void ShibeIs()
        {
            techniques[i] = new Capacite("Shibe Is King", "Neutre", 0, 14, false, 21);
        }

        private void CopyClaim()
        {
            techniques[i] = new Capacite("CopyClaim", "Copyright", 60, 15, true, 22);
        }

        private void noPants()
        {
            techniques[i] = new Capacite("No Pants", "Neutre", 0, 13, false, 23);
        }

        private void ClaqueBec()
        {
            techniques[i] = new Capacite("Claque Bec", "Normal", 65, 0, false, 24);
        }

        private void WorldOnFire()
        {
            techniques[i] = new Capacite("World On Fire", "Feu", 30, 16, true, 25);
        }

        private void SacrificeForL()
        {
            techniques[i] = new Capacite("Sacrifice For The Lords", "Feu", 80, 0, true, 26);
        }

        private void PurificationInFire()
        {
            techniques[i] = new Capacite("Purification In Fire", "Neutre", 0, 17, false, 27);
        }

        private void Waahaha()
        {
            techniques[i] = new Capacite("Waahaha", "Inconnue", 10, 14, true, 28);
        }

        private void WaluigiTime()
        {
            techniques[i] = new Capacite("Waluigi Time !", "Inconnu", 20, 18, true, 29);
        }

        private void Heyeahyeah()
        {
            techniques[i] = new Capacite("Heyeahyeahyeah", "Inconnu", 40, 19, false, 30);
        }

        private void Smash()
        {
            techniques[i] = new Capacite("Smash", "Inconnu", 70, 0, false, 31);
        }

        #endregion
        */
    }
}

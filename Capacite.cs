using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;


namespace JeuxVideo_MemeLegend
{
    public class Capacite
    {
        public string Nom, Type;
        public int IDtech, Puissance, IDEffet;
        public bool NoSp; //false physique, true special


        public Capacite(string Nom, string Type, int Puissance, int IDEffet, bool NoSp, int ID)
        {
            IDtech = ID;
            this.Nom = Nom;
            this.Type = Type;
            this.Puissance = Puissance;
            this.IDEffet = IDEffet;
            this.NoSp = NoSp;
        }

        public int genererdegat(Creature j1, Creature j2)
        {
            int degats = 0;
            if(NoSp == false)
            {
                degats = ((25 * Puissance * j1.Attaque)/(40 * (j2.Defense)));             
            }
            else
            {
                degats = ((25 * Puissance * j1.AttaqueSpec) / ( 40 * (j2.DefenseSpec )));
            }
            degats = (int)(degats * tableType(this.Type, j2.Type));
            return degats;
        }

        public double tableType( string typeAttaque, string typeCible) //table des type, efficacite et faiblesse
        {
            double facteur = 1;

            switch (typeAttaque)
            {
                case "Eau":
                    switch (typeCible)
                    {
                        case "Feu":
                            facteur = 2;
                            break;
                        case "Eau":
                            facteur = 0.5;
                            break;
                        
                        default:
                            break;
                    }
                    break;
                case "Copyright":
                    switch (typeCible)
                    {
                        case "Milliardaire":
                            facteur = 2;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Normal":
                    switch (typeCible)
                    {
                        case "Inconnu":
                            facteur = 2;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Milliardaire":
                    switch (typeCible)
                    {
                        case "Normal":
                            facteur = 2;
                            break;
                        case "Combat":
                            facteur = 2;
                            break;
                        case "Copyright":
                            facteur = 2;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Poison":
                    switch (typeCible)
                    {
                        case "Poison":
                            facteur = 0.5;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Feu":
                    switch (typeCible)
                    {
                        case "Feu":
                            facteur = 0.5;
                            break;
                        case "Eau":
                            facteur = 0.5;
                            break;
                        case "Milliardaire":
                            facteur = 0.5;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Combat":
                    switch (typeCible)
                    {
                        case "Normal":
                            facteur = 2;
                            break;
                        case "Psy":
                            facteur = 0.5;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Psy":
                    switch (typeCible)
                    {
                        case "Combat":
                            facteur = 2;
                            break;
                        case "Inconnu":
                            facteur = 2;
                            break;
                        case "Poison":
                            facteur = 2;
                            break;
                        case "Psy":
                            facteur = 0.5;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Inconnu":
                    switch (typeCible)
                    {
                        case "Psy":
                            facteur = 2;
                            break;
                        case "Milliardaire":
                            facteur = 2;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    facteur = 1;
                    break;
            }

            return facteur;
        }

        public void AppliquerEffet(Creature J1, Creature J2, TextBox boite)
        {
            switch(IDEffet)
            {
                case 1://buff def -15pv
                    ModifierStat(J1, "Def", 40);
                    boite.Text = "la défense de " + J1.Nom + " augmente de 40.";
                    MajBoite(boite);

                    J1.HP -= 15;
                    boite.Text = J1.Nom + " subit 15 point de dégats.";
                    MajBoite(boite);
                    break;

                case 2: //buff attaque - 15pv
                    ModifierStat(J1, "AttSpec", 40);
                    boite.Text = "l'attaque spéciale de " + J1.Nom + " augmente de 40.";
                    MajBoite(boite);

                    J1.HP -= 15;
                    boite.Text = J1.Nom + " subit 15 point de dégats.";
                    MajBoite(boite);
                    break;

                case 3://buff 50 att/spec -30 def rageon
                    ModifierStat(J1, "Att", 40);
                    ModifierStat(J1, "AttaSpec", 40);
                    boite.Text = "l'attaque et l'attaque spéciale de " + J1.Nom + " est augmenter de 40.";
                    MajBoite(boite);

                    ModifierStat(J1, "DefSpec", -25);
                    boite.Text = J1.Nom + " perd 25 de défense spéciale.";
                    MajBoite(boite);
                    break;
                case 4: // j2 -30 att
                    ModifierStat(J2, "Att", -30);
                    boite.Text = J2.Nom + " perd 30 d'attaque.";
                    MajBoite(boite);
                    boite.Text = J2.Nom + " est triggered :D";
                    MajBoite(boite);

                    break;

                case 5: // invest
                    ModifierStat(J1, "AttSpec", 30);
                    boite.Text = J1.Nom + " augmente son attaque spéciale de 30. ";
                    MajBoite(boite);
                    boite.Text = J1.Nom + " : aucun profit n'est à oublier...";
                    MajBoite(boite);
                    break;

                case 6://meteor
                    ModifierStat(J1, "AttSpec", -20);
                    boite.Text = J1.Nom + " perd 20 d'attaque spéciale.";
                    MajBoite(boite);
                    break;

                case 7:
                    ModifierStat(J1, "AttSpec", 10);
                    ModifierStat(J1, "Att", 10);
                    ModifierStat(J1, "Def", 10);
                    ModifierStat(J1, "DefSpec", 10);

                    ModifierStat(J2, "AttSpec", 10);
                    ModifierStat(J2, "Att", 10);
                    ModifierStat(J2, "Def", 10);
                    ModifierStat(J2, "DefSpec", 10);

                    boite.Text = J1.Nom + " et "+ J2.Nom +" en fument peut-être trop ???";
                    MajBoite(boite);
                    boite.Text = J1.Nom + " et " + J2.Nom + " augmentent toute leur stats !!!";
                    MajBoite(boite);
                    break;

                case 8://feat
                    ModifierStat(J1, "Def", 20);
                    ModifierStat(J1, "DefSpec", 20);
                    boite.Text = J1.Nom + " veut faire un feat...";
                    MajBoite(boite);
                    boite.Text = J1.Nom + " augmente sa défense et sa défense spéciale de 20.";
                    MajBoite(boite);
                    break;

                case 9:
                    ModifierStat(J2, "Def", -20);
                    boite.Text = "oui je sais très bien que tu aime ça <3 .";
                    MajBoite(boite);
                    boite.Text = J2.Nom + " voit sa défense diminuer de 20.";
                    MajBoite(boite);
                    break;

                case 10:
                    ModifierStat(J1, "Att", 25);
                    boite.Text = J1.Nom + " fait gonfler ses muscles et voit sa force augmenter de 25";
                    MajBoite(boite);
                    break;

                case 11:
                    ModifierStat(J1, "Def", -15);
                    ModifierStat(J1, "DefSpec", -15);
                    boite.Text = J1.Nom + " voit sa défense et sa défense spéciale diminuer de 15.";
                    MajBoite(boite);

                    break;

                case 12:
                    ModifierStat(J2, "AttSpec", -15);
                    ModifierStat(J2, "Att", -15);
                    boite.Text = J1.Nom + " tente de séduire par ses muscles.";
                    MajBoite(boite);
                    boite.Text = J2.Nom + " voit son attaque et son attaque spéciale spéciale diminuer de 15.";
                    MajBoite(boite);
                    break;

                case 13:
                    ModifierStat(J1, "Att", 30);
                    boite.Text = J1.Nom + " augmente son attaque de 20.";
                    MajBoite(boite);
                    break;

                case 14:
                    ModifierStat(J1, "AttSpec", 10);
                    ModifierStat(J1, "Att", 10);
                    ModifierStat(J1, "Def", 10);
                    ModifierStat(J1, "DefSpec", 10);
                    boite.Text = J1.Nom + " augmente toutes ses stats de 10.";
                    MajBoite(boite);
                    break;

                case 15:
                    ModifierStat(J2, "Def", -10);
                    ModifierStat(J2, "DefSpec", -10);
                    boite.Text = J2.Nom + " voit sa défense et sa défense spéciale diminuer de 10.";
                    MajBoite(boite);
                    break;

                case 16:
                    ModifierStat(J1, "AttSpec", 10);
                    ModifierStat(J1, "Att", 10);
                    boite.Text = J1.Nom + " voit son attaque et son attaque spéciale diminuer de 10.";
                    MajBoite(boite);
                    break;

                case 17:
                    ModifierStat(J1, "HP", 65);
                    boite.Text = J1.Nom + " se soigne de 65 HP.";
                    MajBoite(boite);
                    ModifierStat(J2, "Def", -10);
                    boite.Text = J1.Nom + " voit sa défense diminuer de 10.";
                    MajBoite(boite);
                    break;

                case 18:
                    ModifierStat(J1, "AttSpec", 20);
                    ModifierStat(J1, "Att", 20);
                    ModifierStat(J1, "Def", -10);
                    ModifierStat(J1, "DefSpec", -10);
                    boite.Text = J1.Nom + " passe à l'offensive !!!";
                    MajBoite(boite);
                    break;

                case 19:
                    ModifierStat(J2, "Def", -15);
                    boite.Text = J2.Nom + " voit sa défense de diminuer de 15.";
                    MajBoite(boite);
                    break;

                default:
                    break;
            }
        }

        private void MajBoite(TextBox boite)
        {
            boite.Refresh();
            Application.DoEvents();
            Thread.Sleep(1500);
        }

        private void ModifierStat(Creature J1,string stat, int modif)
        {
            switch (stat)
            {          
                case "Att":
                    J1.Attaque += modif;
                    break;
                case "AttSpec":
                    J1.AttaqueSpec += modif;
                    break;
                case "Def":
                    J1.Defense += modif;
                    break;
                case "DefSpec":
                    J1.DefenseSpec += modif;
                    break;
                case "HP":
                    J1.HP += modif;
                    break;
                default:
                    break;
            }
        }

    }
}

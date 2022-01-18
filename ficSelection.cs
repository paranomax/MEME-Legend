using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Media;

namespace JeuxVideo_MemeLegend
{
    public partial class EcranSelection : Form
    {
        bool joueur = true;
        int i = 0;
        Creature creature;
        Creature[] joueur1 = new Creature[3];
        Creature[] joueur2 = new Creature[3];

        //System.Media.SoundPlayer menu; chercher pour mettre un son

        public EcranSelection()
        {
            InitializeComponent();           
        }

        
        private void EcranSelection_Load_1(object sender, EventArgs e)
        {
            bLancer.Enabled = false;
            pbKermit_Click(null, null);
        }

        private void bQuitter_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "souhaitez-vous réellement quitter ???", "Votre souhait",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
            
        }

        private void bAjouter_Click(object sender, EventArgs e)
        {
            if (joueur == true)
            {
                joueur1[i] = creature;
                joueur = false;
                lChoisi.Text = "▼Joueur2▼";
                lChoisi.BackColor = Color.RoyalBlue;
                clbJ1.Items.Add(creature.Nom);
            }            
            else
            {
                joueur2[i] = creature;
                joueur = true;
                clbJ2.Items.Add(creature.Nom);
                i++;
                lChoisi.Text = "▲Joueur1▲";
                lChoisi.BackColor = Color.OrangeRed;
                if(i==3)
                {
                    bLancer.Enabled = true;
                    bAjouter.Enabled = false;
                }
            }
            pbKermit_Click(null, null);               
        }
        

        private void pbKermit_Click(object sender, EventArgs e)
        {
            creature = new Creature("Kermit", "Eau", 100, 100, 50, 70, 80, 85, new int[4] {1, 2, 3, 4 } );
            majInfo();
        }

        private void pbDonald_Click(object sender, EventArgs e)
        {
            creature = new Creature("Donald Duck", "Copyright", 90, 90, 65, 70, 70, 90, new int[4] { 22, 23, 5, 24 });
            majInfo();
        }

        private void pbdoggo_Click(object sender, EventArgs e)
        {
            creature = new Creature("Doggo", "Normal", 120, 120, 85, 65, 65, 70, new int[4] { 6, 20, 21, 19 });
            majInfo();
        }

        private void pbElon_Click(object sender, EventArgs e)
        {
            creature = new Creature("Elon Musk", "Milliardaire", 90, 85, 75, 90, 65, 65, new int[4] { 8, 9, 10, 11 });
            majInfo();
        }

        private void pbSnoop_Click(object sender, EventArgs e)
        {
            creature = new Creature("Snoop Dogg", "Poison", 110, 110, 50, 80, 85, 95, new int[4] { 12, 13, 14, 11 });
            majInfo();
        }

        private void pbElmo_Click(object sender, EventArgs e)
        {
            creature = new Creature("Elmo", "Feu", 90, 90, 80, 95, 65, 65, new int[4] { 9, 25, 26, 27 });
            majInfo();
        }

        private void pbRicardo_Click(object sender, EventArgs e)
        {
            creature = new Creature("Ricardo", "Combat", 100, 100, 100, 70, 90, 60, new int[4] { 15, 16, 17, 18 });
            majInfo();
        }

        private void pbPepe_Click(object sender, EventArgs e)
        {
            creature = new Creature("Pepe", "Psy", 80, 80, 75, 95, 65, 90, new int[4] { 5, 6, 7, 1 });
            majInfo();
        }

        private void PbWaluigi_Click(object sender, EventArgs e)
        {
            creature = new Creature("Waluigi", "Inconnu", 130, 130, 100, 100, 50, 45, new int[4] { 28, 29, 30, 31 });
            majInfo();
        }

        private void majInfo()
        {
            lNoma.Text = creature.Nom;
            lTypea.Text = creature.Type;
            lPVa.Text = creature.HPMax.ToString();
            lAtta.Text = creature.Attaque.ToString();
            lAttSpeca.Text = creature.AttaqueSpec.ToString();
            lDefa.Text = creature.Defense.ToString();
            lDefSpeca.Text = creature.DefenseSpec.ToString();
            lCapacite1.Text = creature.techniques[0].Nom;
            lCapacite2.Text = creature.techniques[1].Nom;
            lCapacite3.Text = creature.techniques[2].Nom;
            lCapacite4.Text = creature.techniques[3].Nom;
        }

        private void bLancer_Click(object sender, EventArgs e)
        {
            EcranCombat f = new EcranCombat(joueur1, joueur2, true); 
            Hide();
            f.ShowDialog();
            Close();
        }

        
    }
}

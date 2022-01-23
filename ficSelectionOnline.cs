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
    public partial class ficSelectionOnline : Form
    {
        int i = 0;
        NewCreature NewCreature;
        NewCreature[] Joueur = new NewCreature[3];

        public ficSelectionOnline()
        {
            InitializeComponent();
        }
        private void ficSelectionOnline_Load(object sender, EventArgs e)
        {
            tbServeur.Text = System.Environment.MachineName;
            pbKermit_Click(null, null);
            cbServeur.SelectedIndex = 0;
        }

        private void pbKermit_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Kermit", "Eau", 100, 100, 50, 70, 80, 85, new int[4] { 1, 2, 3, 4});
            majInfo();
        }

        private void pbDonald_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Donald Duck", "Copyright", 90, 90, 65, 70, 70, 90, new int[4] { 22, 23, 5, 24});
            majInfo();
        }

        private void pbdoggo_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Doggo", "Normal", 120, 120, 85, 65, 65, 70, new int[4] { 6, 19,20, 21 });
            majInfo();
        }

        private void pbElon_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Elon Musk", "Milliardaire", 90, 85, 75, 90, 65, 65, new int[4] { 8, 9, 10, 11 });
            majInfo();
        }

        private void pbSnoop_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Snoop Dogg", "Poison", 110, 110, 50, 80, 85, 95, new int[4] { 12, 13, 14, 11 });
            majInfo();
        }

        private void pbElmo_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Elmo", "Feu", 90, 90, 80, 95, 65, 65, new int[4] { 9, 25, 26, 27 });
            majInfo();
        }

        private void pbRicardo_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Ricardo", "Combat", 100, 100, 100, 70, 90, 60, new int[4] { 15, 16, 17, 18 });
            majInfo();
        }

        private void pbPepe_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Pepe", "Psy", 80, 80, 75, 95, 65, 90, new int[4] { 5, 6, 7, 1 });
            majInfo();
        }

        private void PbWaluigi_Click(object sender, EventArgs e)
        {
            NewCreature = new NewCreature("Waluigi", "Inconnu", 130, 130, 100, 100, 50, 45, new int[4] { 28, 29, 30, 31});
            majInfo();
        }

        private void bAjouter_Click(object sender, EventArgs e)
        {
            
            Joueur[i] = NewCreature;
            clbJ1.Items.Add(NewCreature.Nom);
            i++;
            if (i == 3)
            {
                bLancer.Enabled = true;
                bAjouter.Enabled = false;
            }
            pbKermit_Click(null, null);
        }

        private void majInfo()
        {
            lNoma.Text = NewCreature.Nom;
            lTypea.Text = NewCreature.Type;
            lPVa.Text = NewCreature.HPMax.ToString();
            lAtta.Text = NewCreature.Attaque.ToString();
            lAttSpeca.Text = NewCreature.AttaqueSpec.ToString();
            lDefa.Text = NewCreature.Defense.ToString();
            lDefSpeca.Text = NewCreature.DefenseSpec.ToString();
            lCapacite1.Text = NewCreature.techniques[0].Nom;
            lCapacite2.Text = NewCreature.techniques[1].Nom;
            lCapacite3.Text = NewCreature.techniques[2].Nom;
            lCapacite4.Text = NewCreature.techniques[3].Nom;
        }

        private void bLancer_Click(object sender, EventArgs e)
        {
            if(cbServeur.SelectedItem.ToString() == "Serveur")
            {
                Hide();
                ficCombatOnline f = new ficCombatOnline(Joueur, true, tbServeur.Text);
                f.ShowDialog();
                Show();
            }
            else
            {
                Hide();
                ficCombatOnline f = new ficCombatOnline(Joueur, false, tbServeur.Text);
                f.ShowDialog();
                Show();
            }
            
        }

        private void ficSelectionOnline_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}

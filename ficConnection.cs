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
    public partial class ficConnection : Form
    {
        public ficConnection()
        {
            InitializeComponent();
            tbServeur.Text = System.Environment.MachineName;
        }

        private void cbServeur_CheckedChanged(object sender, EventArgs e)
        {
            if (cbServeur.Checked == true)
                cbClient.Checked = false;
        }

        private void cbClient_CheckedChanged(object sender, EventArgs e)
        {
            if (cbClient.Checked == true)
                cbServeur.Checked = false;
        }

        private void bLancer_Click(object sender, EventArgs e)
        {
            if ((tbServeur.Text != string.Empty) && (cbClient.Checked = true))
            {
                //creer ecran de selection avec la configuration
            }
            else if ((tbServeur.Text != string.Empty) && (cbServeur.Checked = true))
            {

            }
            else
                MessageBox.Show("erreur de configuration");
           
        }

        private void ficConnection_Load(object sender, EventArgs e)
        {

        }
    }
}

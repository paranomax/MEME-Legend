using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace JeuxVideo_MemeLegend
{
    public partial class fPrincipale : Form
    {
        private string sFichier;
        private Creature[] joueur1 = new Creature[3];
        private Creature[] joueur2 = new Creature[3];
        //private Socket sClient, sServeur;
        //private byte[] bBuffer;
        public fPrincipale()
        {
            InitializeComponent();
            /*sServeur = null;
            sClient = null;
            bBuffer = new byte[256];*/
        }

        private void bLocal_Click(object sender, EventArgs e)
        {
            EcranSelection f = new EcranSelection();
            Hide();
            f.ShowDialog();
            Show();
        }


        private void lQuitter_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bCharger_Click(object sender, EventArgs e)
        {
            EcranCombat f;
            string[] noms;
            string sLecture;
            int tour;
            if (ofdOuvrir.ShowDialog() == DialogResult.OK)
            {
                sFichier = ofdOuvrir.FileName;
                StreamReader sr = new StreamReader(sFichier);

                
                try
                {
                    for (int i = 0; i < 3; i++)
                    {
                        sLecture = sr.ReadLine();
                        noms = sLecture.Split(';');

                        joueur1[i] = new Creature(noms[0], 
                                                  noms[1],
                                                  Int32.Parse(noms[2]), 
                                                  Int32.Parse(noms[3]),
                                                  Int32.Parse(noms[4]),
                                                  Int32.Parse(noms[5]),
                                                  Int32.Parse(noms[6]),
                                                  Int32.Parse(noms[7]),
                                                  new int[4] { Int32.Parse(noms[8]), Int32.Parse(noms[9]), Int32.Parse(noms[10]), Int32.Parse(noms[11]) });
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        sLecture = sr.ReadLine();
                        noms = sLecture.Split(';');

                        joueur2[i] = new Creature(noms[0],
                                                  noms[1],
                                                  Int32.Parse(noms[2]),
                                                  Int32.Parse(noms[3]),
                                                  Int32.Parse(noms[4]),
                                                  Int32.Parse(noms[5]),
                                                  Int32.Parse(noms[6]),
                                                  Int32.Parse(noms[7]),
                                                  new int[4] { Int32.Parse(noms[8]), Int32.Parse(noms[9]), Int32.Parse(noms[10]), Int32.Parse(noms[11]) });
                    }
                    tour = Int32.Parse(sr.ReadLine());

                    if(tour == 1)
                         f = new EcranCombat(joueur1, joueur2, true);
                    else
                         f = new EcranCombat(joueur1, joueur2, false);
                    Hide();
                    f.ShowDialog();
                    Show();
                }
                catch
                {
                     MessageBox.Show("erreur de chargement");
                }
                sr.Close();
            }
        }

        private void bServeur_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Création d'une partie");
        }

        private void cClient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vous allez rejoindre une partie");
        }

        private void bOnline_Click(object sender, EventArgs e)
        {
            ficConnection f = new ficConnection();
            Hide();
            f.ShowDialog();
            Show();
        }
    }
}

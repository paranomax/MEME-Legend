namespace JeuxVideo_MemeLegend
{
    partial class fPrincipale
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.lTitre = new System.Windows.Forms.Label();
            this.bLocal = new System.Windows.Forms.Button();
            this.lQuitter = new System.Windows.Forms.Label();
            this.bCharger = new System.Windows.Forms.Button();
            this.ofdOuvrir = new System.Windows.Forms.OpenFileDialog();
            this.bOnline = new System.Windows.Forms.Button();
            this.bChargerOnline = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lTitre
            // 
            this.lTitre.AutoSize = true;
            this.lTitre.BackColor = System.Drawing.Color.GreenYellow;
            this.lTitre.Font = new System.Drawing.Font("Minecraftia", 16.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitre.ForeColor = System.Drawing.Color.DarkGreen;
            this.lTitre.Location = new System.Drawing.Point(102, 61);
            this.lTitre.Name = "lTitre";
            this.lTitre.Size = new System.Drawing.Size(239, 49);
            this.lTitre.TabIndex = 0;
            this.lTitre.Text = "MEME Legend";
            // 
            // bLocal
            // 
            this.bLocal.Font = new System.Drawing.Font("Minecraftia", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLocal.Location = new System.Drawing.Point(124, 149);
            this.bLocal.Name = "bLocal";
            this.bLocal.Size = new System.Drawing.Size(183, 38);
            this.bLocal.TabIndex = 1;
            this.bLocal.Text = "Jouer en local";
            this.bLocal.UseVisualStyleBackColor = true;
            this.bLocal.Click += new System.EventHandler(this.bLocal_Click);
            // 
            // lQuitter
            // 
            this.lQuitter.AutoSize = true;
            this.lQuitter.BackColor = System.Drawing.Color.CadetBlue;
            this.lQuitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuitter.Location = new System.Drawing.Point(349, 365);
            this.lQuitter.Name = "lQuitter";
            this.lQuitter.Size = new System.Drawing.Size(60, 20);
            this.lQuitter.TabIndex = 3;
            this.lQuitter.Text = "Quitter";
            this.lQuitter.Click += new System.EventHandler(this.lQuitter_Click);
            // 
            // bCharger
            // 
            this.bCharger.Font = new System.Drawing.Font("Minecraftia", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bCharger.Location = new System.Drawing.Point(124, 237);
            this.bCharger.Name = "bCharger";
            this.bCharger.Size = new System.Drawing.Size(183, 38);
            this.bCharger.TabIndex = 4;
            this.bCharger.Text = "Charger un partie";
            this.bCharger.UseVisualStyleBackColor = true;
            this.bCharger.Click += new System.EventHandler(this.bCharger_Click);
            // 
            // ofdOuvrir
            // 
            this.ofdOuvrir.FileName = "openFileDialog1";
            // 
            // bOnline
            // 
            this.bOnline.Font = new System.Drawing.Font("Minecraftia", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bOnline.Location = new System.Drawing.Point(124, 193);
            this.bOnline.Name = "bOnline";
            this.bOnline.Size = new System.Drawing.Size(183, 38);
            this.bOnline.TabIndex = 5;
            this.bOnline.Text = "Jouer en ligne";
            this.bOnline.UseVisualStyleBackColor = true;
            this.bOnline.Click += new System.EventHandler(this.bOnline_Click);
            // 
            // bChargerOnline
            // 
            this.bChargerOnline.Font = new System.Drawing.Font("Minecraftia", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bChargerOnline.Location = new System.Drawing.Point(124, 281);
            this.bChargerOnline.Name = "bChargerOnline";
            this.bChargerOnline.Size = new System.Drawing.Size(183, 56);
            this.bChargerOnline.TabIndex = 6;
            this.bChargerOnline.Text = "Charger un partie en ligne";
            this.bChargerOnline.UseVisualStyleBackColor = true;
            // 
            // fPrincipale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.ClientSize = new System.Drawing.Size(432, 403);
            this.Controls.Add(this.bChargerOnline);
            this.Controls.Add(this.bOnline);
            this.Controls.Add(this.bCharger);
            this.Controls.Add(this.lQuitter);
            this.Controls.Add(this.bLocal);
            this.Controls.Add(this.lTitre);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fPrincipale";
            this.Text = "MEME LEGEND";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lTitre;
        private System.Windows.Forms.Button bLocal;
        private System.Windows.Forms.Label lQuitter;
        private System.Windows.Forms.Button bCharger;
        private System.Windows.Forms.OpenFileDialog ofdOuvrir;
        private System.Windows.Forms.Button bOnline;
        private System.Windows.Forms.Button bChargerOnline;
    }
}


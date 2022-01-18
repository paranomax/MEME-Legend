
namespace JeuxVideo_MemeLegend
{
    partial class ficConnection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lServeur = new System.Windows.Forms.Label();
            this.tbServeur = new System.Windows.Forms.TextBox();
            this.cbServeur = new System.Windows.Forms.CheckBox();
            this.cbClient = new System.Windows.Forms.CheckBox();
            this.bLancer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lServeur
            // 
            this.lServeur.AutoSize = true;
            this.lServeur.Location = new System.Drawing.Point(13, 13);
            this.lServeur.Name = "lServeur";
            this.lServeur.Size = new System.Drawing.Size(58, 17);
            this.lServeur.TabIndex = 0;
            this.lServeur.Text = "Serveur";
            // 
            // tbServeur
            // 
            this.tbServeur.Location = new System.Drawing.Point(16, 33);
            this.tbServeur.Name = "tbServeur";
            this.tbServeur.Size = new System.Drawing.Size(321, 22);
            this.tbServeur.TabIndex = 1;
            // 
            // cbServeur
            // 
            this.cbServeur.AutoSize = true;
            this.cbServeur.Checked = true;
            this.cbServeur.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbServeur.Location = new System.Drawing.Point(239, 61);
            this.cbServeur.Name = "cbServeur";
            this.cbServeur.Size = new System.Drawing.Size(80, 21);
            this.cbServeur.TabIndex = 2;
            this.cbServeur.Text = "Serveur";
            this.cbServeur.UseVisualStyleBackColor = true;
            this.cbServeur.CheckedChanged += new System.EventHandler(this.cbServeur_CheckedChanged);
            // 
            // cbClient
            // 
            this.cbClient.AutoSize = true;
            this.cbClient.Location = new System.Drawing.Point(239, 88);
            this.cbClient.Name = "cbClient";
            this.cbClient.Size = new System.Drawing.Size(65, 21);
            this.cbClient.TabIndex = 3;
            this.cbClient.Text = "Client";
            this.cbClient.UseVisualStyleBackColor = true;
            this.cbClient.CheckedChanged += new System.EventHandler(this.cbClient_CheckedChanged);
            // 
            // bLancer
            // 
            this.bLancer.Location = new System.Drawing.Point(16, 62);
            this.bLancer.Name = "bLancer";
            this.bLancer.Size = new System.Drawing.Size(121, 47);
            this.bLancer.TabIndex = 4;
            this.bLancer.Text = "Lancer la partie";
            this.bLancer.UseVisualStyleBackColor = true;
            this.bLancer.Click += new System.EventHandler(this.bLancer_Click);
            // 
            // ficConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.ClientSize = new System.Drawing.Size(357, 135);
            this.Controls.Add(this.bLancer);
            this.Controls.Add(this.cbClient);
            this.Controls.Add(this.cbServeur);
            this.Controls.Add(this.tbServeur);
            this.Controls.Add(this.lServeur);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ficConnection";
            this.Text = "Ecran de connexion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lServeur;
        private System.Windows.Forms.TextBox tbServeur;
        private System.Windows.Forms.CheckBox cbServeur;
        private System.Windows.Forms.CheckBox cbClient;
        private System.Windows.Forms.Button bLancer;
    }
}
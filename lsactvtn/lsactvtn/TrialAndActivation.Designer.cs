namespace lsactvtn
{
    partial class TrialAndActivation
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lDemo = new System.Windows.Forms.Label();
            this.lAcheter = new System.Windows.Forms.LinkLabel();
            this.bDemo = new System.Windows.Forms.Button();
            this.bActiver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lDemo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lAcheter);
            this.splitContainer1.Panel2.Controls.Add(this.bDemo);
            this.splitContainer1.Panel2.Controls.Add(this.bActiver);
            this.splitContainer1.Size = new System.Drawing.Size(273, 266);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 0;
            // 
            // lDemo
            // 
            this.lDemo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lDemo.Location = new System.Drawing.Point(12, 27);
            this.lDemo.Name = "lDemo";
            this.lDemo.Size = new System.Drawing.Size(249, 100);
            this.lDemo.TabIndex = 0;
            this.lDemo.Text = "lDemo";
            // 
            // lAcheter
            // 
            this.lAcheter.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lAcheter.Location = new System.Drawing.Point(12, 10);
            this.lAcheter.Name = "lAcheter";
            this.lAcheter.Size = new System.Drawing.Size(109, 95);
            this.lAcheter.TabIndex = 2;
            this.lAcheter.TabStop = true;
            this.lAcheter.Text = "Acheter";
            this.lAcheter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lAcheter_LinkClicked);
            // 
            // bDemo
            // 
            this.bDemo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.bDemo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bDemo.Location = new System.Drawing.Point(127, 10);
            this.bDemo.Name = "bDemo";
            this.bDemo.Size = new System.Drawing.Size(134, 47);
            this.bDemo.TabIndex = 1;
            this.bDemo.Text = "Continuer à utiliser la version Démo";
            this.bDemo.UseVisualStyleBackColor = true;
            this.bDemo.Click += new System.EventHandler(this.bDemo_Click);
            // 
            // bActiver
            // 
            this.bActiver.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.bActiver.Location = new System.Drawing.Point(127, 58);
            this.bActiver.Name = "bActiver";
            this.bActiver.Size = new System.Drawing.Size(134, 47);
            this.bActiver.TabIndex = 0;
            this.bActiver.Text = "Activer la version Pro";// "Introduire un code d\'activation";
            this.bActiver.UseVisualStyleBackColor = true;
            this.bActiver.Click += new System.EventHandler(this.bActiver_Click);
            // 
            // TrialAndActivation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 266);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrialAndActivation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control de licence";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lDemo;
        private System.Windows.Forms.Button bDemo;
        private System.Windows.Forms.Button bActiver;
        private System.Windows.Forms.LinkLabel lAcheter;
    }
}
namespace gmadget2
{
    partial class gmadui
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGo = new System.Windows.Forms.Button();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblWorkshopURL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(713, 6);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(103, 6);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(604, 23);
            this.txtURL.TabIndex = 1;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.ForeColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(12, 35);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(776, 403);
            this.txtLog.TabIndex = 2;
            // 
            // lblWorkshopURL
            // 
            this.lblWorkshopURL.AutoSize = true;
            this.lblWorkshopURL.Location = new System.Drawing.Point(12, 9);
            this.lblWorkshopURL.Name = "lblWorkshopURL";
            this.lblWorkshopURL.Size = new System.Drawing.Size(85, 15);
            this.lblWorkshopURL.TabIndex = 3;
            this.lblWorkshopURL.Text = "Workshop URL";
            // 
            // gmadui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblWorkshopURL);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.btnGo);
            this.Name = "gmadui";
            this.Text = "gmadget2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnGo;
        private TextBox txtURL;
        private TextBox txtLog;
        private Label lblWorkshopURL;
    }
}
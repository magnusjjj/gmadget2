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
            this.lblWorkshopURL = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.lblOutputLog = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(840, 6);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(242, 23);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(103, 6);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(731, 23);
            this.txtURL.TabIndex = 1;
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
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.ForeColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(12, 51);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1070, 503);
            this.txtLog.TabIndex = 4;
            this.txtLog.Text = "";
            // 
            // lblOutputLog
            // 
            this.lblOutputLog.AutoSize = true;
            this.lblOutputLog.Location = new System.Drawing.Point(12, 33);
            this.lblOutputLog.Name = "lblOutputLog";
            this.lblOutputLog.Size = new System.Drawing.Size(68, 15);
            this.lblOutputLog.TabIndex = 5;
            this.lblOutputLog.Text = "Output log:";
            // 
            // gmadui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 567);
            this.Controls.Add(this.lblOutputLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblWorkshopURL);
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
        private Label lblWorkshopURL;
        private RichTextBox txtLog;
        private Label lblOutputLog;
    }
}
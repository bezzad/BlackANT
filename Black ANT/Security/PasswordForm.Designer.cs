namespace Security
{
    partial class PasswordForm
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPassword = new DVTextBox.DVTextBox(this.components);
            this.txtVolumeLabel = new System.Windows.Forms.Label();
            this.btnMove = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.txtVolumeLabel);
            this.panel1.Location = new System.Drawing.Point(12, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 127);
            this.panel1.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtPassword.DefaultValue = "Enter Password";
            this.txtPassword.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtPassword.EnterToTab = false;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.Location = new System.Drawing.Point(35, 70);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(191, 26);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "Enter Password";
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassword.TextForeColor = System.Drawing.Color.Black;
            // 
            // txtVolumeLabel
            // 
            this.txtVolumeLabel.AutoEllipsis = true;
            this.txtVolumeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtVolumeLabel.ForeColor = System.Drawing.Color.Crimson;
            this.txtVolumeLabel.Location = new System.Drawing.Point(35, 34);
            this.txtVolumeLabel.Name = "txtVolumeLabel";
            this.txtVolumeLabel.Size = new System.Drawing.Size(191, 24);
            this.txtVolumeLabel.TabIndex = 0;
            this.txtVolumeLabel.Text = "Volume Label";
            this.txtVolumeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtVolumeLabel.UseCompatibleTextRendering = true;
            // 
            // btnMove
            // 
            this.btnMove.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMove.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.btnMove.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnMove.Location = new System.Drawing.Point(27, 2);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(105, 34);
            this.btnMove.TabIndex = 0;
            this.btnMove.TabStop = false;
            this.btnMove.Text = "Authentication";
            this.btnMove.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Location = new System.Drawing.Point(38, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(99, 31);
            this.panel2.TabIndex = 0;
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 165);
            this.ControlBox = false;
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PasswordForm";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label txtVolumeLabel;
        private DVTextBox.DVTextBox txtPassword;

    }
}
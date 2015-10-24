namespace Security
{
    partial class ChangePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePassword));
            this.txtCurrentPass = new System.Windows.Forms.TextBox();
            this.txtNewPass = new System.Windows.Forms.TextBox();
            this.txtReenterPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pbStateCurrentPass = new PictureBoxStatus.PicBoxStatus();
            this.pbStateReenterPass = new PictureBoxStatus.PicBoxStatus();
            this.lblStrength = new System.Windows.Forms.Label();
            this.btnPassReveal = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCurrentPass
            // 
            this.txtCurrentPass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.txtCurrentPass, 3);
            this.txtCurrentPass.Location = new System.Drawing.Point(173, 23);
            this.txtCurrentPass.MaxLength = 100;
            this.txtCurrentPass.Name = "txtCurrentPass";
            this.txtCurrentPass.ShortcutsEnabled = false;
            this.txtCurrentPass.Size = new System.Drawing.Size(183, 23);
            this.txtCurrentPass.TabIndex = 0;
            this.txtCurrentPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCurrentPass.UseSystemPasswordChar = true;
            this.txtCurrentPass.WordWrap = false;
            // 
            // txtNewPass
            // 
            this.txtNewPass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.txtNewPass, 3);
            this.txtNewPass.Location = new System.Drawing.Point(173, 79);
            this.txtNewPass.MaxLength = 100;
            this.txtNewPass.Name = "txtNewPass";
            this.txtNewPass.ShortcutsEnabled = false;
            this.txtNewPass.Size = new System.Drawing.Size(183, 23);
            this.txtNewPass.TabIndex = 1;
            this.txtNewPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNewPass.UseSystemPasswordChar = true;
            this.txtNewPass.WordWrap = false;
            // 
            // txtReenterPass
            // 
            this.txtReenterPass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.txtReenterPass, 3);
            this.txtReenterPass.Location = new System.Drawing.Point(173, 107);
            this.txtReenterPass.MaxLength = 100;
            this.txtReenterPass.Name = "txtReenterPass";
            this.txtReenterPass.ShortcutsEnabled = false;
            this.txtReenterPass.Size = new System.Drawing.Size(183, 23);
            this.txtReenterPass.TabIndex = 2;
            this.txtReenterPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtReenterPass.UseSystemPasswordChar = true;
            this.txtReenterPass.WordWrap = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Password:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(23, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "New Password:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(23, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "Reenter Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.UseCompatibleTextRendering = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.AutoSize = true;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(173, 163);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(84, 34);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(272, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 34);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.61905F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.61905F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.txtReenterPass, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.txtNewPass, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.label2, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.txtCurrentPass, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.label3, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.btnOk, 2, 6);
            this.tableLayoutPanel.Controls.Add(this.btnCancel, 4, 6);
            this.tableLayoutPanel.Controls.Add(this.pbStateCurrentPass, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.pbStateReenterPass, 5, 4);
            this.tableLayoutPanel.Controls.Add(this.lblStrength, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.btnPassReveal, 5, 2);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(420, 220);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pbStateCurrentPass
            // 
            this.pbStateCurrentPass.BackColor = System.Drawing.Color.Transparent;
            this.pbStateCurrentPass.CurrentStatus = PictureBoxStatus.ImageState.NotLoad;
            this.pbStateCurrentPass.ImageLoaded = ((System.Drawing.Bitmap)(resources.GetObject("pbStateCurrentPass.ImageLoaded")));
            this.pbStateCurrentPass.ImageLoading = ((System.Drawing.Bitmap)(resources.GetObject("pbStateCurrentPass.ImageLoading")));
            this.pbStateCurrentPass.ImageNotLoad = ((System.Drawing.Bitmap)(resources.GetObject("pbStateCurrentPass.ImageNotLoad")));
            this.pbStateCurrentPass.LoadingAnimateSpeed = 1000;
            this.pbStateCurrentPass.LoadingImageAnimated = false;
            this.pbStateCurrentPass.Location = new System.Drawing.Point(363, 23);
            this.pbStateCurrentPass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbStateCurrentPass.Name = "pbStateCurrentPass";
            this.pbStateCurrentPass.Size = new System.Drawing.Size(27, 22);
            this.pbStateCurrentPass.TabIndex = 0;
            this.pbStateCurrentPass.TabStop = false;
            // 
            // pbStateReenterPass
            // 
            this.pbStateReenterPass.BackColor = System.Drawing.Color.Transparent;
            this.pbStateReenterPass.CurrentStatus = PictureBoxStatus.ImageState.NotLoad;
            this.pbStateReenterPass.ImageLoaded = ((System.Drawing.Bitmap)(resources.GetObject("pbStateReenterPass.ImageLoaded")));
            this.pbStateReenterPass.ImageLoading = ((System.Drawing.Bitmap)(resources.GetObject("pbStateReenterPass.ImageLoading")));
            this.pbStateReenterPass.ImageNotLoad = ((System.Drawing.Bitmap)(resources.GetObject("pbStateReenterPass.ImageNotLoad")));
            this.pbStateReenterPass.LoadingAnimateSpeed = 1000;
            this.pbStateReenterPass.LoadingImageAnimated = false;
            this.pbStateReenterPass.Location = new System.Drawing.Point(363, 107);
            this.pbStateReenterPass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbStateReenterPass.Name = "pbStateReenterPass";
            this.pbStateReenterPass.Size = new System.Drawing.Size(27, 22);
            this.pbStateReenterPass.TabIndex = 0;
            this.pbStateReenterPass.TabStop = false;
            // 
            // lblStrength
            // 
            this.lblStrength.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStrength.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel.SetColumnSpan(this.lblStrength, 3);
            this.lblStrength.Location = new System.Drawing.Point(173, 48);
            this.lblStrength.Name = "lblStrength";
            this.lblStrength.Size = new System.Drawing.Size(183, 28);
            this.lblStrength.TabIndex = 0;
            this.lblStrength.Text = "Very Weak";
            this.lblStrength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPassReveal
            // 
            this.btnPassReveal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPassReveal.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPassReveal.BackColor = System.Drawing.Color.Transparent;
            this.btnPassReveal.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPassReveal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPassReveal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPassReveal.Font = new System.Drawing.Font("Lucida Sans Typewriter", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassReveal.Location = new System.Drawing.Point(362, 57);
            this.btnPassReveal.Name = "btnPassReveal";
            this.tableLayoutPanel.SetRowSpan(this.btnPassReveal, 2);
            this.btnPassReveal.Size = new System.Drawing.Size(29, 37);
            this.btnPassReveal.TabIndex = 0;
            this.btnPassReveal.TabStop = false;
            this.btnPassReveal.Text = "₱";
            this.btnPassReveal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPassReveal.UseVisualStyleBackColor = false;
            this.btnPassReveal.CheckedChanged += new System.EventHandler(this.btnPassReveal_CheckedChanged);
            // 
            // ChangePassword
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(420, 220);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Lucida Sans Typewriter", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePassword";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
            this.TopMost = true;
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCurrentPass;
        private System.Windows.Forms.TextBox txtNewPass;
        private System.Windows.Forms.TextBox txtReenterPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private PictureBoxStatus.PicBoxStatus pbStateCurrentPass;
        private PictureBoxStatus.PicBoxStatus pbStateReenterPass;
        private System.Windows.Forms.Label lblStrength;
        private System.Windows.Forms.CheckBox btnPassReveal;
    }
}
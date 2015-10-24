namespace Black_ANT
{
    partial class GUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.gridViewReport = new System.Windows.Forms.DataGridView();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prbMaster = new System.Windows.Forms.ProgressBar();
            this.prbSlave = new System.Windows.Forms.ProgressBar();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.fileExtensionUserControl = new FileExtensionUserControl.FileExtensionUserControl();
            this.grbAppearance = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.txtVolumeLabel = new DVTextBox.DVTextBox(this.components);
            this.pbSearchState = new PictureBoxStatus.PicBoxStatus();
            this.label1 = new System.Windows.Forms.Label();
            this.pbMassStorageState = new PictureBoxStatus.PicBoxStatus();
            this.label2 = new System.Windows.Forms.Label();
            this.gbReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReport)).BeginInit();
            this.grbAppearance.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Location = new System.Drawing.Point(722, 599);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 50);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "&Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnAbout.Location = new System.Drawing.Point(12, 599);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(50, 50);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "!";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // gbReport
            // 
            this.gbReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbReport.Controls.Add(this.gridViewReport);
            this.gbReport.Location = new System.Drawing.Point(12, 274);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(760, 319);
            this.gbReport.TabIndex = 6;
            this.gbReport.TabStop = false;
            this.gbReport.Text = "Report List";
            // 
            // gridViewReport
            // 
            this.gridViewReport.AllowUserToAddRows = false;
            this.gridViewReport.AllowUserToDeleteRows = false;
            this.gridViewReport.AllowUserToOrderColumns = true;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gridViewReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.gridViewReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridViewReport.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridViewReport.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gridViewReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridViewReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViewReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gridViewReport.ColumnHeadersHeight = 30;
            this.gridViewReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colSender,
            this.colDate,
            this.colTime,
            this.colMessage});
            this.gridViewReport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridViewReport.GridColor = System.Drawing.Color.OrangeRed;
            this.gridViewReport.Location = new System.Drawing.Point(3, 16);
            this.gridViewReport.Name = "gridViewReport";
            this.gridViewReport.ReadOnly = true;
            this.gridViewReport.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridViewReport.RowHeadersVisible = false;
            this.gridViewReport.RowHeadersWidth = 40;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridViewReport.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.gridViewReport.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridViewReport.RowTemplate.Height = 30;
            this.gridViewReport.RowTemplate.ReadOnly = true;
            this.gridViewReport.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViewReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViewReport.ShowCellErrors = false;
            this.gridViewReport.ShowEditingIcon = false;
            this.gridViewReport.ShowRowErrors = false;
            this.gridViewReport.Size = new System.Drawing.Size(754, 300);
            this.gridViewReport.TabIndex = 0;
            this.gridViewReport.TabStop = false;
            // 
            // colNumber
            // 
            this.colNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colNumber.DefaultCellStyle = dataGridViewCellStyle11;
            this.colNumber.Frozen = true;
            this.colNumber.HeaderText = "No.";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.Width = 35;
            // 
            // colSender
            // 
            this.colSender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSender.DefaultCellStyle = dataGridViewCellStyle12;
            this.colSender.HeaderText = "Sender";
            this.colSender.Name = "colSender";
            this.colSender.ReadOnly = true;
            this.colSender.Width = 240;
            // 
            // colDate
            // 
            this.colDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDate.DefaultCellStyle = dataGridViewCellStyle13;
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colDate.Width = 130;
            // 
            // colTime
            // 
            this.colTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colTime.DefaultCellStyle = dataGridViewCellStyle14;
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 90;
            // 
            // colMessage
            // 
            this.colMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colMessage.DefaultCellStyle = dataGridViewCellStyle15;
            this.colMessage.HeaderText = "Report Message";
            this.colMessage.Name = "colMessage";
            this.colMessage.ReadOnly = true;
            // 
            // prbMaster
            // 
            this.prbMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prbMaster.Location = new System.Drawing.Point(68, 626);
            this.prbMaster.Name = "prbMaster";
            this.prbMaster.Size = new System.Drawing.Size(585, 23);
            this.prbMaster.TabIndex = 8;
            // 
            // prbSlave
            // 
            this.prbSlave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prbSlave.Location = new System.Drawing.Point(68, 599);
            this.prbSlave.Name = "prbSlave";
            this.prbSlave.Size = new System.Drawing.Size(585, 23);
            this.prbSlave.TabIndex = 8;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnChangePassword.Location = new System.Drawing.Point(659, 599);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(57, 50);
            this.btnChangePassword.TabIndex = 2;
            this.btnChangePassword.Text = "Change Pass";
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // fileExtensionUserControl
            // 
            this.fileExtensionUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileExtensionUserControl.Location = new System.Drawing.Point(12, 1);
            this.fileExtensionUserControl.Name = "fileExtensionUserControl";
            this.fileExtensionUserControl.Size = new System.Drawing.Size(445, 270);
            this.fileExtensionUserControl.TabIndex = 9;
            // 
            // grbAppearance
            // 
            this.grbAppearance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbAppearance.Controls.Add(this.tableLayoutPanel);
            this.grbAppearance.Location = new System.Drawing.Point(463, 4);
            this.grbAppearance.Name = "grbAppearance";
            this.grbAppearance.Size = new System.Drawing.Size(309, 264);
            this.grbAppearance.TabIndex = 10;
            this.grbAppearance.TabStop = false;
            this.grbAppearance.Text = "Appearance";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel.Controls.Add(this.txtVolumeLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pbSearchState, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.pbMassStorageState, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.label2, 3, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(303, 245);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // txtVolumeLabel
            // 
            this.txtVolumeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel.SetColumnSpan(this.txtVolumeLabel, 5);
            this.txtVolumeLabel.DefaultValue = "Default Value";
            this.txtVolumeLabel.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtVolumeLabel.EnterToTab = false;
            this.txtVolumeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtVolumeLabel.ForeColor = System.Drawing.Color.Gray;
            this.txtVolumeLabel.Location = new System.Drawing.Point(3, 3);
            this.txtVolumeLabel.Name = "txtVolumeLabel";
            this.txtVolumeLabel.Size = new System.Drawing.Size(297, 21);
            this.txtVolumeLabel.TabIndex = 0;
            this.txtVolumeLabel.Text = "Default Value";
            this.txtVolumeLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVolumeLabel.TextForeColor = System.Drawing.Color.Black;
            // 
            // pbSearchState
            // 
            this.pbSearchState.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbSearchState.CurrentStatus = PictureBoxStatus.ImageState.NotLoad;
            this.pbSearchState.ImageLoaded = ((System.Drawing.Bitmap)(resources.GetObject("pbSearchState.ImageLoaded")));
            this.pbSearchState.ImageLoading = ((System.Drawing.Bitmap)(resources.GetObject("pbSearchState.ImageLoading")));
            this.pbSearchState.ImageNotLoad = ((System.Drawing.Bitmap)(resources.GetObject("pbSearchState.ImageNotLoad")));
            this.pbSearchState.LoadingAnimateSpeed = 1000;
            this.pbSearchState.LoadingImageAnimated = false;
            this.pbSearchState.Location = new System.Drawing.Point(104, 33);
            this.pbSearchState.Name = "pbSearchState";
            this.pbSearchState.Size = new System.Drawing.Size(37, 34);
            this.pbSearchState.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search State:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbMassStorageState
            // 
            this.pbMassStorageState.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbMassStorageState.CurrentStatus = PictureBoxStatus.ImageState.NotLoad;
            this.pbMassStorageState.ImageLoaded = ((System.Drawing.Bitmap)(resources.GetObject("pbMassStorageState.ImageLoaded")));
            this.pbMassStorageState.ImageLoading = ((System.Drawing.Bitmap)(resources.GetObject("pbMassStorageState.ImageLoading")));
            this.pbMassStorageState.ImageNotLoad = ((System.Drawing.Bitmap)(resources.GetObject("pbMassStorageState.ImageNotLoad")));
            this.pbMassStorageState.LoadingAnimateSpeed = 1000;
            this.pbMassStorageState.LoadingImageAnimated = false;
            this.pbMassStorageState.Location = new System.Drawing.Point(262, 33);
            this.pbMassStorageState.Name = "pbMassStorageState";
            this.pbMassStorageState.Size = new System.Drawing.Size(37, 34);
            this.pbMassStorageState.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(161, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 38);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mass Storage:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(784, 661);
            this.Controls.Add(this.grbAppearance);
            this.Controls.Add(this.fileExtensionUserControl);
            this.Controls.Add(this.prbSlave);
            this.Controls.Add(this.prbMaster);
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnExit);
            this.DoubleBuffered = true;
            this.MinimizeBox = false;
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Black ANT";
            this.TopMost = true;
            this.gbReport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReport)).EndInit();
            this.grbAppearance.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.ProgressBar prbMaster;
        private System.Windows.Forms.ProgressBar prbSlave;
        private System.Windows.Forms.Button btnChangePassword;
        private FileExtensionUserControl.FileExtensionUserControl fileExtensionUserControl;
        private System.Windows.Forms.DataGridView gridViewReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.GroupBox grbAppearance;
        private DVTextBox.DVTextBox txtVolumeLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private PictureBoxStatus.PicBoxStatus pbSearchState;
        private PictureBoxStatus.PicBoxStatus pbMassStorageState;
        private System.Windows.Forms.Label label2;
    }
}


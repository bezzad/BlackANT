using System;
using System.ComponentModel;
using System.Windows.Forms;
using FileExtension;

namespace FileExtensionUserControl
{
    public class ExtensionButton : System.Windows.Forms.Button, ICloneable
    {
        #region Fields
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public ToolTip ButtonTooltip { get; set; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("File Extension Option for file search options."), Category("Data")] 
        public FileExtensionOption FileExtension
        {
            get { return fileExtension; }
            set
            {
                this.fileExtension = value;
                SetDefaultValues();
            }
        }
        protected FileExtensionOption fileExtension;

        #endregion

        #region Constructors
        public ExtensionButton() : this(new FileExtensionOption()) { }

        public ExtensionButton(IContainer container) : this(new FileExtensionOption()) { container.Add(this); }
 
        public ExtensionButton(FileExtensionOption ExtensionOption)
        {
            InitializeComponent();
            // 
            // Initialize File Extension Option
            // 
            FileExtension = ExtensionOption;

            this.Click += (source, e) =>
                {
                    if (MessageBox.Show(string.Format("Do you want to remove [{0}] Extension from list?", this.FileExtension.ExtensionName),
                        "Warning to the lost data!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes) { this.Dispose(); }
                };
        }
        #endregion

        #region Methods
        protected void SetDefaultValues()
        {
            float FontSize = FileExtension.ExtensionName.Length > 7 ? 8 : 15 - FileExtension.ExtensionName.Length;
            // 
            // Initialize Button
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Engravers MT", FontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Name = "btn" + ((!string.IsNullOrEmpty(FileExtension.ExtensionName)) ? FileExtension.ExtensionName.Substring(1).ToUpper() : "");
            this.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.UseCompatibleTextRendering = true;
            this.UseMnemonic = false;
            this.UseVisualStyleBackColor = false;
            this.Size = new System.Drawing.Size(80, 60);
            this.Image = FileExtension.GetIcon();
            base.Text = FileExtension.ExtensionName.ToUpper();
            // 
            // Initialize ButtonTooltip
            // 
            this.ButtonTooltip = new ToolTip();
            this.ButtonTooltip.AutomaticDelay = 50;
            this.ButtonTooltip.AutoPopDelay = 10000;
            this.ButtonTooltip.InitialDelay = 50;
            this.ButtonTooltip.ReshowDelay = 10;
            this.ButtonTooltip.ShowAlways = true;
            this.ButtonTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            if (!string.IsNullOrEmpty(this.FileExtension.ExtensionName))
            {
                this.ButtonTooltip.SetToolTip(this,
                    string.Format("Click On Extension Button to Remove it!" +
                    "\n\r\n\r {0} \n\r      Minimum Size:  {1} \n\r      Maximum Size:  {2} {3}{4}\n\r ",
                    this.FileExtension.FileTypeToString(),
                    WinExtensions.CalcMemoryMensurableUnit(this.FileExtension.MinSizeLimit),
                    WinExtensions.CalcMemoryMensurableUnit(this.FileExtension.MaxSizeLimit),
                    (this.FileExtension.DateSensitive) ? "\n\r      Date Sensitive:  " : "",
                    (this.FileExtension.DateSensitive) ? this.FileExtension.MaxIntervalFromLastAccessTime.ToString() + " Days" : ""));
            }
        }

        public new string Text
        {
            get { return this.FileExtension.ExtensionName.ToUpper(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    if (value.StartsWith("."))
                    {
                        this.FileExtension.ExtensionName = value.ToLower();
                        SetDefaultValues();
                    }
            }
        }
        #endregion

        #region Designer
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
        #endregion

        #region Implement Interface ICloneable
        public object Clone()
        {
            ExtensionButton obj = new ExtensionButton(this.FileExtension);
            return obj as object;
        }
        #endregion

    }
}


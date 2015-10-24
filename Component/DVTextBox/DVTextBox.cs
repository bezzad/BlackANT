using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;
using System.Globalization;

namespace DVTextBox
{
    [ProvideProperty("DVTextBox", typeof(Control))]
    public class DVTextBox : TextBox
    {
        #region Members
        //[DefaultValue("Default Value")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Default text, when text box is empty this value be superseded."), Category("Appearance")]
        public string DefaultValue
        {
            set
            {
                if (this.Text == this.defaultValue || this.Text == value || this.Text == string.Empty)
                {
                    this.Text = value;
                    this.ForeColor = this.DefaultValueColor;
                }
                this.defaultValue = value;
            }
            get { return defaultValue; }
        }
        private string defaultValue;

        //[DefaultValue(typeof(Color), "Gray")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Default value text's fore color."), Category("Appearance")]
        public Color DefaultValueColor
        {
            set
            {
                if (this.ForeColor == this.defaultValueColor || this.Text == string.Empty || this.Text == this.DefaultValue)
                {
                    this.ForeColor = value;
                }
                defaultValueColor = value;
            }
            get { return this.defaultValueColor; }
        }
        private Color defaultValueColor;

        //[DefaultValue(typeof(Color), "Black")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Main text's fore color."), Category("Appearance")]
        public Color TextForeColor { get; set; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Is Numerical TextBox? if value is true then just typed numbers, and if false then typed any chars."), Category("Behavior")]
        [DefaultValue(false)]
        public bool IsNumerical
        {
            get { return isNumerical; }
            set
            {
                isNumerical = value;
                if (!value)
                {
                    this.ThousandsSplitter = false;
                    this.AcceptMathChars = false;
                }
            }
        }
        private bool isNumerical;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Show Thousands Splitter in TextBox? if value is true then split any 3 numerical digits by char ',' .\n\rNote: IsNumerical must be 'true' for runned this behavior."), Category("Behavior")]
        [DisplayName("Thousands Splitter"), DefaultValue(false)]        
        public bool ThousandsSplitter
        {
            get { return thousandsSplitter; }
            set
            {
                thousandsSplitter = value;
                if (value)
                {
                    IsNumerical = true;
                    AcceptMathChars = false;
                }
            }
        }
        private bool thousandsSplitter;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Convert Enter key press to TAB and focus next controls."), Category("Behavior")]
        [DisplayName("Enter key To Tab")]
        public bool EnterToTab
        {
            get { return enterToTab; }
            set
            {
                enterToTab = value;
                if (value)
                {
                   // AcceptsReturn = false;
                }
            }
        }
        private bool enterToTab;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Accepting mathematical operators such as + , - , * and /"), Category("Behavior")]
        [DefaultValue(false)]
        public bool AcceptMathChars
        {
            get { return acceptMathChars; }
            set
            {
                acceptMathChars = value;
                if (value)
                {
                    IsNumerical = true;
                    ThousandsSplitter = false;
                }
            }
        }
        private bool acceptMathChars;

        public string MathParserResult { get { return mathParserResult; } }
        private string mathParserResult;

        private MathParser mathParser = new MathParser();

        #endregion

        #region Constructors
        public DVTextBox()
        {
            InitializeComponent();

            this.TextForeColor = Color.Black;
            this.DefaultValueColor = Color.Gray;
            this.DefaultValue = "Default Value";
            this.Text = this.DefaultValue;
        }

        public DVTextBox(IContainer container)
            : this() { container.Add(this); }

        #endregion

        #region PreInitialized Events

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //
            if (this.Text == this.DefaultValue)
            {
                // Clean default text
                this.Text = string.Empty;
                this.ForeColor = this.TextForeColor;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            //
            if (this.Text == string.Empty)
            {
                // Set to default text
                this.ForeColor = this.DefaultValueColor;
                this.Text = this.DefaultValue;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            //
            if (this.Text == string.Empty)
            {
                // Set to default text
                this.ForeColor = this.DefaultValueColor;
                this.Text = this.DefaultValue;
            }

            if (ThousandsSplitter && !AcceptMathChars)
            {
                int indexSelectionBuffer = this.SelectionStart;
                if (!string.IsNullOrEmpty(this.Text) && e.KeyData != Keys.Left && e.KeyData != Keys.Right)
                {
                    BigInteger valueBefore;
                    // Parse currency value using en-GB culture. 
                    // value = "�1,097.63";
                    // Displays:  
                    //       Converted '�1,097.63' to 1097.63
                    var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                    var culture = CultureInfo.CreateSpecificCulture("en-US");
                    if (BigInteger.TryParse(this.Text, style, culture, out valueBefore))
                    {
                        this.Text = String.Format(culture, "{0:N0}", valueBefore);
                        if (e.KeyData != Keys.Delete && e.KeyData != Keys.Back) this.Select(this.Text.Length, 0);
                        else this.Select(indexSelectionBuffer, 0);
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //
            if (e.KeyCode == Keys.Enter && EnterToTab) SendKeys.Send("{TAB}");
            else if (this.Text == this.DefaultValue)
            {
                // Clean default text
                this.Text = string.Empty;
                this.ForeColor = this.TextForeColor;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            //
            if (!char.IsDigit(e.KeyChar) && this.IsNumerical)
            {
                int charValue = e.KeyChar;
                const int BackKey_CharValue = 8; // 8 or '\b'
                const int DeleteKey_CharValue = 13; // 13 or '\d'

                if (charValue == BackKey_CharValue || charValue == DeleteKey_CharValue)
                {
                    e.Handled = false;
                    return;
                }
                else if (AcceptMathChars && !ThousandsSplitter)
                {
                    if (e.KeyChar == '+' || e.KeyChar == '-' ||
                        e.KeyChar == '*' || e.KeyChar == '/' ||
                        e.KeyChar == '(' || e.KeyChar == ')')
                    {
                        e.Handled = false;
                        return;
                    }
                }

                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            //
            if (IsNumerical && AcceptMathChars && !string.IsNullOrEmpty(this.Text))
            {
                try
                {
                    mathParserResult = mathParser.Calculate(this.Text).ToString();
                }
                catch { mathParserResult = ""; }
            }
            else mathParserResult = "";
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
    }
}

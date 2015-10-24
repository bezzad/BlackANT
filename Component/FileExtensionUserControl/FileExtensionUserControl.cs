using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FileExtension;

namespace FileExtensionUserControl
{
    public class FileExtensionUserControl : UserControl, IList<ExtensionButton>, IEnumerable<ExtensionButton>, ICollection<ExtensionButton>, IEnumerable, ICloneable
    {
        #region Fields
        private bool OnPainted = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // just Run time
        protected IList<ExtensionButton> CollectionExtensionButtons { get; set; }

        // Summary:
        //     Occurs when a new control is added to the FileExtensionUserControl.CollectionExtensionButton.
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event ControlEventHandler CollectionItemAdded = delegate { };

        // Summary:
        //     Occurs when a ExtensionButton is removed from the FileExtensionUserControl.CollectionExtensionButton.
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event ControlEventHandler CollectionItemRemoved = delegate { };

        #endregion

        public FileExtensionUserControl()
        {
            InitializeComponent();

            if (this.CollectionExtensionButtons == null)
                this.CollectionExtensionButtons = new List<ExtensionButton>();

            this.flowLayoutPanel.ControlRemoved += (source, e) =>
            {
                if (OnPainted)
                {
                    this.Remove((ExtensionButton)e.Control);
                    this.CollectionItemRemoved(source, e);
                }
            };

            this.flowLayoutPanel.ControlAdded += (source, e) => { if (OnPainted) { this.CollectionItemAdded(source, e); } };

        }


        #region Methods
        public override string ToString()
        {
            return String.Join(Environment.NewLine, this.CollectionExtensionButtons.Select(x => x.FileExtension.ToString()));
        }

        /// <summary>
        /// Converts the string representation of a extension to its FileExtensionOption
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A string containing a file extension to convert.</param>
        /// <returns> 
        /// When this method returns, contains the FileExtension value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </returns>
        public static List<FileExtensionOption> Parse(string fromString)
        {
            /// http://regexpal.com/

            List<FileExtensionOption> Extensions = new List<FileExtensionOption>();

            using (StringReader reader = new StringReader(fromString))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        //-------------------
                        try
                        {
                            // Example FileExtensionUserControl.ToString() list's:
                            //
                            // .mp4  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(12)
                            // .3gp  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(90)
                            // .pdf  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                            // .cs  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                            // .htm  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)
                            // .html  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                            // .avi  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                            //                
                            MatchCollection matches =
                                Regex.Matches(line, @"[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max", RegexOptions.Singleline);

                            if (matches.Count < 5 || matches.Count > 7)
                                throw new Exception("This string is not a File Extension!");
                            else
                            {
                                /// For example:  ".mp4  -  Minimum Size[0] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)"
                                /// Pattern:      "[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max"
                                ///
                                /// then matches collection is:
                                /// 
                                ///             matches[0].Value = ".mp4"
                                ///             matches[1].Value = "Minimum Size"
                                ///             matches[2].Value = long Number = "0"
                                ///             matches[3].Value = "Maximum Size"
                                ///             matches[4].Value = long Number = "244554"
                                ///             matches[5].Value? = "Date-Sensitive Max"
                                ///             matches[6].Value? = int number
                                ///             
                                string extensionName = matches[0].Value;
                                long MinSize = long.Parse(matches[2].Value);
                                long MaxSize = long.Parse(matches[4].Value);
                                bool dateSensitive = matches.Count > 5 ? true : false;
                                int maxInterval = dateSensitive ? int.Parse(matches[6].Value) : int.MaxValue;
                                FileType extension = new FileType(extensionName);
                                Extensions.Add(new FileExtensionOption(extension, MinSize, MaxSize, dateSensitive, maxInterval));
                            }
                        }
                        catch (Exception ex) { throw ex; }
                        //-------------------
                    }
                } while (line != null);
            }


            return Extensions;
        }

        /// <summary>
        /// Converts the string representation of a extension to its FileExtensionOption
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">A string containing a file extension to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the FileExtension value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string fromString, out List<FileExtensionOption> result)
        {
            /// http://regexpal.com/


            bool Succes = true;
            result = new List<FileExtensionOption>();

            if (string.IsNullOrEmpty(fromString))
            { Succes = false; goto ExceptionHandeled; }

            try
            {
                using (StringReader reader = new StringReader(fromString))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            //-------------------
                            try
                            {
                                // Example FileExtensionUserControl.ToString() list's:
                                //
                                // .mp4  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(12)
                                // .3gp  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(90)
                                // .pdf  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                                // .cs  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                                // .htm  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)
                                // .html  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                                // .avi  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                                //                
                                MatchCollection matches =
                                    Regex.Matches(line, @"[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max", RegexOptions.Singleline);

                                if (matches.Count < 5 || matches.Count > 7)
                                    throw new Exception("This string is not a File Extension!");
                                else
                                {
                                    /// For example:  ".mp4  -  Minimum Size[0] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)"
                                    /// Pattern:      "[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max"
                                    ///
                                    /// then matches collection is:
                                    /// 
                                    ///             matches[0].Value = ".mp4"
                                    ///             matches[1].Value = "Minimum Size"
                                    ///             matches[2].Value = long Number = "0"
                                    ///             matches[3].Value = "Maximum Size"
                                    ///             matches[4].Value = long Number = "244554"
                                    ///             matches[5].Value? = "Date-Sensitive Max"
                                    ///             matches[6].Value? = int number
                                    ///             
                                    string extensionName = matches[0].Value;
                                    long MinSize = long.Parse(matches[2].Value);
                                    long MaxSize = long.Parse(matches[4].Value);
                                    bool dateSensitive = matches.Count > 5 ? true : false;
                                    int maxInterval = dateSensitive ? int.Parse(matches[6].Value) : int.MaxValue;
                                    FileType extension = new FileType(extensionName);
                                    result.Add(new FileExtensionOption(extension, MinSize, MaxSize, dateSensitive, maxInterval));
                                }
                            }
                            catch (Exception) { Succes = false; }
                            //-------------------
                        }
                    } while (line != null);
                }
            }
            catch { Succes = false; }

        ExceptionHandeled:
            if (!Succes) { result = null; }

            return Succes;
        }

        private void WinExtensions_DatabaseLoaded(object sender, EventArgs e)
        {
            try
            {
                //
                // Load Completed!
                //

                if (!IsHandleCreated)
                {
                    this.CreateControl();
                    return;
                }

                string[] allExtensionNames = WinExtensions.CollectionCommonFileTypes.GetAllExtensionNames();
                //
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (txtExtensionType.InvokeRequired && txtExtensionType.AutoCompleteCustomSource.Count == 0 && txtExtensionType.AutoCompleteCustomSource == null)
                {
                    txtExtensionType.Invoke(new Action(delegate
                    {
                        txtExtensionType.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                        txtExtensionType.AutoCompleteCustomSource.AddRange(allExtensionNames);
                    }));
                }
                else if (txtExtensionType.AutoCompleteCustomSource.Count == 0 && txtExtensionType.AutoCompleteCustomSource == null)
                {
                    txtExtensionType.BeginInvoke(new Action(delegate
                    {
                        txtExtensionType.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                        txtExtensionType.AutoCompleteCustomSource.AddRange(allExtensionNames);
                    }));
                }
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (picBoxStatus.InvokeRequired && picBoxStatus.CurrentStatus != PictureBoxStatus.ImageState.Loaded)
                {
                    picBoxStatus.Invoke(new Action(delegate { picBoxStatus.CurrentStatus = PictureBoxStatus.ImageState.Loaded; }));
                }
                else if (picBoxStatus.CurrentStatus != PictureBoxStatus.ImageState.Loaded)
                {
                    picBoxStatus.BeginInvoke(new Action(delegate { picBoxStatus.CurrentStatus = PictureBoxStatus.ImageState.Loaded; }));
                }

            }
            catch
            { }
            finally
            {
                WinExtensions.DatabaseLoaded -= WinExtensions_DatabaseLoaded;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            //
            //if (System.ComponentModel.LicenseManager.UsageMode == LicenseUsageMode.Runtime) // does not work in compact framework 
            if (IsRuntimeMode)
            {
                if (OnPainted) // Just OnPaint in Runtime for one times...
                    return;

                OnPainted = true;
                #region Run time logic

                if (CollectionExtensionButtons == null)
                    CollectionExtensionButtons = new List<ExtensionButton>();
                //
                // Define default Events
                //            
                this.nudDateSensitive.KeyDown += (source, ea) => { if (ea.KeyCode == Keys.Enter) SendKeys.Send("{TAB}"); };
                int numudLenght = nudDateSensitive.Value.ToString().Length;
                this.nudDateSensitive.GotFocus += (source, ea) => { this.nudDateSensitive.Select(0, (numudLenght > 3) ? ++numudLenght : numudLenght); };
                this.dateTimePicker.KeyDown += (source, ea) => { if (ea.KeyCode == Keys.Enter) SendKeys.Send("{TAB}"); };

                txtMaxSize.KeyUp += txtMaxSize_KeyUp;

                txtMinSize.KeyUp += txtMinSize_KeyUp;

                txtExtensionType.TextChanged += txtExtensionType_TextChanged;
                dateTimePicker.Value = new DateTime(1989, 3, 9); // My birthday
                dateTimePicker_ValueChanged(dateTimePicker, new EventArgs());
                //
                WinExtensions.DatabaseLoaded += WinExtensions_DatabaseLoaded;
                //
                // Load Database... (File Extensions Database)
                //
                picBoxStatus.CurrentStatus = PictureBoxStatus.ImageState.Loading;
                WinExtensions.LoadDatabase();
                //
                // Set ToolTip for Labels...
                //
                string guide = ". 1 Bit = Binary Digit \n\r" +
                               "· 8 Bits = 1 Byte \n\r" +
                               "· 1024 Bytes = 1 Kilobyte \n\r" +
                               "· 1024 Kilobytes = 1 Megabyte \n\r" +
                               "· 1024 Megabytes = 1 Gigabyte \n\r" +
                               "· 1024 Gigabytes = 1 Terabyte \n\r" +
                               "· 1024 Terabytes = 1 Petabyte \n\r" +
                               "· 1024 Petabytes = 1 Exabyte \n\r" +
                               "· 1024 Exabytes = 1 Zettabyte \n\r" +
                               "· 1024 Zettabytes = 1 Yottabyte \n\r" +
                               "· 1024 Yottabytes = 1 Brontobyte \n\r" +
                               "· 1024 Brontobytes = 1 Geopbyte \n\r" +
                               ". . .";

                toolTip.SetToolTip(lblMaxSize, guide);
                toolTip.SetToolTip(lblMinSize, guide);

                #endregion
            }
            else
            {
                //
                // Below code is not run in run time, so that's just for design time
                #region Design time logic

                CollectionExtensionButtons = new List<ExtensionButton>();

                #endregion
            }
        }

        private void txtMinSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMinSize.Text) && e.KeyData != Keys.Left && e.KeyData != Keys.Right)
            {
                lblMinSize.Text = WinExtensions.CalcMemoryMensurableUnit(txtMinSize.MathParserResult);
            }
        }

        private void txtMaxSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaxSize.Text) && e.KeyData != Keys.Left && e.KeyData != Keys.Right)
            {
                lblMaxSize.Text = WinExtensions.CalcMemoryMensurableUnit(txtMaxSize.MathParserResult);
            }
        }

        private void txtExtensionType_TextChanged(object sender, EventArgs e)
        {
            if (txtExtensionType.Text != string.Empty && txtExtensionType.Text != txtExtensionType.DefaultValue)
            {
                if (!txtExtensionType.Text.StartsWith(".") && txtExtensionType.Text.Length > 0)
                {
                    txtExtensionType.Text = txtExtensionType.Text.Insert(0, ".");
                    txtExtensionType.Select(txtExtensionType.Text.Length - 0, 0);
                }

                // if text is a real extensions then...
                if (txtExtensionType.Text.StartsWith(".") && txtExtensionType.Text.Length > 1)
                {
                    lblExtensionDescription.Visible = true;
                    lblExtensionPopularity.Visible = true;
                    FileType fileType;

                    if (WinExtensions.CollectionCommonFileTypes == null) return;

                    if (WinExtensions.CollectionCommonFileTypes.TryGetFileTypeFromExtensionName(txtExtensionType.Text, out fileType))
                    {
                        lblExtensionDescription.Text = fileType.Description;
                        lblExtensionPopularity.Text = fileType.PopularityToString();
                    }
                    else
                    {
                        lblExtensionPopularity.Text = "";
                        lblExtensionDescription.Text = "";
                    }

                    CommonFileType commonFileType;
                    if (WinExtensions.CollectionCommonFileTypes.TryGetCommonFileTypeFromExtensionName(txtExtensionType.Text, out commonFileType))
                    {
                        this.toolTip.SetToolTip(this.lblExtensionDescription, commonFileType.FileFormats + "\n\r" + commonFileType.Description);
                        this.toolTip.SetToolTip(this.lblExtensionPopularity, commonFileType.FileFormats + "\n\r" + commonFileType.Description);
                    }
                }
                else
                {
                    lblExtensionPopularity.Visible = false; lblExtensionPopularity.Text = "";
                    lblExtensionDescription.Visible = false; lblExtensionDescription.Text = "";
                }
            }
            else
            {
                lblExtensionPopularity.Visible = false; lblExtensionPopularity.Text = "";
                lblExtensionDescription.Visible = false; lblExtensionDescription.Text = "";
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            nudDateSensitive.Enabled = dateTimePicker.Checked;
            var subDate = DateTime.Now.Subtract(dateTimePicker.Value);

            if (subDate.Days > nudDateSensitive.Maximum)
            {
                subDate = new TimeSpan((int)nudDateSensitive.Maximum, 0, 0, 0);
                nudDateSensitive.Value = subDate.Days;
                dateTimePicker.Value = DateTime.Now.Subtract(subDate);
            }
            else if (subDate.Days < nudDateSensitive.Minimum)
            {
                subDate = new TimeSpan((int)nudDateSensitive.Minimum, 0, 0, 0);
                nudDateSensitive.Value = subDate.Days;
                dateTimePicker.Value = DateTime.Now.Subtract(subDate);
            }
            else nudDateSensitive.Value = subDate.Days;
        }

        private void nudDateSensitive_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan subDate = new TimeSpan((int)nudDateSensitive.Value, 0, 0, 0);
            dateTimePicker.Value = DateTime.Now.Subtract(subDate);
        }

        private void extensionButton_Click(object sender, EventArgs e)
        {
            ExtensionButton clickedExt = sender as ExtensionButton;

            if (MessageBox.Show(string.Format("Do you want to remove [{0}] Extension from list?", clickedExt.FileExtension.ExtensionName),
                "Warning to the lost data!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.Remove(sender as ExtensionButton);
            }
        }

        private void btnAddE_Click(object sender, EventArgs e)
        {
            long maxSize = long.MaxValue;
            long minSize = 0;
            FileType fileType;
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            //
            // Extension TextBox validator...
            if (string.IsNullOrEmpty(txtExtensionType.Text) || txtExtensionType.Text == txtExtensionType.DefaultValue)
            {
                txtExtensionType.Focus(); txtExtensionType.SelectAll();
                return;
            }
            //
            // Max Size TextBox Validator...
            if (!string.IsNullOrEmpty(txtMaxSize.MathParserResult) && txtMaxSize.Text != txtMaxSize.DefaultValue)
                if (!long.TryParse(txtMaxSize.MathParserResult, style, culture, out maxSize))
                    maxSize = long.MaxValue;
            //
            // Min Size TextBox Validator...
            if (!string.IsNullOrEmpty(txtMinSize.MathParserResult) && txtMinSize.Text != txtMinSize.DefaultValue)
                if (!long.TryParse(txtMinSize.MathParserResult, style, culture, out minSize))
                    minSize = 0;
            //
            // Set File Type from Common File Type Collection
            if (!WinExtensions.CollectionCommonFileTypes.TryGetFileTypeFromExtensionName(txtExtensionType.Text, out fileType))
                fileType = new FileType(txtExtensionType.Text); // If is not common file type
            //
            // Create File Extension Option object
            FileExtensionOption fileExtensionOption = new FileExtensionOption(fileType, minSize, maxSize, dateTimePicker.Checked, (int)nudDateSensitive.Value);
            //
            // Check duplicate extensions from list...
            if (CollectionExtensionButtons.Where(x => x.FileExtension == fileExtensionOption).Count() > 0) // if Count>0 then have duplicate ...
            {
                MessageBox.Show(string.Format("File Type [{0}] is already exist in list!", fileExtensionOption.ExtensionName),
                    "Duplicate Extension Added!", MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
            else // No duplicate extension in list, so add this extension to list...
            {
                //
                // ---------- Create Extension Button ------------
                ExtensionButton extensionButton = new ExtensionButton(fileExtensionOption);
                //
                // Add to list and to UI panel
                this.Add(extensionButton);
            }
            //
            // ReSelecet Extension TextBox for new ...
            this.Focus();
            txtExtensionType.Focus(); txtExtensionType.SelectAll();
        }


        #endregion

        #region Implement Interface IList<ExtenisonButton>

        // Summary:
        //     Inserts an element into the System.Collections.Generic.List<ExtensionButton> at the specified
        //     index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert. The value can be null for reference types.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is greater than System.Collections.Generic.List<ExtensionButton>.Count.
        public void Insert(int index, ExtensionButton item)
        {
            CollectionExtensionButtons.Insert(index, item);
            this.flowLayoutPanel.Controls.Add(item);
            this.flowLayoutPanel.Controls.SetChildIndex(item, index);
        }

        // Summary:
        //     Inserts the elements of a collection into the System.Collections.Generic.List<ExtensionButton>
        //     at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which the new elements should be inserted.
        //
        //   collection:
        //     The collection whose elements should be inserted into the System.Collections.Generic.List<ExtensionButton>.
        //     The collection itself cannot be null, but it can contain elements that are
        //     null, if type T is a reference type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is greater than System.Collections.Generic.List<ExtensionButton>.Count.
        public void InsertRange(int index, IEnumerable<ExtensionButton> collection)
        {
            foreach (ExtensionButton item in collection.Reverse())
                this.Insert(index, item);
        }

        // Summary:
        //     Removes all the elements that match the conditions defined by the specified
        //     predicate.
        //
        // Parameters:
        //   match:
        //     The System.Predicate<ExtensionButton> delegate that defines the conditions of the elements
        //     to remove.
        //
        // Returns:
        //     The number of elements removed from the System.Collections.Generic.List<ExtensionButton>
        //     .
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     match is null.
        public int RemoveAll(Predicate<ExtensionButton> match)
        {
            int counter = 0;

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            this.Where(entity => match(entity))
                      .ToList().ForEach(entity => { this.Remove(entity); counter++; });

            return counter;
        }

        // Summary:
        //     Removes the element at the specified index of the System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to remove.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is equal to or greater than System.Collections.Generic.List<ExtensionButton>.Count.
        public void RemoveAt(int index)
        {
            CollectionExtensionButtons.RemoveAt(index);
            this.flowLayoutPanel.Controls.RemoveAt(index);
        }

        bool ICollection<ExtensionButton>.Remove(ExtensionButton item)
        {
            this.flowLayoutPanel.Controls.Remove(item);
            return CollectionExtensionButtons.Remove(item);
        }

        // Summary:
        //     Removes the first occurrence of a specific object from the System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   item:
        //     The object to remove from the System.Collections.Generic.List<ExtensionButton>. The value
        //     can be null for reference types.
        //
        // Returns:
        //     true if item is successfully removed; otherwise, false. This method also
        //     returns false if item was not found in the System.Collections.Generic.List<ExtensionButton>.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Remove(ExtensionButton item)
        {
            this.flowLayoutPanel.Controls.Remove(item);
            return CollectionExtensionButtons.Remove(item);
        }

        // Summary:
        //     Removes a range of elements from the System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   index:
        //     The zero-based starting index of the range of elements to remove.
        //
        //   count:
        //     The number of elements to remove.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-count is less than 0.
        //
        //   System.ArgumentException:
        //     index and count do not denote a valid range of elements in the System.Collections.Generic.List<ExtensionButton>.
        public void RemoveRange(int index, int count)
        {
            List<ExtensionButton> buffer = new List<ExtensionButton>();

            for (int i = index; i < index + count; i++)
                buffer.Add(this[i]);

            foreach (ExtensionButton anyItem in buffer)
                this.Remove(anyItem);
        }

        // Summary:
        //     Gets or sets the element at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to get or set.
        //
        // Returns:
        //     The element at the specified index.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is equal to or greater than System.Collections.Generic.List<ExtensionButton>.Count.
        public ExtensionButton this[int index]
        {
            get
            {
                return CollectionExtensionButtons[index];
            }
            set
            {
                CollectionExtensionButtons[index] = value;
                this.flowLayoutPanel.Controls.RemoveAt(index);
                this.flowLayoutPanel.Controls.Add(value);
                this.flowLayoutPanel.Controls.SetChildIndex(value, index);
            }
        }

        // Summary:
        //     Removes all elements from the System.Collections.Generic.List<ExtensionButton>.
        public void Clear()
        {
            CollectionExtensionButtons.Clear();
            this.flowLayoutPanel.Controls.Clear();
        }

        // Summary:
        //     Determines whether an element is in the System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.List<ExtensionButton>. The value
        //     can be null for reference types.
        //
        // Returns:
        //     true if item is found in the System.Collections.Generic.List<ExtensionButton>; otherwise,
        //     false.
        public bool Contains(ExtensionButton item)
        {
            return CollectionExtensionButtons.Contains(item);
        }

        // Summary:
        //     Copies the entire System.Collections.Generic.List<ExtensionButton> to a compatible one-dimensional
        //     array, starting at the beginning of the target array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements
        //     copied from System.Collections.Generic.List<ExtensionButton>. The System.Array must have
        //     zero-based indexing.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     array is null.
        //
        //   System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.List<ExtensionButton> is
        //     greater than the number of elements that the destination array can contain.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public void CopyTo(ExtensionButton[] array)
        {
            array = new ExtensionButton[this.Count];

            for (int index = 0; index < this.Count; index++)
                CollectionExtensionButtons.CopyTo(array, index);
        }

        // Summary:
        //     Copies the entire System.Collections.Generic.List<ExtensionButton> to a compatible one-dimensional
        //     array, starting at the specified index of the target array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements
        //     copied from System.Collections.Generic.List<ExtensionButton>. The System.Array must have
        //     zero-based indexing.
        //
        //   arrayIndex:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     array is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     arrayIndex is less than 0.
        //
        //   System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.List<ExtensionButton> is
        //     greater than the available space from arrayIndex to the end of the destination
        //     array.
        public void CopyTo(ExtensionButton[] array, int arrayIndex)
        {
            CollectionExtensionButtons.CopyTo(array, arrayIndex);
        }

        // Summary:
        //     Gets the number of elements actually contained in the System.Collections.Generic.List<ExtensionButton>.
        //
        // Returns:
        //     The number of elements actually contained in the System.Collections.Generic.List<ExtensionButton>.
        public int Count
        {
            get { return CollectionExtensionButtons.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        // Summary:
        //     Returns an enumerator that iterates through the System.Collections.Generic.List<ExtensionButton>.
        //
        // Returns:
        //     A System.Collections.Generic.List<ExtensionButton>.Enumerator for the System.Collections.Generic.List<ExtensionButton>.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public IEnumerator<ExtensionButton> GetEnumerator()
        {
            return CollectionExtensionButtons.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CollectionExtensionButtons.GetEnumerator();
        }

        // Summary:
        //     Adds an object to the end of the System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   item:
        //     The object to be added to the end of the System.Collections.Generic.List<ExtensionButton>.
        //     The value can be null for reference types.
        public void Add(ExtensionButton item)
        {
            CollectionExtensionButtons.Add(item);
            this.flowLayoutPanel.Controls.Add(item);
        }

        // Summary:
        //     Adds an object to the end of the FileExtensionOption.
        //
        // Parameters:
        //   item:
        //     The object to be added to the end of the System.Collections.Generic.List<FileExtensionOption>.
        //     The value can be null for reference types.
        public void Add(FileExtensionOption item)
        {
            ExtensionButton eb = new ExtensionButton(item);
            CollectionExtensionButtons.Add(eb);
            this.flowLayoutPanel.Controls.Add(eb);
        }

        // Summary:
        //     Adds the elements of the specified collection to the end of the System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements should be added to the end of the System.Collections.Generic.List<ExtensionButton>.
        //     The collection itself cannot be null, but it can contain elements that are
        //     null, if type T is a reference type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        public void AddRange(IEnumerable<ExtensionButton> collection)
        {
            foreach (ExtensionButton extBtn in collection)
            { this.Add(extBtn); }
        }

        // Summary:
        //     Adds the elements of the specified collection to the end of the System.Collections.Generic.List<FileExtensionOption>.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements should be added to the end of the System.Collections.Generic.List<FileExtensionOption>.
        //     The collection itself cannot be null, but it can contain elements that are
        //     null, if type T is a reference type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        public void AddRange(IEnumerable<FileExtensionOption> collection)
        {
            foreach (FileExtensionOption ext in collection)
            { this.Add(new ExtensionButton(ext)); }
        }

        // Summary:
        //     Creates a shallow copy of a range of elements in the source System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   index:
        //     The zero-based System.Collections.Generic.List<ExtensionButton> index at which the range
        //     starts.
        //
        //   count:
        //     The number of elements in the range.
        //
        // Returns:
        //     A shallow copy of a range of elements in the source System.Collections.Generic.List<ExtensionButton>.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-count is less than 0.
        //
        //   System.ArgumentException:
        //     index and count do not denote a valid range of elements in the System.Collections.Generic.List<ExtensionButton>.
        public List<ExtensionButton> GetRange(int index, int count)
        {
            List<ExtensionButton> lstRange = new List<ExtensionButton>();

            for (int i = index; i < index + count; i++)
                lstRange.Add(this[i]);

            return lstRange;
        }

        // Summary:
        //     Searches for the specified object and returns the zero-based index of the
        //     first occurrence within the entire System.Collections.Generic.List<ExtensionButton>.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.List<ExtensionButton>. The value
        //     can be null for reference types.
        //
        // Returns:
        //     The zero-based index of the first occurrence of item within the entire System.Collections.Generic.List<ExtensionButton>,
        //     if found; otherwise, –1.
        public int IndexOf(ExtensionButton item)
        {
            return CollectionExtensionButtons.IndexOf(item);
        }

        #endregion

        #region Implement Interface ICloneable
        public object Clone()
        {
            return CollectionExtensionButtons.Select(x => x.Clone()).ToList() as object;
        }
        #endregion

        #region Component Assembly Properties
        public static bool IsDesignMode
        {
            get
            {
                try
                {
                    // Ugly hack, but it works in every version
                    if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                        return true;
                }
                catch
                {
                    if (Application.ExecutablePath.EndsWith("devenv.exe", StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                return false;
            }
        }

        public static bool IsRuntimeMode { get { return !IsDesignMode; } }

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileExtensionUserControl));
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSmallerThan = new System.Windows.Forms.Label();
            this.lblMinSize = new System.Windows.Forms.Label();
            this.lblExtensionDescription = new System.Windows.Forms.Label();
            this.lblSmallerThan2 = new System.Windows.Forms.Label();
            this.lblMaxSize = new System.Windows.Forms.Label();
            this.nudDateSensitive = new System.Windows.Forms.NumericUpDown();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.grbPanel = new System.Windows.Forms.GroupBox();
            this.grbControls = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddE = new System.Windows.Forms.Button();
            this.txtMinSize = new DVTextBox.DVTextBox(this.components);
            this.lblExtensionPopularity = new System.Windows.Forms.Label();
            this.txtMaxSize = new DVTextBox.DVTextBox(this.components);
            this.txtExtensionType = new DVTextBox.DVTextBox(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.picBoxStatus = new PictureBoxStatus.PicBoxStatus();
            ((System.ComponentModel.ISupportInitialize)(this.nudDateSensitive)).BeginInit();
            this.grbPanel.SuspendLayout();
            this.grbControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.AutoScrollMargin = new System.Drawing.Size(0, 20000);
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(459, 126);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // lblSmallerThan
            // 
            this.lblSmallerThan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSmallerThan.AutoSize = true;
            this.lblSmallerThan.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmallerThan.Location = new System.Drawing.Point(142, 10);
            this.lblSmallerThan.Name = "lblSmallerThan";
            this.lblSmallerThan.Size = new System.Drawing.Size(31, 31);
            this.lblSmallerThan.TabIndex = 100;
            this.lblSmallerThan.Text = "<";
            this.lblSmallerThan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMinSize
            // 
            this.lblMinSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMinSize.AutoSize = true;
            this.lblMinSize.Location = new System.Drawing.Point(22, 42);
            this.lblMinSize.Name = "lblMinSize";
            this.lblMinSize.Size = new System.Drawing.Size(0, 13);
            this.lblMinSize.TabIndex = 6;
            this.lblMinSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExtensionDescription
            // 
            this.lblExtensionDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExtensionDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblExtensionDescription.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lblExtensionDescription.Location = new System.Drawing.Point(145, 41);
            this.lblExtensionDescription.Name = "lblExtensionDescription";
            this.lblExtensionDescription.Size = new System.Drawing.Size(174, 39);
            this.lblExtensionDescription.TabIndex = 100;
            this.lblExtensionDescription.Text = "Selected Extension";
            this.lblExtensionDescription.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblExtensionDescription.UseMnemonic = false;
            this.lblExtensionDescription.Visible = false;
            // 
            // lblSmallerThan2
            // 
            this.lblSmallerThan2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSmallerThan2.AutoSize = true;
            this.lblSmallerThan2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmallerThan2.Location = new System.Drawing.Point(291, 10);
            this.lblSmallerThan2.Name = "lblSmallerThan2";
            this.lblSmallerThan2.Size = new System.Drawing.Size(31, 31);
            this.lblSmallerThan2.TabIndex = 100;
            this.lblSmallerThan2.Text = "<";
            this.lblSmallerThan2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMaxSize
            // 
            this.lblMaxSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxSize.AutoSize = true;
            this.lblMaxSize.Location = new System.Drawing.Point(345, 42);
            this.lblMaxSize.Name = "lblMaxSize";
            this.lblMaxSize.Size = new System.Drawing.Size(0, 13);
            this.lblMaxSize.TabIndex = 8;
            this.lblMaxSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudDateSensitive
            // 
            this.nudDateSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudDateSensitive.Location = new System.Drawing.Point(100, 103);
            this.nudDateSensitive.Maximum = new decimal(new int[] {
            36500,
            0,
            0,
            0});
            this.nudDateSensitive.Minimum = new decimal(new int[] {
            36500,
            0,
            0,
            -2147483648});
            this.nudDateSensitive.Name = "nudDateSensitive";
            this.nudDateSensitive.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.nudDateSensitive.Size = new System.Drawing.Size(62, 20);
            this.nudDateSensitive.TabIndex = 3;
            this.nudDateSensitive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDateSensitive.ThousandsSeparator = true;
            this.nudDateSensitive.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudDateSensitive.Value = new decimal(new int[] {
            36500,
            0,
            0,
            0});
            this.nudDateSensitive.ValueChanged += new System.EventHandler(this.nudDateSensitive_ValueChanged);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateTimePicker.Location = new System.Drawing.Point(6, 134);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowCheckBox = true;
            this.dateTimePicker.Size = new System.Drawing.Size(198, 20);
            this.dateTimePicker.TabIndex = 5;
            this.dateTimePicker.TabStop = false;
            this.dateTimePicker.Value = new System.DateTime(1989, 3, 9, 0, 0, 0, 0);
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.Location = new System.Drawing.Point(6, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 100;
            this.label1.Text = "Date Sensitive:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // grbPanel
            // 
            this.grbPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbPanel.Controls.Add(this.flowLayoutPanel);
            this.grbPanel.Location = new System.Drawing.Point(3, 3);
            this.grbPanel.Name = "grbPanel";
            this.grbPanel.Size = new System.Drawing.Size(465, 145);
            this.grbPanel.TabIndex = 1;
            this.grbPanel.TabStop = false;
            this.grbPanel.Text = "Selected Extension";
            // 
            // grbControls
            // 
            this.grbControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbControls.Controls.Add(this.picBoxStatus);
            this.grbControls.Controls.Add(this.label2);
            this.grbControls.Controls.Add(this.label1);
            this.grbControls.Controls.Add(this.nudDateSensitive);
            this.grbControls.Controls.Add(this.dateTimePicker);
            this.grbControls.Controls.Add(this.btnAddE);
            this.grbControls.Controls.Add(this.txtMinSize);
            this.grbControls.Controls.Add(this.lblMaxSize);
            this.grbControls.Controls.Add(this.lblExtensionPopularity);
            this.grbControls.Controls.Add(this.lblExtensionDescription);
            this.grbControls.Controls.Add(this.lblMinSize);
            this.grbControls.Controls.Add(this.txtMaxSize);
            this.grbControls.Controls.Add(this.lblSmallerThan2);
            this.grbControls.Controls.Add(this.txtExtensionType);
            this.grbControls.Controls.Add(this.lblSmallerThan);
            this.grbControls.Location = new System.Drawing.Point(3, 151);
            this.grbControls.Name = "grbControls";
            this.grbControls.Size = new System.Drawing.Size(464, 162);
            this.grbControls.TabIndex = 0;
            this.grbControls.TabStop = false;
            this.grbControls.Text = "Add Extension";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Database State:";
            // 
            // btnAddE
            // 
            this.btnAddE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddE.Location = new System.Drawing.Point(339, 96);
            this.btnAddE.Name = "btnAddE";
            this.btnAddE.Size = new System.Drawing.Size(107, 50);
            this.btnAddE.TabIndex = 4;
            this.btnAddE.Text = "&Add Extension";
            this.btnAddE.UseVisualStyleBackColor = true;
            this.btnAddE.Click += new System.EventHandler(this.btnAddE_Click);
            // 
            // txtMinSize
            // 
            this.txtMinSize.AcceptMathChars = true;
            this.txtMinSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMinSize.DefaultValue = "Min Size (Bytes)";
            this.txtMinSize.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtMinSize.EnterToTab = true;
            this.txtMinSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtMinSize.ForeColor = System.Drawing.Color.Gray;
            this.txtMinSize.IsNumerical = true;
            this.txtMinSize.Location = new System.Drawing.Point(6, 18);
            this.txtMinSize.Name = "txtMinSize";
            this.txtMinSize.Size = new System.Drawing.Size(130, 20);
            this.txtMinSize.TabIndex = 1;
            this.txtMinSize.Text = "Min Size (Bytes)";
            this.txtMinSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinSize.TextForeColor = System.Drawing.Color.Empty;
            // 
            // lblExtensionPopularity
            // 
            this.lblExtensionPopularity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExtensionPopularity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblExtensionPopularity.ForeColor = System.Drawing.Color.Gold;
            this.lblExtensionPopularity.Location = new System.Drawing.Point(177, 80);
            this.lblExtensionPopularity.Name = "lblExtensionPopularity";
            this.lblExtensionPopularity.Size = new System.Drawing.Size(110, 23);
            this.lblExtensionPopularity.TabIndex = 100;
            this.lblExtensionPopularity.Text = "Star";
            this.lblExtensionPopularity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblExtensionPopularity.UseMnemonic = false;
            this.lblExtensionPopularity.Visible = false;
            // 
            // txtMaxSize
            // 
            this.txtMaxSize.AcceptMathChars = true;
            this.txtMaxSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxSize.DefaultValue = "Max Size (Bytes)";
            this.txtMaxSize.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtMaxSize.EnterToTab = true;
            this.txtMaxSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtMaxSize.ForeColor = System.Drawing.Color.Gray;
            this.txtMaxSize.IsNumerical = true;
            this.txtMaxSize.Location = new System.Drawing.Point(328, 18);
            this.txtMaxSize.Name = "txtMaxSize";
            this.txtMaxSize.Size = new System.Drawing.Size(130, 20);
            this.txtMaxSize.TabIndex = 2;
            this.txtMaxSize.Text = "Max Size (Bytes)";
            this.txtMaxSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaxSize.TextForeColor = System.Drawing.Color.Empty;
            // 
            // txtExtensionType
            // 
            this.txtExtensionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExtensionType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.txtExtensionType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtExtensionType.DefaultValue = ".Extension Type";
            this.txtExtensionType.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtExtensionType.EnterToTab = true;
            this.txtExtensionType.ForeColor = System.Drawing.Color.Gray;
            this.txtExtensionType.Location = new System.Drawing.Point(179, 18);
            this.txtExtensionType.Name = "txtExtensionType";
            this.txtExtensionType.Size = new System.Drawing.Size(106, 20);
            this.txtExtensionType.TabIndex = 0;
            this.txtExtensionType.Text = ".Extension Type";
            this.txtExtensionType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtExtensionType.TextForeColor = System.Drawing.Color.Empty;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 50;
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 50;
            this.toolTip.ReshowDelay = 10;
            this.toolTip.ShowAlways = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // picBoxStatus
            // 
            this.picBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picBoxStatus.CurrentStatus = PictureBoxStatus.ImageState.NotLoad;
            this.picBoxStatus.ImageLoaded = ((System.Drawing.Bitmap)(resources.GetObject("picBoxStatus.ImageLoaded")));
            this.picBoxStatus.ImageLoading = ((System.Drawing.Bitmap)(resources.GetObject("picBoxStatus.ImageLoading")));
            this.picBoxStatus.ImageNotLoad = ((System.Drawing.Bitmap)(resources.GetObject("picBoxStatus.ImageNotLoad")));
            this.picBoxStatus.LoadingAnimateSpeed = 1000;
            this.picBoxStatus.LoadingImageAnimated = false;
            this.picBoxStatus.Location = new System.Drawing.Point(100, 57);
            this.picBoxStatus.Name = "picBoxStatus";
            this.picBoxStatus.Size = new System.Drawing.Size(40, 40);
            this.picBoxStatus.TabIndex = 101;
            // 
            // FileExtensionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grbControls);
            this.Controls.Add(this.grbPanel);
            this.Name = "FileExtensionUserControl";
            this.Size = new System.Drawing.Size(470, 316);
            ((System.ComponentModel.ISupportInitialize)(this.nudDateSensitive)).EndInit();
            this.grbPanel.ResumeLayout(false);
            this.grbPanel.PerformLayout();
            this.grbControls.ResumeLayout(false);
            this.grbControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DVTextBox.DVTextBox txtMinSize;
        private DVTextBox.DVTextBox txtExtensionType;
        private DVTextBox.DVTextBox txtMaxSize;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Label lblSmallerThan;
        private System.Windows.Forms.Label lblMinSize;
        private System.Windows.Forms.Label lblExtensionDescription;
        private System.Windows.Forms.Label lblSmallerThan2;
        private System.Windows.Forms.GroupBox grbPanel;
        private System.Windows.Forms.GroupBox grbControls;
        private System.Windows.Forms.Label lblMaxSize;
        private System.Windows.Forms.Button btnAddE;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.NumericUpDown nudDateSensitive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblExtensionPopularity;
        private ToolTip toolTip;
        private Label label2;
        private PictureBoxStatus.PicBoxStatus picBoxStatus;
        #endregion
    }
}
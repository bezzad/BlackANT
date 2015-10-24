using EventArguments;
using FileExtension;
using FileExtensionUserControl;
using FTP;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Black_ANT
{
    /// <summary>
    /// Graphical User Interface
    /// </summary>
    public partial class GUI : Form
    {
        int ReportCounter = 0; // report line counter
        long StartTime; // Tick of GUI start time's
        bool loaded = false; // for calculate Activate time's for Once of On Paint refreshing

        #region Methods

        public GUI(long startTime)
        {
            InitializeComponent();
            this.Icon = global::Black_ANT.Properties.Resources.BlackANT;
            this.StartTime = startTime;
            this.Load += GUI_Load;
            this.txtVolumeLabel.KeyDown += txtVolumeLabel_KeyDown;
            this.FormClosing += (s, e) => Transaction.ProgressChanged -= Transaction_ProgressChanged;
            //
            // Read Extension Buttons from Registry
            //
            List<FileExtensionOption> lstSaveExtensions;
            if (FileExtensionUserControl.FileExtensionUserControl.TryParse((string)AppDataManager.ReadData("ExtensionButtons", true), out lstSaveExtensions))
                if (lstSaveExtensions != null)
                    this.fileExtensionUserControl.AddRange(lstSaveExtensions);
            //
            // Create Events for knows when extension data add or removed from FileExtensionUserControl
            //
            this.fileExtensionUserControl.CollectionItemAdded += fileExtensionUserControl_CollectionItemAdded;
            this.fileExtensionUserControl.CollectionItemRemoved += fileExtensionUserControl_CollectionItemRemoved;
            //
            // Read Any Reports from Kernel
            //
            foreach (var _report in EventReflector.Reports)
            { ManageReport(_report); }

            gridViewReport.Rows.AddRange(EventReflector.Reports.Where(x => x != null).Select(x => CreateRow(x)).ToArray());
            //
            // Connect GUI Reporter gridViewer to ReflectorReports Event
            //
            EventReflector.ReflectedReporter += Reports_ReflectedReports;
            EventReflector.ReflectedReporter += (s, e) => ManageReport(e);
        }

        private void ManageReport(ReportEventArgs e)
        {
            if (e == null) return;

            switch (e.ReportCode)
            {
                case ReportCodes.DiskProbingStarted: SetPicBoxStatusAsync(pbSearchState, PictureBoxStatus.ImageState.Loading);
                    break;
                case ReportCodes.DiskProbingFinished: SetPicBoxStatusAsync(pbSearchState, PictureBoxStatus.ImageState.Loaded);
                    break;
                case ReportCodes.MassStorageInserted: SetPicBoxStatusAsync(pbMassStorageState, PictureBoxStatus.ImageState.Loading);
                    break;
                case ReportCodes.IdentifyMassStorage: SetPicBoxStatusAsync(pbMassStorageState, PictureBoxStatus.ImageState.Loading);
                    break;
                case ReportCodes.IdentityVerificationMassStorage: SetPicBoxStatusAsync(pbMassStorageState, PictureBoxStatus.ImageState.Loaded);
                    break;
                case ReportCodes.MassStorageRejected: SetPicBoxStatusAsync(pbMassStorageState, PictureBoxStatus.ImageState.NotLoad);
                    break;
                case ReportCodes.NotAllowed: SetPicBoxStatusAsync(pbMassStorageState, PictureBoxStatus.ImageState.NotLoad);
                    break;
            }
        }
        private void SetPicBoxStatusAsync(PictureBoxStatus.PicBoxStatus picBox, PictureBoxStatus.ImageState state)
        {
            if (picBox.InvokeRequired) { picBox.Invoke(new Action(delegate { picBox.CurrentStatus = state; })); }
            else { picBox.CurrentStatus = state; }
        }


        private void SetVolumeTextBox()
        {
            string volumeLabel = (string)AppDataManager.ReadData("VolumeLabel", true);
            txtVolumeLabel.Text = string.Empty;
            txtVolumeLabel.DefaultValue = "Mass Storage Volume Label" + (string.IsNullOrEmpty(volumeLabel) ? "" : ":   " + volumeLabel);
        }

        private DataGridViewRow CreateRow(ReportEventArgs report)
        {
            DataGridViewRow row = (DataGridViewRow)gridViewReport.RowTemplate.Clone();

            row.CreateCells(gridViewReport,
                Interlocked.Increment(ref ReportCounter),
                report.Source + (report.ReportCode == ReportCodes.ExceptionHandled ? " (#EXP#)" : ""),
                report.OccurrenceTime_IST.ToLongDateString(),
                report.OccurrenceTime_IST.ToLongTimeString(),
                report.Message);

            if (report.ReportCode == ReportCodes.ExceptionHandled)
                row.DefaultCellStyle = new DataGridViewCellStyle() { BackColor = System.Drawing.Color.Red };

            return row;
        }

        private void txtVolumeLabel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtVolumeLabel.Text) && txtVolumeLabel.Text != txtVolumeLabel.DefaultValue)
                {
                    try
                    {
                        if (AppDataManager.SaveData(txtVolumeLabel.Text, "VolumeLabel"))
                            EventReflector.CallReport.Post(new ReportEventArgs("GUI.txtVolumeLabel_KeyDown",
                                ReportCodes.DataChanged,
                                "Volume Label Changed and Stored"));
                    }
                    catch (Exception ex)
                    {
                        EventReflector.CallReport.Post(new ReportEventArgs("GUI.txtVolumeLabel_KeyDown", ex));
                    }
                    finally
                    {
                        SetVolumeTextBox();
                    }
                }
            }
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            SetVolumeTextBox();
            this.Text += ComponentAssembly.ComponentController.IsAdmin() ? "   (Run As Administrator)" : "   (Run As Current User)";
        }

        private bool CheckControlCreatedBySubControls(Control c)
        {
            if (c.HasChildren)
                foreach (Control v in c.Controls)
                    if (!CheckControlCreatedBySubControls(v)) return false;

            return (c.IsHandleCreated & c.Created) | !c.Visible;
        }

        private void fileExtensionUserControl_CollectionItemRemoved(object sender, ControlEventArgs e)
        {
            EventReflector.CallReport.Post(new ReportEventArgs("GUI.fileExtensionUserControl.CollectionItemRemoved",
                ReportCodes.RemoveExtension,
                "Remove extension [{0}] from list", ((ExtensionButton)e.Control).FileExtension.ExtensionName));

            AppDataManager.SaveData(this.fileExtensionUserControl.ToString(), "ExtensionButtons");
        }

        private void fileExtensionUserControl_CollectionItemAdded(object sender, ControlEventArgs e)
        {
            EventReflector.CallReport.Post(new ReportEventArgs("GUI.fileExtensionUserControl.CollectionItemAdded",
                ReportCodes.AddExtension,
                "Add new extension [{0}] to list", ((ExtensionButton)e.Control).FileExtension.ExtensionName));

            AppDataManager.SaveData(this.fileExtensionUserControl.ToString(), "ExtensionButtons");
        }

        private void Reports_ReflectedReports(object sender, ReportEventArgs e)
        {
            try
            {
                if (gridViewReport.InvokeRequired)
                {
                    gridViewReport.Invoke(new Action(delegate
                    {
                        gridViewReport.Rows.Add(CreateRow(e));
                        gridViewReport.ClearSelection();
                        gridViewReport.Rows[gridViewReport.RowCount - 1].Selected = true;
                    }));
                }
                else if (gridViewReport.IsHandleCreated && gridViewReport.Created)
                {
                    gridViewReport.Rows.Add(CreateRow(e));
                    gridViewReport.ClearSelection();
                    gridViewReport.Rows[gridViewReport.RowCount - 1].Selected = true;
                }
            }
            catch { }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to Exit completely from this Application?", "Exit Requested",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                Application.Exit();

        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox.AboutForm aboutF = new AboutBox.AboutForm(this);
            aboutF.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePassword CHP = new ChangePassword(AppDataManager.GetHashPassword);
            if (CHP.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // save code here...
                AppDataManager.SetHashPassword(CHP.NewSHA512HashPass);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (loaded)
                return;

            if (!CheckControlCreatedBySubControls(this))
                return;

            loaded = true;
            TimeSpan ts = ComponentAssembly.ComponentController.GetDurationRealTime(StartTime);


            this.Text = string.Format("Black ANT                Activate Time: {0}{1}{2}ms",
                ts.Minutes > 0 ? ts.Minutes + "min:" : "",
                ts.Seconds > 0 | ts.Minutes > 0 ? ts.Seconds + "sec." : "",
                ts.Milliseconds);

            Transaction.ProgressChanged += Transaction_ProgressChanged;
        }

        private void Transaction_ProgressChanged(object sender, Ionic.Zip.SaveProgressEventArgs e)
        {
            try
            {
                if (prbMaster == null || prbSlave == null ||
                    prbMaster.IsDisposed || prbSlave.IsDisposed ||
                    !prbMaster.IsHandleCreated || !prbSlave.IsHandleCreated ||
                    !prbMaster.Created || !prbSlave.Created)
                    return;

                if (e.TotalBytesToTransfer > 0)
                {
                    if (this.prbSlave.InvokeRequired)
                    {
                        this.prbSlave.Invoke(new Action(delegate
                        {
                            this.prbSlave.Value = unchecked((int)(e.BytesTransferred * 100 / e.TotalBytesToTransfer));
                        }));
                    }
                }
                if (e.EntriesSaved > 0 && e.EntriesTotal > 0)
                {
                    if (this.prbMaster.InvokeRequired)
                    {
                        this.prbMaster.Invoke(new Action(delegate
                        {
                            this.prbMaster.Value = unchecked((int)((double)e.EntriesSaved * 100 / (double)e.EntriesTotal));
                        }));
                    }
                }
            }
            catch (Exception ex) { EventReflector.CallReport.Post(new ReportEventArgs("GUI.TransferToDisk_ProgressChanged", ex)); }
        }

        #endregion
    }
}

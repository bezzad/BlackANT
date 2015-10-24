using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.IO;
using System.Threading;

namespace DiskDriveListener
{
    /// <summary>
    /// A class for detect Insertion / Removal USB Mass Storage drive.
    /// </summary>
    public class DiskDriveChecker
    {
        #region Properties
        /// <summary>
        /// List of any exist drives.
        /// </summary>
        public static Dictionary<String, DriveInfo> Drivers = new Dictionary<string, DriveInfo>();

        /// <summary>
        /// Two management timer for check any events of Win32
        /// </summary>
        ManagementEventWatcher mwe_deletion, mwe_creation;

        /// <summary>
        /// A Thread for run Listener in parallel of Main Programs.
        /// </summary>
        Thread wmiThread;

        /// <summary>
        /// Target void Function(DriveInfo Parameter's)
        /// </summary>
        public event EventHandler<DiskDriveInsertionEventArgs> OnDiskDriveInserted;

        /// <summary>
        /// Target void Function(void)
        /// </summary>
        public event EventHandler OnDiskDriveRemoval;
        #endregion

        /// <summary>
        /// Constructor of DiskDriveChecker.
        /// </summary>
        public DiskDriveChecker()
        {
            wmiThread = new Thread(new ThreadStart(WMIEvent));
        }

        /// <summary>
        /// Start Listener of Win32 Events.
        /// </summary>
        public void StartEventsListener()
        {
            try
            {
                wmiThread.Start();
            }
            catch { };
        }

        /// <summary>
        /// Stop all Listener;
        /// </summary>
        public void StopEventsListener()
        {
            try
            {
                if (mwe_creation != null)
                    mwe_creation.Stop();

                if (mwe_deletion != null)
                    mwe_deletion.Stop();

                wmiThread.Abort();
            }
            catch { };
        }

        /// <summary>
        /// Declaration and define any listener for Win32 events.
        /// </summary>
        protected void WMIEvent()
        {
            DiskDriveChecker.Drivers.Clear();
            // find and save any exist drives
            foreach (var driver in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                DiskDriveChecker.Drivers.Add(driver.Name, driver);
            }

            //Insert USB codes...
            try
            {
                WqlEventQuery q_creation = new WqlEventQuery();
                q_creation.EventClassName = "__InstanceCreationEvent";
                q_creation.WithinInterval = new TimeSpan(0, 0, 3);    //How often do you want to check it? 2Sec.
                q_creation.Condition = @"TargetInstance ISA 'Win32_DiskDriveToDiskPartition'";
                mwe_creation = new ManagementEventWatcher(q_creation);
                mwe_creation.EventArrived += new EventArrivedEventHandler(this.USBEventArrived_Creation);
                mwe_creation.Start(); // Start listen for events
            }
            catch { }


            //Remove USB codes...
            try
            {
                WqlEventQuery q_deletion = new WqlEventQuery();
                q_deletion.EventClassName = "__InstanceDeletionEvent";
                q_deletion.WithinInterval = new TimeSpan(0, 0, 4);    //How often do you want to check it? 2Sec.
                q_deletion.Condition = @"TargetInstance ISA 'Win32_DiskDriveToDiskPartition'  ";
                mwe_deletion = new ManagementEventWatcher(q_deletion);
                mwe_deletion.EventArrived += new EventArrivedEventHandler(this.USBEventArrived_Deletion);
                mwe_deletion.Start(); // Start listen for events
            }
            catch { }
        }

        /// <summary>
        /// A EventHandel of USB insertion.
        /// </summary>
        /// <param name="sender">WMI object</param>
        /// <param name="e">EventArrivedEventArgs</param>
        protected void USBEventArrived_Creation(object sender, EventArrivedEventArgs e)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (!Drivers.ContainsKey(drive.Name) && drive.IsReady)
                {
                    Drivers.Add(drive.Name, drive);

                    // Execute a given function.
                    if (OnDiskDriveInserted != null)
                        OnDiskDriveInserted(this, new DiskDriveInsertionEventArgs(drive));
                }

            }
        }

        /// <summary>
        /// A EventHandel of USB removal.
        /// </summary>
        /// <param name="sender">WMI object</param>
        /// <param name="e">EventArrivedEventArgs</param>
        protected void USBEventArrived_Deletion(object sender, EventArrivedEventArgs e)
        {
            Drivers.Clear();
            foreach (var driver in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                Drivers.Add(driver.Name, driver);
            }

            // Execute a given function.
            if (OnDiskDriveRemoval != null)
                OnDiskDriveRemoval(this, new EventArgs());
        }

        /// <summary>
        /// Destroys of DiskDriveChecker. (Stop any listener...)
        /// </summary>
        ~DiskDriveChecker()
        {
            if (mwe_creation != null)
                mwe_creation.Stop();

            if (mwe_deletion != null)
                mwe_deletion.Stop();
        }
    }
}

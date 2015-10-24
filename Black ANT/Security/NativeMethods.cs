using System;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace Security
{
    internal class NativeMethods
    {
        public static volatile Mutex mutex = new Mutex(true, "{6DE7854A-9FA7-4368-8336-629A283B501E}");
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
       
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}

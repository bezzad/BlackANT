using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardActivityMonitor.WinApi;
using Security;
using EventArguments;

namespace MouseKeyboardActivityMonitor.HookUserActivity
{
    public sealed class HookToMyPassword : KeyboardHookListener, IDisposable
    {
        public event EventHandler<ReportEventArgs> Reporter;
        public event EventHandler PasswordEntered;

        private bool _Pause = false;
        private string _SH512HashPassword;
        private string _RealTimeEntryChars = string.Empty;
        private bool validCHar = true;


        public void Pause() { this._Pause = true; }
        public void Resume() { this._Pause = false; }

        public HookToMyPassword()
            : base(new GlobalHooker())
        {       
            this.KeyPress += HookToMyPassword_KeyPress;
            this.KeyDown += HookToMyPassword_KeyDown;
        }

        private void HookToMyPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (_Pause) return;

            if(e.KeyCode == Keys.Enter)
            {
                validCHar = false;

                
                
                if(_SH512HashPassword == _RealTimeEntryChars.GetSHA512HashCode())
                {
                    this.Reporter(this, new ReportEventArgs("HookToMyPassword", ReportCodes.PasswordCorrect, "Correct Password Entered"));
                    this.PasswordEntered(this, e);
                }

                _RealTimeEntryChars = string.Empty; 
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

        private void HookToMyPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Pause) return;

            if (validCHar)
                _RealTimeEntryChars += e.KeyChar;
        }

        void IDisposable.Dispose()
        {
            base.Dispose();
            //
            // First Override Memory data then clear that.
            //
            this._RealTimeEntryChars = "FFFFFFFFFFFFFFFFFF";
            this._RealTimeEntryChars = string.Empty;
           
            this._SH512HashPassword = 
                "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"+
                "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"+
                "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"+
                "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";

            this._SH512HashPassword = string.Empty;

            this.validCHar = false;
        }

        ~HookToMyPassword()
        {
            this.Stop();

            this.Dispose();

            GC.Collect();
        }

        public override void Stop()
        {
            Reporter("HookToMyPassword", new ReportEventArgs("HookToMyPassword",
                ReportCodes.KeyListenerStopped,
                "Keyboard Listener Stopped"));

            base.Stop();
        }

        public void Start(string SH512HashPassword)
        {
            _SH512HashPassword = SH512HashPassword;

            base.Start();

            Reporter("HookToMyPassword", new ReportEventArgs("HookToMyPassword",
                ReportCodes.KeyListenerStarted,
                "Keyboard Listener Started"));
        }
    }
}

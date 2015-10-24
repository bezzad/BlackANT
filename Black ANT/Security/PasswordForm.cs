using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Security
{
    partial class PasswordForm : Form
    {
        private string HashPassword;
        private bool MouseButtonPressed = false;
        private Point MouseLocation;

        /// <summary>
        /// Constructor of class.
        /// </summary>
        /// <param name="formText">Text of form Volume Label text's.</param>
        /// <param name="HashCodePass">Password in MD5 Hash coding form.</param>
        public PasswordForm(string formText, string HashCodePass)
        {
            InitializeComponent();

            //
            // PasswordForm Form
            //
            this.DialogResult = DialogResult.Cancel;
            this.HashPassword = HashCodePass;
            this.txtVolumeLabel.Text = formText;
            this.Location = new Point(0, 0);
            this.DialogResult = DialogResult.Cancel;
            //
            // Password TextBox
            //
            this.txtPassword.KeyDown += txtPassword_KeyDown;
            this.txtPassword.KeyUp += txtPassword_KeyUp;
            this.txtPassword.MouseDown += txtPassword_MouseDown;
            this.txtPassword.MouseLeave += txtPassword_MouseLeave;
            this.txtPassword.Focus();
            this.txtPassword.Select(0, 0);
            //
            // Volume Label TextBox
            //
            this.txtVolumeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtVolumeLabel.ForeColor = Color.DimGray;
            //
            // btnMove Button
            // 
            this.btnMove.MouseDown += btnMove_MouseDown;
            this.btnMove.MouseUp += btnMove_MouseUp;
            this.btnMove.MouseMove += btnMove_MouseMove;
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnMove_MouseDown(object sender, MouseEventArgs e)
        {
            MouseLocation = e.Location;
            MouseButtonPressed = true;
        }
        private void btnMove_MouseUp(object sender, MouseEventArgs e)
        {
            MouseButtonPressed = false;
        }
        private void btnMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtonPressed)
            {
                this.Location = new Point(this.Location.X + (e.X - MouseLocation.X), this.Location.Y + (e.Y - MouseLocation.Y));
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtPassword.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Your Password", "Empty TextBox Error");
                    return;
                }
                else
                {
                    CheckUserPass(txtPassword.Text);
                }
            }

        }
        private void txtPassword_MouseLeave(object sender, EventArgs e)
        {
            if (txtPassword.Text == string.Empty) { txtPassword.UseSystemPasswordChar = false; }
        }
        private void txtPassword_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtPassword.Text == txtPassword.DefaultValue)
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtPassword.Text == string.Empty) { txtPassword.UseSystemPasswordChar = false; }
            else if (txtPassword.Text != txtPassword.DefaultValue && txtPassword.Text != string.Empty && txtPassword.UseSystemPasswordChar == false)
            {
                txtPassword.UseSystemPasswordChar = true;
                txtPassword.Select(txtPassword.Text.Length, 0);
            }
        }
        

        private void CheckUserPass(string pass)
        {
            if (HashPassword != pass.GetSHA512HashCode())
            {
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
                if (txtPassword.Text == string.Empty) { txtPassword.UseSystemPasswordChar = false; }
                MessageBox.Show("Your Password is incorrect", "Password Error");
                txtPassword.Select(0, 0);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

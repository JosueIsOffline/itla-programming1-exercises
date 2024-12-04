using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MembershipSystem.Controllers;


namespace MembershipSystem.Forms
{
    public partial class frmLogin : Form
    {
       
        public frmLogin()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wnsg, int wparam, int lparam);

        private async void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(txtUser.Text))
                {
                    MessageBox.Show("Please enter username", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUser.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtPass.Text))
                {
                    MessageBox.Show("Please enter password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPass.Focus();
                    return;
                }

                Cursor = Cursors.WaitCursor;

                var user = await Auth.Instance.GetUserAsync(txtUser.Text, txtPass.Text);

                if (user != null)
                {
                    var mainForm = new Form1();
                    mainForm.CurrentUser = user; 

                    this.Hide();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Clear();
                    txtPass.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void iconShowPass_Click(object sender, EventArgs e)
        {
            if(txtPass.PasswordChar == '*')
            {
                iconShowPass.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
                txtPass.PasswordChar = '\0';
            }
            else if(txtPass.PasswordChar == '\0')
            {
                iconShowPass.IconChar = FontAwesome.Sharp.IconChar.Eye;
                txtPass.PasswordChar = '*';
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPass.Clear();
            txtUser.Focus();
        }

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0X112, 0xf012, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}

using MembershipSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MembershipSystem.Forms
{
    public partial class frmSettings : Form
    {

        User CurrentUser { get; set;}

        private Form1 mainForm;

        public frmSettings(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void Logout()
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro que desea cerrar sesión?",
                    "Confirmar Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    
                    CurrentUser = null;

                    
                    var loginForm = new frmLogin();
                    loginForm.Show();

                    
                    if (mainForm != null)
                    {
                        mainForm.Close();
                    }
                    else
                    {
                        
                        foreach (Form form in Application.OpenForms)
                        {
                            if (form is frmLogin)
                                continue;
                            form.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cerrar sesión: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {

        }
    }
}

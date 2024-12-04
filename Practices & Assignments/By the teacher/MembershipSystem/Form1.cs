using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MembershipSystem.Forms;
using MembershipSystem.Models;

namespace MembershipSystem
{
    public partial class Form1 : Form
    {
        private Timer timer1;
        private Employee _currentUser;

        public Employee CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; UpdateUserInfo(); }
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wnsg, int wparam, int lparam);

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]



        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public Form1()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            pnlNav.Height = btnDashboard.Height;
            pnlNav.Top = btnDashboard.Top;
            pnlNav.Left = btnDashboard.Left;
            btnDashboard.BackColor = Color.FromArgb(46, 51, 73);

            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();


            InitializeDashboard();
            UpdateUserInfo();

        }

        private void UpdateUserInfo()
        {
            if (_currentUser != null)
            {
                
                labelUser.Text = _currentUser.Username;
                
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateDataTime();
        }

        private void UpdateDataTime()
        {
            labelDateTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }

        private void InitializeDashboard()
        {
            lblTitle.Text = "Dashboard";
            this.PnlFormLoader.Controls.Clear();
            formDashboard FrmDashboard_Vrb = new formDashboard()
            { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnDashboard.Height;
            pnlNav.Top = btnDashboard.Top;
            pnlNav.Left = btnDashboard.Left;
            btnDashboard.BackColor = Color.FromArgb(46, 51, 73);

            lblTitle.Text = "Dashboard";
            this.PnlFormLoader.Controls.Clear();
            formDashboard FrmDashboard_Vrb = new formDashboard() 
            { Dock = DockStyle.Fill,TopLevel = false, TopMost = true};
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();

        }

        private void btnMembers_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnMembers.Height;
            pnlNav.Top = btnMembers.Top;
            pnlNav.Left = btnMembers.Left;
            btnMembers.BackColor = Color.FromArgb(46, 51, 73);

            lblTitle.Text = "Miembros";
            this.PnlFormLoader.Controls.Clear();
            frmMembers FrmMembers_Vrb = new frmMembers()
            { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmMembers_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmMembers_Vrb);
            FrmMembers_Vrb.Show();
        }

        private void btnMemberships_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnMemberships.Height;
            pnlNav.Top = btnMemberships.Top;
            pnlNav.Left = btnMemberships.Left;
            btnMemberships.BackColor = Color.FromArgb(46, 51, 73);

            lblTitle.Text = "Membresías";
            this.PnlFormLoader.Controls.Clear();
            frmMemberShips FrmMemberShips_Vrb = new frmMemberShips()
            { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmMemberShips_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmMemberShips_Vrb);
            FrmMemberShips_Vrb.Show();
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnPayments.Height;
            pnlNav.Top = btnPayments.Top;
            pnlNav.Left = btnPayments.Left;
            btnPayments.BackColor = Color.FromArgb(46, 51, 73);

            lblTitle.Text = "Pagos";
            this.PnlFormLoader.Controls.Clear();
            frmPayments FrmPayments_Vrb = new frmPayments()
            { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmPayments_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmPayments_Vrb);
            FrmPayments_Vrb.Show();
        }


        private void btnConfiguration_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnConfiguration.Height;
            pnlNav.Top = btnConfiguration.Top;
            pnlNav.Left = btnConfiguration.Left;
            btnConfiguration.BackColor = Color.FromArgb(46, 51, 73);

            lblTitle.Text = "Configuracion";
            this.PnlFormLoader.Controls.Clear();
            frmSettings FrmSettings_Vrb = new frmSettings(this)
            { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmSettings_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmSettings_Vrb);
            FrmSettings_Vrb.Show();
        }

        private void btnDashboard_Leave(object sender, EventArgs e)
        {
            btnDashboard.BackColor = Color.FromArgb(42, 40, 60);
        }

        private void btnMembers_Leave(object sender, EventArgs e)
        {
            btnMembers.BackColor = Color.FromArgb(42, 40, 60);
        }

        private void btnMemberships_Leave(object sender, EventArgs e)
        {
            btnMemberships.BackColor = Color.FromArgb(42, 40, 60);

        }

        private void btnPayments_Leave(object sender, EventArgs e)
        {
            btnPayments.BackColor = Color.FromArgb(42, 40, 60);
        }


        private void btnConfiguration_Leave(object sender, EventArgs e)
        {
            btnConfiguration.BackColor = Color.FromArgb(42, 40, 60);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0X112, 0xf012, 0);
        }
    }
}

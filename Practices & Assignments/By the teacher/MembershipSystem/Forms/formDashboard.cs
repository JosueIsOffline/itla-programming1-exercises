using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MembershipSystem.Controllers;

namespace MembershipSystem.Forms
{
    public partial class formDashboard : Form
    {

        private DashboardController _dashboardController;

        public formDashboard()
        {
            InitializeComponent();
            _dashboardController = new DashboardController();

            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            var dashboard = _dashboardController.GetDashboardData();

            labelTotalMembers.Text = dashboard.TotalMembers.ToString();
            labelActiveMembersTotal.Text = dashboard.ActiveMembers.ToString();
            labelInactiveMembers.Text = dashboard.InactiveMembers.ToString();
            labelTotalPayments.Text = dashboard.TotalRevenue.ToString("C");



            //lstRecentActivities.Items.Clear();
            //lstRecentActivities.Items.AddRange(dashboard.RecentActivities.ToArray());
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}

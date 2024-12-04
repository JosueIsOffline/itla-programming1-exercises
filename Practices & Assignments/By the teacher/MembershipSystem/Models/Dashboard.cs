
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipSystem.Models
{
        public class Dashboard 
    {

        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int InactiveMembers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NewMembersThisMonth { get; set; }

        public Dashboard()
        {
        }

        public void UpdateDashboardData(
            int totalMembers,
            int activeMembers,
            int inactiveMembers,
            decimal totalRevenue,
            int newMembersThisMonth)
        {
            TotalMembers = totalMembers;
            ActiveMembers = activeMembers;
            InactiveMembers = inactiveMembers;
            TotalRevenue = totalRevenue;
            NewMembersThisMonth = newMembersThisMonth;
        }
    }
}

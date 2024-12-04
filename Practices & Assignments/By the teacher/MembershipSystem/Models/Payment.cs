using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipSystem.Models
{
    public class Payment
    {

        public int Id { get; set; }
        public int MembershipId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }

        // Addictional Properties
        public string UserFullName { get; set; }
        public string MembershipName { get; set; }
    }
}

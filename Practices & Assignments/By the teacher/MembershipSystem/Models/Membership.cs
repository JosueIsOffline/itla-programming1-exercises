using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipSystem.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationMonths { get; set; }  // Cambiado a int
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }       // Cambiado de Active a Status
        public decimal Price { get; set; }       // Cambiado a decimal
        public string UserFullName { get; set; }


        public Membership()
        {
            StartDate = DateTime.Now;
            Active = true;
        }

        // Método para verificar si la membresía está vencida
        public bool IsExpired()
        {
            return DateTime.Now > EndDate;
        }

        // Método para calcular días restantes
        public int RemainingDays()
        {
            if (IsExpired()) return 0;
            return (EndDate - DateTime.Now).Days;
        }
    }
}

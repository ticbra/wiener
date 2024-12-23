using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApp.Models
{
        public class Partner
        {
            public int Id { get; set; }
            public string FullName => FirstName + " " + LastName;
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string PartnerNumber { get; set; }
            public string CroatianPIN { get; set; }
            public int PartnerTypeId { get; set; }
            public DateTime CreatedAtUtc { get; set; }
            public string CreateByUser { get; set; }
            public bool IsForeign { get; set; }
            public string ExternalCode { get; set; }
            public string Gender { get; set; }

        public int TotalPolicies { get; set; } // Ukupan broj polica
        public decimal TotalAmount { get; set; } // Ukupan iznos polica
    }
    }

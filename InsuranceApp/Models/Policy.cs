using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.Models
{
    public class Policy
    {
        [Required(ErrorMessage = "Partner ID je obavezan.")]
        public int PartnerId { get; set; }

        [Required(ErrorMessage = "Broj police je obavezan.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Broj police mora biti između 10 i 15 znakova.")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Iznos police je obavezan.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Iznos mora biti veći od 0.")]
        public decimal PolicyAmount { get; set; }
    }
}

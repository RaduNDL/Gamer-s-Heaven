using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class BillingViewModel
    {
        [Required]
        [Display(Name = "Cardholder Name")]
        public string CardholderName { get; set; }

        [Required]
        [CreditCard]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        public string ExpirationDate { get; set; }

        [Required]
        [Display(Name = "CVV")]
        public string CVV { get; set; }
    }
}

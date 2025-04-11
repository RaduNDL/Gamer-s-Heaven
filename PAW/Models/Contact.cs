using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class Contact
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Event
    {
        [Column("EventId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(ErrorMessage = "Maximum length for Name is 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is a required field.")]
        [MaxLength(ErrorMessage = "Maximum length for Description is 200 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Speaker is a required field.")]
        [MaxLength(ErrorMessage = "Maximum length for Speaker is 50 characters.")]
        public string Speaker { get; set; }

        [Required(ErrorMessage = "Place is a required field.")]
        [MaxLength(ErrorMessage = "Maximum length for Place is 50 characters.")]
        public string Place { get; set; }

        [Required(ErrorMessage = "Date is a required field.")]
        public DateTime Date { get; set; }
    }
}

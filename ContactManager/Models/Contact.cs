using ContactManager_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager_API.Models
{
    public class Contact
    {
        public Contact()
        {
            Emails = new List<Email>(); //Initialize the Emails collection.
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateOnly? BirthDate { get; set; } = null!;
        public ICollection<Email> Emails { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}

using ContactManager_API.Models.DTO;
using ContactManager_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager_API.Models
{
    public class Email
    {
        public long Id { get; set; }
        public bool IsPrimary { get; set; }
        public string Address { get; set; } = default!;

        public long? ContactId { get; set; } //Foreign key
        public Contact Contact { get; set; } // Navigation property to the parent Contact

        public static Email MapEmailDTOToEmail(EmailDTO emailDTO)
        {
            return new Email
            {
                Address = emailDTO.Address,
                IsPrimary = emailDTO.IsPrimary,
            };
        }

    }
}

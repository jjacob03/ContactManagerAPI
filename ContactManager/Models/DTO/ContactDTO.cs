namespace ContactManager_API.Models.DTO
{
    public class ContactDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateOnly? BirthDate { get; set; } = null!;
        public ICollection<EmailDTO> Emails { get; set; } = null!;

        public static ContactDTO MapContactToDTO(Contact contact)
        {
            return new ContactDTO
            {
                Id = contact.Id,
                Name = contact.Name,
                BirthDate = contact.BirthDate,
                Emails = contact.Emails.Select(email => new EmailDTO
                {
                    Id = email.Id,
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                }).ToList(),
            };
        }
    }
}

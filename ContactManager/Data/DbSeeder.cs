using ContactManager_API.Models;
using System;

namespace ContactManager_API.Data
{
    public class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Contacts.Any())
            {
                var contact1 = new Contact
                {
                    Id = 1,
                    Name = "Kevin Durant",
                    BirthDate = DateOnly.Parse("1989-01-09"),
                    CreatedDate = DateTime.Now
                };

                var contact2 = new Contact
                {
                    Id = 2,
                    Name = "Damian Lillard",
                    BirthDate = DateOnly.Parse("1990-02-10"),
                    CreatedDate = DateTime.Now
                };

                var contact3 = new Contact
                {
                    Id = 3,
                    Name = "Devin Booker",
                    BirthDate = DateOnly.Parse("1991-03-11"),
                    CreatedDate = DateTime.Now
                };

                var contact4 = new Contact
                {
                    Id = 4,
                    Name = "Jalen Brunson",
                    BirthDate = DateOnly.Parse("1992-04-12"),
                    CreatedDate = DateTime.Now
                };

                var contact5 = new Contact
                {
                    Id = 5,
                    Name = "Luka Doncic",
                    BirthDate = DateOnly.Parse("1993-05-13"),
                    CreatedDate = DateTime.Now

                };

                var email1 = new Email
                {
                    Id = 1,
                    IsPrimary = true,
                    Address = "kdurant@example.com",
                    ContactId = 1
                };

                var email2 = new Email
                {
                    Id = 2,
                    IsPrimary = false,
                    Address = "dlillard@example.com",
                    ContactId = 2
                };

                var email3 = new Email
                {
                    Id = 3,
                    IsPrimary = true,
                    Address = "damianlillard@test.com",
                    ContactId = 2
                };

                var email4 = new Email
                {
                    Id = 4,
                    IsPrimary = true,
                    Address = "dbooker@example.com",
                    ContactId = 3
                };

                var email5 = new Email
                {
                    Id = 5,
                    IsPrimary = false,
                    Address = "jbrunson@example.com",
                    ContactId = 4

                };

                var email6 = new Email
                {
                    Id = 6,
                    IsPrimary = true,
                    Address = "jbrunson@test.com",
                    ContactId = 4
                };

                var email7 = new Email
                {
                    Id = 7,
                    IsPrimary = false,
                    Address = "jbrunson@complete.com",
                    ContactId = 4
                };

                var email8 = new Email
                {
                    Id = 8,
                    IsPrimary = true,
                    Address = "ldoncic@example.com",
                    ContactId = 5
                };

                contact1.Emails.Add(email1);
                contact2.Emails.Add(email2);
                contact2.Emails.Add(email3);
                contact3.Emails.Add(email4);
                contact4.Emails.Add(email5);
                contact4.Emails.Add(email6);
                contact4.Emails.Add(email7);
                contact5.Emails.Add(email8);

                context.Contacts.AddRange(contact1, contact2, contact3, contact4, contact5);
                context.SaveChanges();
            }
        }
    }
}

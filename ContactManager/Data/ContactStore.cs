using ContactManager_API.Models;
using ContactManager_API.Models.DTO;

namespace ContactManager_API.Data
{
    public class ContactStore
    {

        public static List<ContactDTO> contactList = new List<ContactDTO>
        {
            new ContactDTO
            {
                Id=1,
                Name="Kevin Durant",
                BirthDate=DateOnly.Parse("1989-01-09"),
                Emails=new List<EmailDTO>{
                    new EmailDTO() {
                        Id=1,
                        IsPrimary=true,
                        Address="kdurant@example.com"
                    }
                }
            },
            new ContactDTO{
                Id=2,
                Name="Damian Lillard",
                BirthDate=DateOnly.Parse("1990-02-10"),
                Emails=new List<EmailDTO>{
                    new EmailDTO() {
                        Id=2,
                        IsPrimary=false,
                        Address="dlillard@example.com"
                    }  ,
                    new EmailDTO() {
                        Id=3,
                        IsPrimary=true,
                        Address="damianlillard@test.com"
                    }
                }
            },
            new ContactDTO{
                Id=1,
                Name="Devin Booker",
                BirthDate=DateOnly.Parse("1991-03-11"),
                Emails=new List<EmailDTO>{
                    new EmailDTO() {
                        Id=4,
                        IsPrimary=true,
                        Address="dbooker@example.com"
                    }
                }
            },
            new ContactDTO{
                Id=1,
                Name="Jalen Brunson",
                BirthDate=DateOnly.Parse("1992-04-12"),
                Emails=new List<EmailDTO>{
                    new EmailDTO() {
                        Id=5,
                        IsPrimary=false,
                        Address="jbrunson@example.com"
                    }
                    ,
                    new EmailDTO() {
                        Id=6,
                        IsPrimary=true,
                        Address="jbrunson@test.com"
                    }
                    ,
                    new EmailDTO() {
                        Id=7,
                        IsPrimary=false,
                        Address="jbrunson@complete.com"
                    }
                }
            },
            new ContactDTO{
                Id=1,
                Name="Luka Doncic",
                BirthDate=DateOnly.Parse("1993-05-13"),
                Emails=new List<EmailDTO>{
                    new EmailDTO() {
                        Id=8,
                        IsPrimary=true,
                        Address="ldoncic@example.com"
                    }
                }
            }
        };
    }
}

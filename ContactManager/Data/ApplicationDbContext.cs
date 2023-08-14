using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using ContactManager_API.Models;
using ContactManager_API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContactManager_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; } //This (Contact) will be the name of the database table when EF creates it.
        public DbSet<Email> Email { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Contact>()
              .Property(e => e.BirthDate)
              .HasConversion(new DateOnlyConverter());

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Emails)
                .WithOne(e => e.Contact)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            #region Seed Data For SQL DB
            /*modelBuilder.Entity<Contact>().HasData(
                new Contact
                {
                    Id = 1,
                    Name = "Kevin Durant",
                    BirthDate = DateOnly.Parse("1989-01-09"),
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Id = 2,
                    Name = "Damian Lillard",
                    BirthDate = DateOnly.Parse("1990-02-10"),
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Id = 3,
                    Name = "Devin Booker",
                    BirthDate = DateOnly.Parse("1991-03-11"),
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Id = 4,
                    Name = "Jalen Brunson",
                    BirthDate = DateOnly.Parse("1992-04-12"),
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Id = 5,
                    Name = "Luka Doncic",
                    BirthDate = DateOnly.Parse("1993-05-13"),
                    CreatedDate = DateTime.Now

                });

            modelBuilder.Entity<Email>().HasData(
                new Email()
                {
                    Id = 1,
                    IsPrimary = true,
                    Address = "kdurant@example.com",
                    ContactId = 1
                },
               new Email()
               {
                   Id = 2,
                   IsPrimary = false,
                   Address = "dlillard@example.com",
                   ContactId = 2
               },
               new Email()
               {
                   Id = 3,
                   IsPrimary = true,
                   Address = "damianlillard@test.com",
                   ContactId = 2
               },
               new Email()
               {
                   Id = 4,
                   IsPrimary = true,
                   Address = "dbooker@example.com",
                   ContactId = 3
               },
                new Email()
                {
                    Id = 5,
                    IsPrimary = false,
                    Address = "jbrunson@example.com",
                    ContactId = 4

                },
                new Email()
                {
                    Id = 6,
                    IsPrimary = true,
                    Address = "jbrunson@test.com",
                    ContactId = 4
                }
                ,
                new Email()
                {
                    Id = 7,
                    IsPrimary = false,
                    Address = "jbrunson@complete.com",
                    ContactId = 4
                },
                new Email()
                {
                    Id = 8,
                    IsPrimary = true,
                    Address = "ldoncic@example.com",
                    ContactId = 5
                }
           );*/
            #endregion
        }
    }
}

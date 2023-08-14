using System;
using Microsoft.AspNetCore.Mvc;
using ContactManager_API.Data;
using ContactManager_API.Models.DTO;
using ContactManager_API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ContactManager_API.Controllers
{
    [Route("api/ContactManagementAPI")]
    [ApiController]
    public class ContactManagerAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ContactManagerAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        #region GetContacts
        [HttpGet("GetContacts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ContactDTO>> GetContacts()
        {
            var contacts = _db.Contacts.Include(c => c.Emails).ToList(); //Get all contacts (inlcuding email list) from the database.

            // Map contacts to ContactDTO
            var contactDTOs = contacts.Select(contact => new ContactDTO
            {
                Id = contact.Id,
                Name = contact.Name,
                BirthDate = contact.BirthDate,
                Emails = contact.Emails.Select(email => new EmailDTO
                {
                    Id = email.Id,
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                }).ToList()
            }).ToList();

            return Ok(contactDTOs);
        }
        #endregion

        #region SearchContact
        [HttpGet("SearchContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ContactDTO>> SearchContact(string name, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Contact> query = _db.Contacts.Include(c => c.Emails).AsQueryable();

            //Querying database using just name.
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(contact => contact.Name.Contains(name));
            }

            #region DEPRECATED
            //DEPRECEATED: Cannot do this because the LINQ query cannot translate this, so we have to do a simple query first using name above
            //then further filter out the list manually below.
            /*if (startDate.HasValue)
            {
                DateTime? startDateTime = startDate.HasValue ? startDate.Value.Date : (DateTime?)null;
                query = query.Where(contact => (contact.BirthDate != null ? contact.BirthDate.Value.ToDateTime(TimeOnly.MinValue) : null) >= startDateTime);
            }

            if (endDate.HasValue)
            {
                DateTime? endDateTime = endDate.HasValue ? endDate.Value.Date.AddDays(1).AddTicks(-1) : (DateTime?)null;
                query = query.Where(contact => (contact.BirthDate != null ? contact.BirthDate.Value.ToDateTime(TimeOnly.MinValue) : null) <= endDateTime);
            }*/
            #endregion

            var contacts = query.ToList();

            // Map contacts to DTO and return. We are further filtering the list based on birth date.
            var contactDTOs = contacts
                .Where(contact => startDate == null || (contact.BirthDate != null && contact.BirthDate.Value.ToDateTime(TimeOnly.MinValue) >= startDate.Value.Date))
                .Where(contact => endDate == null || (contact.BirthDate != null && contact.BirthDate.Value.ToDateTime(TimeOnly.MinValue) <= endDate.Value.Date.AddDays(1).AddTicks(-1)))
                .Select(contact => ContactDTO.MapContactToDTO(contact))
                .ToList();

            return Ok(contactDTOs);
        }
        #endregion

        #region CreateContact
        [HttpPost("CreateContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ContactDTO> CreateContact([FromBody] ContactDTO contactDTO)
        {
            if (_db.Contacts.FirstOrDefault(u => u.Name.ToLower() == contactDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Contact already exists!");
                return BadRequest(ModelState);
            }
            if (contactDTO == null)
            {
                return BadRequest(contactDTO);
            }
            if (contactDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // Check if more than one email has IsPrimary = true
            var primaryEmailCount = contactDTO.Emails.Count(email => email.IsPrimary);
            if (primaryEmailCount != 1)
            {
                return BadRequest("Exactly one email should be marked as primary.");
            }

            var model = new Contact
            {
                Name = contactDTO.Name,
                BirthDate = contactDTO.BirthDate,
                Emails = new List<Email>()
            };

            foreach (var emailDTO in contactDTO.Emails)
            {
                var email = Email.MapEmailDTOToEmail(emailDTO);
                email.ContactId = contactDTO.Id;
                email.Contact = model;
                model.Emails.Add(email);

            }

            _db.Contacts.Add(model);
            _db.SaveChanges();

            return CreatedAtAction("GetContacts", new { id = contactDTO.Id }, contactDTO);

        }
        #endregion

        #region DeleteContact
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteContact")]
        public IActionResult DeleteContact([FromQuery] int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var contact = _db.Contacts.Include(c => c.Emails).FirstOrDefault(u => u.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            _db.RemoveRange(contact.Emails); //Removing emails first before contact.
            _db.Contacts.Remove(contact); //Removing contact itself.
            _db.SaveChanges();
            return NoContent();
        }
        #endregion

        #region UpdateContact
        [HttpPut("UpdateContact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateContact(int id, [FromBody] ContactDTO contactDTO)
        {
            if (contactDTO == null || id != contactDTO.Id)
            {
                return BadRequest();
            }
            // Check if more than one email has IsPrimary = true
            var primaryEmailCount = contactDTO.Emails.Count(email => email.IsPrimary);
            if (primaryEmailCount != 1)
            {
                return BadRequest("Exactly one email should be marked as primary.");
            }

            var existingContact = _db.Contacts
                              .Include(c => c.Emails)
                              .FirstOrDefault(c => c.Id == id);

            if (existingContact == null)
            {
                return NotFound();
            }

            existingContact.Name = contactDTO.Name;
            existingContact.BirthDate = contactDTO.BirthDate;

            // Clear existing emails and update with new ones
            existingContact.Emails.Clear();
            foreach (var emailDTO in contactDTO.Emails)
            {
                var email = Email.MapEmailDTOToEmail(emailDTO);
                email.ContactId = contactDTO.Id;
                email.Contact = existingContact;
                existingContact.Emails.Add(email);
            }

            _db.Contacts.Update(existingContact);
            _db.SaveChanges();

            return NoContent();
        }
        #endregion

        #region GetContact
        [HttpGet("GetContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ContactDTO>> GetContact([FromQuery] int? id)
        {
            IQueryable<Contact> query = _db.Contacts.Include(c => c.Emails).AsQueryable();

            //Querying database using just name.
            if (id.HasValue)
            {
                query = query.Where(contact => contact.Id == id);
            }

            var contact = query.FirstOrDefault();

            if (contact == null)
            {
                return NotFound("Contact not found.");
            }

            // Map contact to DTO and return. 
            var contactDTO = ContactDTO.MapContactToDTO(contact);

            return Ok(contactDTO);
        }
        #endregion


    }

}

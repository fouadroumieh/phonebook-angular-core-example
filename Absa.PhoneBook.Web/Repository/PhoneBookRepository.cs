using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhoneBook.Web.Models;

namespace PhoneBook.Web.Repository
{
    public class PhoneBookRepository : IPhoneBookRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PhoneBookRepository> _logger;
        private readonly string _connectionString;

        public PhoneBookRepository(IConfiguration configuration, ILogger<PhoneBookRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connectionString = _configuration.GetValue<String>("Database:connectionString");
        }

        public bool UpdateContact(PhoneBookContactDto phoneBookContactDto)
        {
            using (var db = new LiteRepository(_connectionString))
            {
                var contact = db.Query<PhoneBookContact>()
                    .Where(x => x.Name.Equals(phoneBookContactDto.Name, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                List<Entry> entries = contact?.Entries.ToList() ?? new List<Entry>();
                entries.Add(new Entry
                {
                    PhoneNumber = phoneBookContactDto.Number,
                    EntryType = phoneBookContactDto.EntryType
                });

                if (contact != null)
                    db.Upsert(new PhoneBookContact
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Entries = entries
                    });
            }
            return true;
        }
        public bool InsertContact(PhoneBookContactDto phoneBookContactDto)
        {
            using (var db = new LiteRepository(_connectionString))
            {
                db.Insert(
                    new PhoneBookContact
                    {
                        Name = phoneBookContactDto.Name,
                        Entries = new List<Entry>
                        {
                            new Entry
                            {
                                PhoneNumber = phoneBookContactDto.Number,
                                EntryType = phoneBookContactDto.EntryType
                            }
                        }
                    });
            }

            return true;
        }
        public bool DeleteContact(PhoneBookContactDto phoneBookContactDto)
        {
            using (var db = new LiteRepository(_connectionString))
            {
                //Check if contact alreay exists
                var contact = db.Query<PhoneBookContact>()
                    .Where(x => x.Name.Equals(phoneBookContactDto.Name, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                BsonValue value = new BsonValue(contact.Id);

                return db.Delete<PhoneBookContact>(value);
            }
        }
        public bool CheckifContactExists(string contactName)
        {
            using (var db = new LiteRepository(_connectionString))
            {
                return db.Query<PhoneBookContact>()
                    .Include(x => x.Entries)
                    .Where(x => x.Name.Equals(contactName, StringComparison.OrdinalIgnoreCase))
                    .Exists();
            }

        }
        public bool CheckifContactEntryExists(PhoneBookContactDto phoneBookContactDto)
        {
            using (var db = new LiteRepository(_connectionString))
            {
                var contact = db.Query<PhoneBookContact>()
                    .Where(x => x.Name.Equals(phoneBookContactDto.Name, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                List<Entry> entries = contact?.Entries.ToList() ?? new List<Entry>();

                return entries.Any(x => x.EntryType.ToLower() == phoneBookContactDto.EntryType.ToLower()
                                        && x.PhoneNumber.ToLower() == phoneBookContactDto.Number.ToLower());
            }
        }
        public IList<PhoneBookContact> GetAllContacts()
        {
            using (var db = new LiteRepository(_connectionString))
            {
                return db.Query<PhoneBookContact>()
                    .Include(x => x.Entries)
                    .ToList();
            }
        }
    }
}

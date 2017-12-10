using System;
using System.Collections.Generic;

namespace PhoneBook.Web.Models
{
    public class PhoneBookContact : IModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Entry> Entries { get; set; }
    }
}

using System.Collections.Generic;
using PhoneBook.Web.Models;

namespace PhoneBook.Web.Repository
{
    public interface IPhoneBookRepository
    {
        bool CheckifContactExists(string contactName);
        bool UpdateContact(PhoneBookContactDto phoneBookContact);
        bool InsertContact(PhoneBookContactDto phoneBookContact);
        IList<PhoneBookContact> GetAllContacts();
        bool DeleteContact(PhoneBookContactDto phoneBookContactDto);
        bool CheckifContactEntryExists(PhoneBookContactDto phoneBookContactDto);
    }
}

using System.Linq;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Brokers
{
    internal partial interface IStorageBroker
    {
        Contact InsertContact(Contact contact);
        Contact UpdateContact(Contact contact);
        Contact DeleteContact(Contact contact);
        IQueryable<Contact> SelectAllContacts();
    }
}

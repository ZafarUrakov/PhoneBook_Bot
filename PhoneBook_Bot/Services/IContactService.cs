using System.Linq;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Services
{
    internal interface IContactService
    {
        Contact AddContact(Contact contact);
        Contact ModifyContact(Contact contact);
        Contact RemoveContact(Contact contact);
        IQueryable<Contact> RetrieveAllContact();
    }
}

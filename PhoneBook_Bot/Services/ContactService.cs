using System.Linq;
using PhoneBook_Bot.Brokers;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Services
{
    internal class ContactService : IContactService
    {
        private readonly IStorageBroker storageBroker = new StorageBroker();

        public Contact AddContact(Contact contact) =>
            this.storageBroker.InsertContact(contact);

        public Contact ModifyContact(Contact contact) =>
            this.storageBroker.UpdateContact(contact);

        public Contact RemoveContact(Contact contact) =>
            this.storageBroker.DeleteContact(contact);

        public IQueryable<Contact> RetrieveAllContact() =>
            this.storageBroker.SelectAllContacts();
    }
}

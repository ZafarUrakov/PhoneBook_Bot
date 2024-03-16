using System.Linq;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Brokers
{
    internal partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }

        public StorageBroker() =>
            this.Database.EnsureCreated();

        public Contact InsertContact(Contact contact)
        {
            this.Contacts.Add(contact);
            this.SaveChanges();

            return contact;
        }

        public Contact UpdateContact(Contact contact)
        {
            var broke = new StorageBroker();
            broke.Entry(contact).State = EntityState.Modified;
            broke.SaveChanges();

            return contact;
        }

        public Contact DeleteContact(Contact contact)
        {
            var broke = new StorageBroker();
            broke.Entry(contact).State = EntityState.Deleted;
            broke.SaveChanges();

            return contact;
        }

        public IQueryable<Contact> SelectAllContacts()
        {
            var broker = new StorageBroker();

            return broker.Set<Contact>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data source = ../../../PhoneBook.Db";
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}

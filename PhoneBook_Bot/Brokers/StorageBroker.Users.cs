using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Brokers
{
    internal partial class StorageBroker
    {
        public User InsertUser(User user)
        {
            this.Users.Add(user);
            this.SaveChanges();

            return user;
        }

        public User UpdateUser(User user)
        {
            var broke = new StorageBroker();
            broke.Entry(user).State = EntityState.Modified;
            broke.SaveChanges();

            return user;
        }

        public IQueryable<User> SelectAllUsers()
        {
            var broker = new StorageBroker();

            return broker.Set<User>();
        }
    }
}

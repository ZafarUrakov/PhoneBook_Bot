using System.Linq;
using PhoneBook_Bot.Brokers;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Services
{
    internal class UserService : IUserService
    {
        private readonly IStorageBroker storageBroker = new StorageBroker();

        public User AddUser(User user) =>
            this.storageBroker.InsertUser(user);

        public User ModifyUser(User user) =>
            this.storageBroker.UpdateUser(user);

        public IQueryable<User> RetrieveAllUser() =>
            this.storageBroker.SelectAllUsers();
    }
}

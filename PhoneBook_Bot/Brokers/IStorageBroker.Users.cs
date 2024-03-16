using System.Linq;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Brokers
{
    internal partial interface IStorageBroker
    {
        User InsertUser(User user);
        User UpdateUser(User user);
        IQueryable<User> SelectAllUsers();
    }
}

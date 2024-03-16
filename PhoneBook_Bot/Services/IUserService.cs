using System.Linq;
using PhoneBook_Bot.Models;

namespace PhoneBook_Bot.Services
{
    internal interface IUserService
    {
        User AddUser(User user);
        User ModifyUser(User user);
        IQueryable<User> RetrieveAllUser();
    }
}

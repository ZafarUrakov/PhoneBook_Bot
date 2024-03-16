using System;

namespace PhoneBook_Bot.Models
{
    internal class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }

        public string HelperName { get; set; }
    }
}

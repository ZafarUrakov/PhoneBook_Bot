using System;

namespace PhoneBook_Bot.Models
{
    internal class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}

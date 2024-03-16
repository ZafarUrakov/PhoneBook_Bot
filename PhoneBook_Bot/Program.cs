using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PhoneBook_Bot.Models;
using PhoneBook_Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

internal class Program
{
    private readonly static string token = "7175346184:AAH0WM0eV1XBZRGfsVEysde5cIaUGxaYVTM";
    private readonly static ITelegramBotClient telegramBotClient = new TelegramBotClient(token);
    private readonly static IUserService userService = new UserService();
    private readonly static IContactService contactService = new ContactService();
    private static void Main(string[] args)
    {
        telegramBotClient.StartReceiving(HandleMessage, HandleError);
        Console.ReadKey();
    }

    private static async Task HandleMessage(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.Message.Type is MessageType.Text)
        {
            var maybeUser = userService.RetrieveAllUser()
                .FirstOrDefault(m => m.Name == update.Message.Chat.FirstName);

            if (update.Message.Text is "/start")
            {
                EnsureUser(update, maybeUser);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: MenuButton(),
                    text: "Phone bookga hush kelibsiz.\nMenu 👇🏻");

                return;
            }
            if (update.Message.Text is "Add contact")
            {
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: new ReplyKeyboardRemove(),
                    text: "Iltimos, kontakni ismini yuboring.");

                maybeUser.Status = Status.Name;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.Name)
            {
                var contact = new PhoneBook_Bot.Models.Contact
                {
                    Id = Guid.NewGuid(),
                    Name = update.Message.Text
                };

                contactService.AddContact(contact);

                maybeUser.HelperName = contact.Name;
                userService.ModifyUser(maybeUser);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Iltimos, kontakni nomerini yuboring.");

                maybeUser.Status = Status.PhoneNumber;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.PhoneNumber)
            {
                var contact = contactService.RetrieveAllContact()
                    .FirstOrDefault(c => c.Name == maybeUser.HelperName);

                contact.PhoneNumber = update.Message.Text;
                contactService.ModifyContact(contact);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: MenuButton(),
                    text: $"Kontakt saqlandi.\nContact:\nName: " +
                    $"{contact.Name}\nNumber: {contact.PhoneNumber}");

                maybeUser.Status = Status.Active;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (update.Message.Text is "All contacts")
            {
                var contacts = contactService.RetrieveAllContact();
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var contact in contacts)
                {
                    stringBuilder.AppendLine($"Name: {contact.Name}\nNumber: {contact.PhoneNumber}\n");
                }

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Kantaktlar:\n\n{stringBuilder.ToString()}");

                return;
            }
            if (update.Message.Text is "Update contact")
            {
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"O'zgartiriladigan kontakni ismini yuboring.");

                maybeUser.Status = Status.UpdateContact;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.UpdateContact)
            {
                var contact = contactService.RetrieveAllContact()
                    .FirstOrDefault(c => c.Name == update.Message.Text);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: EditButton(),
                    text: $"Kontakt:\nName: {contact.Name}\nNumber:{contact.PhoneNumber}\n");

                maybeUser.Status = Status.ProcessUpdateContact;
                maybeUser.HelperName = contact.Name;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.ProcessUpdateContact
                && update.Message.Text is "Edit name")
            {
                var contact = contactService.RetrieveAllContact()
                    .FirstOrDefault(c => c.Name == maybeUser.HelperName);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: EditButton(),
                    text: $"Yangi ism yuboring.");

                maybeUser.Status = Status.UpdateName;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.UpdateName)
            {
                var contact = contactService.RetrieveAllContact()
                    .FirstOrDefault(c => c.Name == maybeUser.HelperName);

                contact.Name = update.Message.Text;
                contactService.ModifyContact(contact);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: MenuButton(),
                    text: $"Yangilandi\nKontakt:\nName: " +
                    $"{contact.Name}\nNumber: {contact.PhoneNumber}");

                maybeUser.Status = Status.Active;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.ProcessUpdateContact
                && update.Message.Text is "Edit number")
            {
                var contact = contactService.RetrieveAllContact()
                    .FirstOrDefault(c => c.Name == maybeUser.HelperName);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: EditButton(),
                    text: $"Yangi nomer yuboring.");

                maybeUser.Status = Status.UpdateNumber;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.UpdateNumber)
            {
                var contact = contactService.RetrieveAllContact()
                    .FirstOrDefault(c => c.Name == maybeUser.HelperName);

                contact.PhoneNumber = update.Message.Text;
                contactService.ModifyContact(contact);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: MenuButton(),
                    text: $"Yangilandi\nKontakt:\nName: " +
                    $"{contact.Name}\nNumber: {contact.PhoneNumber}");

                maybeUser.Status = Status.Active;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (update.Message.Text is "Delete contacts")
            {
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: new ReplyKeyboardRemove(),
                    text: $"O'chiradigan kontakni ismini yuboring.");

                maybeUser.Status = Status.DeleteContact;
                userService.ModifyUser(maybeUser);

                return;
            }
            if (maybeUser.Status is Status.DeleteContact)
            {
                var contact = contactService.RetrieveAllContact()
                    .FirstOrDefault(c => c.Name == update.Message.Text);

                contactService.RemoveContact(contact);

                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyMarkup: MenuButton(),
                    text: $"O'chirildi.");

                maybeUser.Status = Status.Active;
                userService.ModifyUser(maybeUser);

                return;
            }
        }
        else
        {
            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Faqat text habar yuboring.");
        }
    }

    private static void EnsureUser(Update update, PhoneBook_Bot.Models.User maybeUser)
    {
        if (maybeUser is null)
        {
            var user = new PhoneBook_Bot.Models.User
            {
                Id = Guid.NewGuid(),
                Name = update.Message.Chat.FirstName,
                Status = Status.Active
            };

            userService.AddUser(user);
        }
        else
        {
            maybeUser.Status = Status.Active;
            userService.ModifyUser(maybeUser);
        }
    }

    private static ReplyKeyboardMarkup MenuButton()
    {
        var keyboardButtons = new List<KeyboardButton[]>
        {
            new KeyboardButton[]
            {
                new KeyboardButton("Add contact"),
                new KeyboardButton("All contacts"),
            },
            new KeyboardButton[]
            {
                new KeyboardButton("Update contact"),
                new KeyboardButton("Delete contacts"),
            }
        };

        return new ReplyKeyboardMarkup(keyboardButtons)
        {
            ResizeKeyboard = true
        };
    }

    private static ReplyKeyboardMarkup EditButton()
    {
        var keyboardButtons = new List<KeyboardButton[]>
        {
            new KeyboardButton[]
            {
                new KeyboardButton("Edit name"),
                new KeyboardButton("Edit number"),
            }
        };

        return new ReplyKeyboardMarkup(keyboardButtons)
        {
            ResizeKeyboard = true
        };
    }

    private static async Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        Console.WriteLine(exception.Message);
    }
}
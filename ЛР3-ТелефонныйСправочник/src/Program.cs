using PhoneBookApp;

namespace PhoneBookApp;


class Program
{
    static void Main(string[] args)
    {
        var dataFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", "data", "contacts.json");

        var phoneBook = new PhoneBook(dataFile);

        while (true)
        {
            ShowMenu();
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1": phoneBook.AddContact(); break;
                case "2": phoneBook.ViewAllContacts(); break;
                case "3": phoneBook.SearchContact(); break;
                case "4": phoneBook.DeleteContact(); break;
                case "5":
                    phoneBook.SaveContacts();
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Введите число от 1 до 5.");
                    break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n=== Телефонный справочник ===");
        Console.WriteLine("1. Добавить контакт");
        Console.WriteLine("2. Просмотреть все контакты");
        Console.WriteLine("3. Найти контакт");
        Console.WriteLine("4. Удалить контакт");
        Console.WriteLine("5. Сохранить и выйти");
        Console.WriteLine("=============================");
        Console.Write("Выберите действие (1-5): ");
    }
}

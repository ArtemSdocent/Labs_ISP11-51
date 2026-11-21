using System.Text.Json;

namespace PhoneBookApp;


public class PhoneBook
{
    private readonly Dictionary<string, Contact> _contacts;
    private readonly string _filePath;

    public PhoneBook(string filePath)
    {
        _filePath = filePath;
        _contacts = new Dictionary<string, Contact>();
        LoadContacts();
    }

   
    private void LoadContacts()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine("Файл не найден. Начинаем с пустого списка.");
            return;
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            var loaded = JsonSerializer.Deserialize<Dictionary<string, Contact>>(json, options);
            if (loaded != null)
            {
                foreach (var kvp in loaded)
                    _contacts[kvp.Key] = kvp.Value;
            }
            Console.WriteLine($"Загружено {_contacts.Count} контактов.");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка чтения JSON: {ex.Message}. Начинаем с пустого списка.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка файла: {ex.Message}. Начинаем с пустого списка.");
        }
    }

   
    public void SaveContacts()
    {
        try
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_contacts, options));
            Console.WriteLine($"Сохранено {_contacts.Count} контактов.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }

   
    public void AddContact()
    {
        Console.Write("Введите имя: ");
        var name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Имя не может быть пустым.");
            return;
        }

        if (_contacts.ContainsKey(name))
        {
            Console.WriteLine($"Контакт '{name}' уже существует (номер: {_contacts[name].Phone}).");
            Console.Write("Перезаписать? (да/нет): ");
            var answer = Console.ReadLine()?.Trim().ToLower();
            if (answer != "да") return;
        }

        Console.Write("Введите номер телефона: ");
        var phone = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(phone))
        {
            Console.WriteLine("Номер не может быть пустым.");
            return;
        }

        _contacts[name] = new Contact(name, phone);
        Console.WriteLine($"Контакт '{name}' добавлен.");
    }

    /// <summary>Просмотр всех контактов.</summary>
    public void ViewAllContacts()
    {
        if (_contacts.Count == 0)
        {
            Console.WriteLine("Список контактов пуст.");
            return;
        }

        Console.WriteLine("\n--- Все контакты ---");
        foreach (var contact in _contacts.Values)
            Console.WriteLine($"  {contact}");
        Console.WriteLine($"--- Всего: {_contacts.Count} ---\n");
    }

    /// <summary>Поиск контакта по имени (регистронезависимый).</summary>
    public void SearchContact()
    {
        Console.Write("Введите имя для поиска: ");
        var query = Console.ReadLine()?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Запрос не может быть пустым.");
            return;
        }

        var found = _contacts.Values
            .Where(c => c.Name.ToLower().Contains(query))
            .ToList();

        if (found.Count > 0)
        {
            Console.WriteLine($"\n--- Найдено: {found.Count} ---");
            foreach (var c in found) Console.WriteLine($"  {c}");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Контакты не найдены.");
        }
    }

    /// <summary>Удаление контакта по имени.</summary>
    public void DeleteContact()
    {
        Console.Write("Введите имя для удаления: ");
        var name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Имя не может быть пустым.");
            return;
        }

        if (_contacts.Remove(name))
            Console.WriteLine($"Контакт '{name}' удалён.");
        else
            Console.WriteLine("Контакт не найден.");
    }
}

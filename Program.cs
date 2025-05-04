using System;
using System.Collections.Generic;
using System.Text;

// --- Factory Method для створення користувачів ---
public abstract class AbstractUser
{
    public abstract void ShowRole();
}

public class Administrator : AbstractUser
{
    public override void ShowRole() => Console.WriteLine("Я Адміністратор системи.");
}

public class Supervisor : AbstractUser
{
    public override void ShowRole() => Console.WriteLine("Я Керівник.");
}

public class Worker : AbstractUser
{
    public override void ShowRole() => Console.WriteLine("Я Працівник компанії.");
}

public abstract class AbstractUserFactory
{
    public abstract AbstractUser MakeUser();
}

public class AdministratorFactory : AbstractUserFactory
{
    public override AbstractUser MakeUser() => new Administrator();
}

public class SupervisorFactory : AbstractUserFactory
{
    public override AbstractUser MakeUser() => new Supervisor();
}

public class WorkerFactory : AbstractUserFactory
{
    public override AbstractUser MakeUser() => new Worker();
}

// --- Composite для представлення файлової структури ---
public interface IFileComponent
{
    void Render(string indent = "");
}

public class FileLeaf : IFileComponent
{
    private readonly string _filename;

    public FileLeaf(string filename)
    {
        _filename = filename;
    }

    public void Render(string indent = "")
    {
        Console.WriteLine($"{indent}Файл: {_filename}");
    }
}

public class DirectoryComposite : IFileComponent
{
    private readonly List<IFileComponent> _items = new();
    private readonly string _directoryName;

    public DirectoryComposite(string directoryName)
    {
        _directoryName = directoryName;
    }

    public void Add(IFileComponent component)
    {
        _items.Add(component);
    }

    public void Render(string indent = "")
    {
        Console.WriteLine($"{indent}Папка: {_directoryName}");
        foreach (var item in _items)
        {
            item.Render(indent + "  ");
        }
    }
}

// --- Strategy для шифрування тексту ---
public interface IEncryptStrategy
{
    string Encrypt(string input);
}

public class AesEncryptor : IEncryptStrategy
{
    public string Encrypt(string input)
    {
        return $"[AES] {Convert.ToBase64String(Encoding.UTF8.GetBytes(input))}";
    }
}

public class RsaEncryptor : IEncryptStrategy
{
    public string Encrypt(string input)
    {
        return $"[RSA] {Convert.ToBase64String(Encoding.UTF8.GetBytes(input))}";
    }
}

public class SimpleBase64Encryptor : IEncryptStrategy
{
    public string Encrypt(string input)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }
}

public class DataEncryptor
{
    private IEncryptStrategy _encryptStrategy;

    public void ChangeStrategy(IEncryptStrategy strategy)
    {
        _encryptStrategy = strategy;
    }

    public void ExecuteEncryption(string input)
    {
        Console.WriteLine(_encryptStrategy.Encrypt(input));
    }
}

// --- Основна програма ---
class Program
{
    static void Main()
    {
        Console.WriteLine("Оберіть патерн для демонстрації:");
        Console.WriteLine("1. Factory Method");
        Console.WriteLine("2. Composite");
        Console.WriteLine("3. Strategy");
        Console.Write("Ваш вибір (1-3): ");

        var choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                RunFactoryMethod();
                break;
            case "2":
                RunComposite();
                break;
            case "3":
                RunStrategy();
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }

    static void RunFactoryMethod()
    {
        Console.WriteLine("=== Factory Method: Створення користувачів ===");
        AbstractUserFactory userFactory = new AdministratorFactory();
        AbstractUser admin = userFactory.MakeUser();
        admin.ShowRole();

        userFactory = new SupervisorFactory();
        AbstractUser supervisor = userFactory.MakeUser();
        supervisor.ShowRole();

        userFactory = new WorkerFactory();
        AbstractUser worker = userFactory.MakeUser();
        worker.ShowRole();
    }

    static void RunComposite()
    {
        Console.WriteLine("=== Composite: Структура файлів ===");
        var mainFolder = new DirectoryComposite("Головна папка");
        mainFolder.Add(new FileLeaf("інструкція.txt"));

        var documents = new DirectoryComposite("Документи");
        documents.Add(new FileLeaf("звіт1.pdf"));
        documents.Add(new FileLeaf("звіт2.pdf"));

        mainFolder.Add(documents);
        mainFolder.Render();
    }

    static void RunStrategy()
    {
        Console.WriteLine("=== Strategy: Шифрування тексту ===");
        var dataEncryptor = new DataEncryptor();

        dataEncryptor.ChangeStrategy(new AesEncryptor());
        dataEncryptor.ExecuteEncryption("Конфіденційні дані");

        dataEncryptor.ChangeStrategy(new RsaEncryptor());
        dataEncryptor.ExecuteEncryption("Важлива інформація");

        dataEncryptor.ChangeStrategy(new SimpleBase64Encryptor());
        dataEncryptor.ExecuteEncryption("Тестовий текст");
    }
}

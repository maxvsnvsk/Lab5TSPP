using System;
using System.Collections.Generic;
using System.Text;

// --- Factory Method для користувачів ---
public abstract class User
{
    public abstract void DisplayRole();
}

public class Admin : User
{
    public override void DisplayRole() => Console.WriteLine("Я Адміністратор.");
}

public class Manager : User
{
    public override void DisplayRole() => Console.WriteLine("Я Менеджер.");
}

public class Employee : User
{
    public override void DisplayRole() => Console.WriteLine("Я Працівник.");
}

public abstract class UserFactory
{
    public abstract User CreateUser();
}

public class AdminFactory : UserFactory
{
    public override User CreateUser() => new Admin();
}

public class ManagerFactory : UserFactory
{
    public override User CreateUser() => new Manager();
}

public class EmployeeFactory : UserFactory
{
    public override User CreateUser() => new Employee();
}

// --- Composite для файлової системи ---
public interface IFileSystemComponent
{
    void Display(string indent = "");
}

public class File : IFileSystemComponent
{
    private readonly string _name;

    public File(string name)
    {
        _name = name;
    }

    public void Display(string indent = "")
    {
        Console.WriteLine($"{indent}Файл: {_name}");
    }
}

public class Directory : IFileSystemComponent
{
    private readonly List<IFileSystemComponent> _children = new();
    private readonly string _name;

    public Directory(string name)
    {
        _name = name;
    }

    public void Add(IFileSystemComponent component)
    {
        _children.Add(component);
    }

    public void Display(string indent = "")
    {
        Console.WriteLine($"{indent}Папка: {_name}");
        foreach (var child in _children)
        {
            child.Display(indent + "  ");
        }
    }
}

// --- Strategy для шифрування даних ---
public interface IEncryptionStrategy
{
    string Encrypt(string data);
}

public class AesEncryption : IEncryptionStrategy
{
    public string Encrypt(string data)
    {
        return $"[AES] {Convert.ToBase64String(Encoding.UTF8.GetBytes(data))}";
    }
}

public class RsaEncryption : IEncryptionStrategy
{
    public string Encrypt(string data)
    {
        return $"[RSA] {Convert.ToBase64String(Encoding.UTF8.GetBytes(data))}";
    }
}

public class Base64Encryption : IEncryptionStrategy
{
    public string Encrypt(string data)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
    }
}

public class Encryptor
{
    private IEncryptionStrategy _strategy;

    public void SetStrategy(IEncryptionStrategy strategy)
    {
        _strategy = strategy;
    }

    public void EncryptData(string data)
    {
        Console.WriteLine(_strategy.Encrypt(data));
    }
}

// --- Основна програма ---
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Factory Method: Користувачі ===");
        UserFactory factory = new AdminFactory();
        User admin = factory.CreateUser();
        admin.DisplayRole();

        factory = new ManagerFactory();
        User manager = factory.CreateUser();
        manager.DisplayRole();

        factory = new EmployeeFactory();
        User employee = factory.CreateUser();
        employee.DisplayRole();

        Console.WriteLine("\n=== Composite: Файлова система ===");
        var root = new Directory("Root");
        root.Add(new File("readme.txt"));

        var subFolder = new Directory("Docs");
        subFolder.Add(new File("doc1.pdf"));
        subFolder.Add(new File("doc2.pdf"));

        root.Add(subFolder);
        root.Display();

        Console.WriteLine("\n=== Strategy: Шифрування даних ===");
        Encryptor encryptor = new Encryptor();

        encryptor.SetStrategy(new AesEncryption());
        encryptor.EncryptData("Секретні дані");

        encryptor.SetStrategy(new RsaEncryption());
        encryptor.EncryptData("Конфіденційна інформація");

        encryptor.SetStrategy(new Base64Encryption());
        encryptor.EncryptData("Простий текст");
    }
}

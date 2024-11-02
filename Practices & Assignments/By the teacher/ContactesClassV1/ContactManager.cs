using System;
using System.Collections.Generic;
public class ContactManager
{
    private List<Contact> _contacts;

    public ContactManager()
    {
        _contacts = new List<Contact>();
    }

    public void AddContact()
    {
        Console.WriteLine("Digite el nombre de la persona");
        string name = Console.ReadLine().Trim();

        Console.WriteLine("Digite el apellido de la persona");
        string lastName = Console.ReadLine().Trim();

        Console.WriteLine("Digite la dirección");
        string address = Console.ReadLine().Trim();

        Console.WriteLine("Digite el teléfono de la persona");
        string phone = Console.ReadLine().Trim();

        Console.WriteLine("Digite el email de la persona");
        string email = Console.ReadLine().Trim();

        int age;
        do
        {
            Console.WriteLine("Digite la edad de la persona en números");
        } while (!int.TryParse(Console.ReadLine(), out age) || age < 0);

        Console.WriteLine("¿Es mejor amigo? (1. Sí, 2. No)");
        bool isBestFriend = Console.ReadLine().Trim() == "1";

        int id = _contacts.Count > 0 ? _contacts.Max(c => c.Id) + 1 : 1;

        var contact = new Contact(id, name, lastName, address, phone, email, age, isBestFriend);
        _contacts.Add(contact);

        Console.WriteLine("Contacto agregado exitosamente.");
    }

    public void ShowContacts()
    {
        if (_contacts.Count == 0)
        {
            Console.WriteLine("No hay contactos registrados.");
            return;
        }

        Console.WriteLine($"{"Nombre",-15}{"Apellido",-15}{"Dirección",-20}{"Teléfono",-15}{"Email",-25}{"Edad",-10}{"Es Mejor Amigo?",-15}");
        Console.WriteLine(new string('_', 115));

        foreach (var contact in _contacts)
        {
            string isBestFriendStr = contact.IsBestFriend ? "Sí" : "No";
            Console.WriteLine($"{contact.Name,-15}{contact.LastName,-15}{contact.Address,-20}{contact.Telephone,-15}{contact.Email,-25}{contact.Age,-10}{isBestFriendStr,-15}");
        }
    }

    public void SearchContacts()
    {
        Console.WriteLine("Seleccione el criterio de búsqueda:");
        Console.WriteLine("1. Nombre");
        Console.WriteLine("2. Apellido");
        Console.WriteLine("3. Teléfono");
        Console.WriteLine("4. Email");

        if (!int.TryParse(Console.ReadLine(), out int searchOption))
        {
            Console.WriteLine("Opción no válida");
            return;
        }

        Console.WriteLine("Digite el término de búsqueda:");
        string searchTerm = Console.ReadLine().Trim().ToLower();
        IEnumerable<Contact> results = new List<Contact>();

        switch (searchOption)
        {
            case 1:
                results = _contacts.Where(c => c.Name.ToLower().Contains(searchTerm));
                break;
            case 2:
                results = _contacts.Where(c => c.LastName.ToLower().Contains(searchTerm));
                break;
            case 3:
                results = _contacts.Where(c => c.Telephone.Contains(searchTerm));
                break;
            case 4:
                results = _contacts.Where(c => c.Email.ToLower().Contains(searchTerm));
                break;
            default:
                Console.WriteLine("Opción no válida");
                return;
        }

        var matchingContacts = results.ToList();
        if (matchingContacts.Count == 0)
        {
            Console.WriteLine("No se encontraron contactos que coincidan con la búsqueda.");
            return;
        }

        Console.WriteLine($"\nContactos encontrados ({matchingContacts.Count}):");
        foreach (var contact in matchingContacts)
        {
            Console.WriteLine($"ID: {contact.Id}, Nombre: {contact.Name} {contact.LastName}, Teléfono: {contact.Telephone}");
        }
    }

    public void ModifyContact()
    {
        if (_contacts.Count == 0)
        {
            Console.WriteLine("No hay contactos para modificar.");
            return;
        }

        Console.WriteLine("Contactos disponibles:");
        foreach (var contact in _contacts)
        {
            Console.WriteLine($"ID: {contact.Id}, Nombre: {contact.Name} {contact.LastName}");
        }

        Console.WriteLine("\nIngrese el ID del contacto a modificar:");
        if (!int.TryParse(Console.ReadLine(), out int contactId))
        {
            Console.WriteLine("ID no válido.");
            return;
        }

        var contact = _contacts.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
        {
            Console.WriteLine("Contacto no encontrado.");
            return;
        }

        Console.WriteLine("¿Qué desea modificar?");
        Console.WriteLine("1. Nombre");
        Console.WriteLine("2. Apellido");
        Console.WriteLine("3. Dirección");
        Console.WriteLine("4. Teléfono");
        Console.WriteLine("5. Email");
        Console.WriteLine("6. Edad");
        Console.WriteLine("7. Estado de mejor amigo");

        if (!int.TryParse(Console.ReadLine(), out int modifyOption))
        {
            Console.WriteLine("Opción no válida");
            return;
        }

        switch (modifyOption)
        {
            case 1:
                Console.WriteLine("Nuevo nombre:");
                contact.Name = Console.ReadLine().Trim();
                break;
            case 2:
                Console.WriteLine("Nuevo apellido:");
                contact.LastName = Console.ReadLine().Trim();
                break;
            case 3:
                Console.WriteLine("Nueva dirección:");
                contact.Address = Console.ReadLine().Trim();
                break;
            case 4:
                Console.WriteLine("Nuevo teléfono:");
                contact.Telephone = Console.ReadLine().Trim();
                break;
            case 5:
                Console.WriteLine("Nuevo email:");
                contact.Email = Console.ReadLine().Trim();
                break;
            case 6:
                Console.WriteLine("Nueva edad:");
                if (int.TryParse(Console.ReadLine(), out int newAge))
                {
                    contact.Age = newAge;
                }
                break;
            case 7:
                Console.WriteLine("¿Es mejor amigo? (1. Sí, 2. No):");
                contact.IsBestFriend = Console.ReadLine().Trim() == "1";
                break;
            default:
                Console.WriteLine("Opción no válida");
                return;
        }

        Console.WriteLine("Contacto modificado exitosamente.");
    }

    public void DeleteContact()
    {
        if (_contacts.Count == 0)
        {
            Console.WriteLine("No hay contactos para eliminar.");
            return;
        }

        Console.WriteLine("Contactos disponibles:");
        foreach (var contact in _contacts)
        {
            Console.WriteLine($"ID: {contact.Id}, Nombre: {contact.Name} {contact.LastName}");
        }

        Console.WriteLine("\nIngrese el ID del contacto a eliminar:");
        if (!int.TryParse(Console.ReadLine(), out int contactId))
        {
            Console.WriteLine("ID no válido.");
            return;
        }

        var contact = _contacts.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
        {
            Console.WriteLine("Contacto no encontrado.");
            return;
        }

        Console.WriteLine($"¿Está seguro de eliminar a {contact.Name} {contact.LastName}? (S/N)");
        if (Console.ReadLine().Trim().ToUpper() != "S")
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        _contacts.Remove(contact);
        Console.WriteLine("Contacto eliminado exitosamente.");
    }
}
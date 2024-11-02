Console.WriteLine("Bienvenido a mi lista de Contactos");

bool running = true;
List<int> ids = new List<int>();
Dictionary<int, string> names = new Dictionary<int, string>();
Dictionary<int, string> lastnames = new Dictionary<int, string>();
Dictionary<int, string> addresses = new Dictionary<int, string>();
Dictionary<int, string> telephones = new Dictionary<int, string>();
Dictionary<int, string> emails = new Dictionary<int, string>();
Dictionary<int, int> ages = new Dictionary<int, int>();
Dictionary<int, bool> bestFriends = new Dictionary<int, bool>();

while (running)
{
    Console.WriteLine(@"1. Agregar Contacto     2. Ver Contactos    3. Buscar Contactos     4. Modificar Contacto    5. Eliminar Contacto    6. Salir");
    Console.WriteLine("Digite el número de la opción deseada");

    if (!int.TryParse(Console.ReadLine(), out int typeOption))
    {
        Console.WriteLine("Por favor, ingrese un número válido");
        continue;
    }

    switch (typeOption)
    {
        case 1:
            AddContact(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
            break;
        case 2:
            ShowContacts(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
            break;
        case 3:
            SearchContact(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
            break;
        case 4:
            ModifyContact(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
            break;
        case 5:
            DeleteContact(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
            break;
        case 6:
            running = false;
            break;
        default:
            Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú.");
            break;
    }
}

static void ShowContacts(
    List<int> ids,
    Dictionary<int, string> names,
    Dictionary<int, string> lastnames,
    Dictionary<int, string> addresses,
    Dictionary<int, string> telephones,
    Dictionary<int, string> emails,
    Dictionary<int, int> ages,
    Dictionary<int, bool> bestFriends)
{
    if (ids.Count == 0)
    {
        Console.WriteLine("No hay contactos registrados.");
        return;
    }

    Console.WriteLine($"{"Nombre",-15}{"Apellido",-15}{"Dirección",-20}{"Teléfono",-15}{"Email",-25}{"Edad",-10}{"Es Mejor Amigo?",-15}");
    Console.WriteLine(new string('_', 115));

    foreach (var id in ids)
    {
        string isBestFriendStr = bestFriends[id] ? "Sí" : "No";
        Console.WriteLine($"{names[id],-15}{lastnames[id],-15}{addresses[id],-20}{telephones[id],-15}{emails[id],-25}{ages[id],-10}{isBestFriendStr,-15}");
    }
}

static void AddContact(
    List<int> ids,
    Dictionary<int, string> names,
    Dictionary<int, string> lastnames,
    Dictionary<int, string> addresses,
    Dictionary<int, string> telephones,
    Dictionary<int, string> emails,
    Dictionary<int, int> ages,
    Dictionary<int, bool> bestFriends)
{
    Console.WriteLine("Digite el nombre de la persona");
    string name = Console.ReadLine().Trim();

    Console.WriteLine("Digite el apellido de la persona");
    string lastname = Console.ReadLine().Trim();

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

    var id = ids.Count > 0 ? ids.Max() + 1 : 1;
    ids.Add(id);
    names.Add(id, name);
    lastnames.Add(id, lastname);
    addresses.Add(id, address);
    telephones.Add(id, phone);
    emails.Add(id, email);
    ages.Add(id, age);
    bestFriends.Add(id, isBestFriend);

    Console.WriteLine("Contacto agregado exitosamente.");
}

static void SearchContact(
    List<int> ids,
    Dictionary<int, string> names,
    Dictionary<int, string> lastnames,
    Dictionary<int, string> addresses,
    Dictionary<int, string> telephones,
    Dictionary<int, string> emails,
    Dictionary<int, int> ages,
    Dictionary<int, bool> bestFriends)
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
    var matchingIds = new List<int>();

    switch (searchOption)
    {
        case 1:
            matchingIds = ids.Where(id => names[id].ToLower().Contains(searchTerm)).ToList();
            break;
        case 2:
            matchingIds = ids.Where(id => lastnames[id].ToLower().Contains(searchTerm)).ToList();
            break;
        case 3:
            matchingIds = ids.Where(id => telephones[id].Contains(searchTerm)).ToList();
            break;
        case 4:
            matchingIds = ids.Where(id => emails[id].ToLower().Contains(searchTerm)).ToList();
            break;
        default:
            Console.WriteLine("Opción no válida");
            return;
    }

    if (matchingIds.Count == 0)
    {
        Console.WriteLine("No se encontraron contactos que coincidan con la búsqueda.");
        return;
    }

    Console.WriteLine($"\nContactos encontrados ({matchingIds.Count}):");
    foreach (var id in matchingIds)
    {
        Console.WriteLine($"ID: {id}, Nombre: {names[id]} {lastnames[id]}, Teléfono: {telephones[id]}");
    }
}

static void ModifyContact(
    List<int> ids,
    Dictionary<int, string> names,
    Dictionary<int, string> lastnames,
    Dictionary<int, string> addresses,
    Dictionary<int, string> telephones,
    Dictionary<int, string> emails,
    Dictionary<int, int> ages,
    Dictionary<int, bool> bestFriends)
{
    if (ids.Count == 0)
    {
        Console.WriteLine("No hay contactos para modificar.");
        return;
    }

    Console.WriteLine("Contactos disponibles:");
    foreach (var id in ids)
    {
        Console.WriteLine($"ID: {id}, Nombre: {names[id]} {lastnames[id]}");
    }

    Console.WriteLine("\nIngrese el ID del contacto a modificar:");
    if (!int.TryParse(Console.ReadLine(), out int contactId) || !ids.Contains(contactId))
    {
        Console.WriteLine("ID no válido.");
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
            names[contactId] = Console.ReadLine().Trim();
            break;
        case 2:
            Console.WriteLine("Nuevo apellido:");
            lastnames[contactId] = Console.ReadLine().Trim();
            break;
        case 3:
            Console.WriteLine("Nueva dirección:");
            addresses[contactId] = Console.ReadLine().Trim();
            break;
        case 4:
            Console.WriteLine("Nuevo teléfono:");
            telephones[contactId] = Console.ReadLine().Trim();
            break;
        case 5:
            Console.WriteLine("Nuevo email:");
            emails[contactId] = Console.ReadLine().Trim();
            break;
        case 6:
            Console.WriteLine("Nueva edad:");
            if (int.TryParse(Console.ReadLine(), out int newAge))
            {
                ages[contactId] = newAge;
            }
            break;
        case 7:
            Console.WriteLine("¿Es mejor amigo? (1. Sí, 2. No):");
            bestFriends[contactId] = Console.ReadLine().Trim() == "1";
            break;
        default:
            Console.WriteLine("Opción no válida");
            return;
    }

    Console.WriteLine("Contacto modificado exitosamente.");
}

static void DeleteContact(
    List<int> ids,
    Dictionary<int, string> names,
    Dictionary<int, string> lastnames,
    Dictionary<int, string> addresses,
    Dictionary<int, string> telephones,
    Dictionary<int, string> emails,
    Dictionary<int, int> ages,
    Dictionary<int, bool> bestFriends)
{
    if (ids.Count == 0)
    {
        Console.WriteLine("No hay contactos para eliminar.");
        return;
    }

    Console.WriteLine("Contactos disponibles:");
    foreach (var id in ids)
    {
        Console.WriteLine($"ID: {id}, Nombre: {names[id]} {lastnames[id]}");
    }

    Console.WriteLine("\nIngrese el ID del contacto a eliminar:");
    if (!int.TryParse(Console.ReadLine(), out int contactId) || !ids.Contains(contactId))
    {
        Console.WriteLine("ID no válido.");
        return;
    }

    Console.WriteLine($"¿Está seguro de eliminar a {names[contactId]} {lastnames[contactId]}? (S/N)");
    if (Console.ReadLine().Trim().ToUpper() != "S")
    {
        Console.WriteLine("Operación cancelada.");
        return;
    }

    ids.Remove(contactId);
    names.Remove(contactId);
    lastnames.Remove(contactId);
    addresses.Remove(contactId);
    telephones.Remove(contactId);
    emails.Remove(contactId);
    ages.Remove(contactId);
    bestFriends.Remove(contactId);

    Console.WriteLine("Contacto eliminado exitosamente.");
}
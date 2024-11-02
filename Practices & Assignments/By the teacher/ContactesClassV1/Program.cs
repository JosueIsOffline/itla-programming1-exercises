Console.WriteLine("Bienvenido a mi lista de Contactos");
var contactManager = new ContactManager();
bool running = true;

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
            contactManager.AddContact();
            break;
        case 2:
            contactManager.ShowContacts();
            break;
        case 3:
            contactManager.SearchContacts();
            break;
        case 4:
            contactManager.ModifyContact();
            break;
        case 5:
            contactManager.DeleteContact();
            break;
        case 6:
            running = false;
            break;
        default:
            Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú.");
            break;
    }
}
using System.Data; // Necesario para evaluar la expresión

double result = 0;
string currentExpression = "";
bool isRunning = true;
string[] operators = { "+", "-", "*", "/" };

while (isRunning)
{
    Console.Clear();
    Console.WriteLine("=== Simple Console Calculator ===");
    Console.WriteLine($"Current Expression: {currentExpression}");
    Console.WriteLine("Enter a number, choose an operation (+, -, *, /), type 'C' to clear, or type 'exit' to quit");

    // Simular el teclado numérico
    Console.WriteLine("\n[7] [8] [9]  [+]");
    Console.WriteLine("[4] [5] [6]  [-]");
    Console.WriteLine("[1] [2] [3]  [*]");
    Console.WriteLine("[0] [.] [=]  [/]");

    try
    {
        Console.WriteLine("\nYour Input: ");
        string input = Console.ReadLine();

        // Salir de la calculadora
        if (input != null && input.ToLower() == "exit")
        {
            isRunning = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Exiting calculator... Goodbye!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        // Limpiar la expresión y resultado
        else if (input != null && input.ToLower() == "c")
        {
            result = 0;
            currentExpression = "";
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Expression cleared. Press any key to continue...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }
        // Evaluar la expresión completa cuando se presiona "="
        else if (input != null && input == "=")
        {
            if (!string.IsNullOrEmpty(currentExpression))
            {
                try
                {
                    // Evaluar la expresión usando DataTable.Compute()
                    result = Convert.ToDouble(new DataTable().Compute(currentExpression, null));
                    Console.WriteLine($"Final result: {result}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    currentExpression = result.ToString(); // Actualizar la expresión con el resultado
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in evaluation: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        // Agregar número u operador a la expresión
        else if (Array.Exists(operators, op => op == input) || double.TryParse(input, out _))
        {
            currentExpression += $"{input} ";  // Acumular la entrada en la expresión
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"There was an error in the program: {ex.Message}");
    }
}

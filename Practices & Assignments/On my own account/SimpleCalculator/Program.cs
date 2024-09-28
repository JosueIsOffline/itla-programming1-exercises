double result = 0;
double currentNumber = 0;
string currentExpression = "";
string operation = "";
bool isRunning = true;
string[] operators = { "+", "-", "*", "/" };


while (isRunning)
{
    Console.Clear();
    Console.WriteLine("=== Simple Console Calculator ===");
    Console.WriteLine($"Current Expression: {currentExpression}");
    Console.WriteLine($"Current Result: {result}");
    Console.WriteLine("Enter a number, choose an operation (+, -, *, /), type 'C' to clear, or type 'exit' to quit");

    // Simular el teclado numérico
    Console.WriteLine("\n[7] [8] [9]  [+]");
    Console.WriteLine("[4] [5] [6]  [-]");
    Console.WriteLine("[1] [2] [3]  [*]");
    Console.WriteLine("[0] [.] [=]  [/]");

    try {

        Console.WriteLine("\nYour Input: ");
        string input = Console.ReadLine();

        if (input != null && input.ToLower() == "exit")
        {
            isRunning = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Existing calculator... Goodbye!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else if(Convert.ToChar(input.ToLower()) == 'c')
        {
            result = 0;
            currentExpression = "";
            operation = "";
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Expression cleared. Press any key to continue...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }
        else if (double.TryParse(input, out currentNumber))
        {
            if (string.IsNullOrEmpty(operation))
            {
                result = currentNumber;
                currentExpression = result.ToString();
                //operation = input;
                //Console.WriteLine("Enter the next number:");
                //double nextNumber = Convert.ToDouble(Console.ReadLine());
            }
            else
            {
                switch (operation)
                {
                    case "+":
                        result += currentNumber;
                        break;
                    case "-":
                        result -= currentNumber;
                        break;
                    case "*":
                        result *= currentNumber;
                        break;
                    case "/":
                        if (currentNumber != 0)
                        {
                            result /= currentNumber;
                        }
                        else
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You can't divide by Zero");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                }
                currentExpression += $" {currentNumber}";
            }
        }
        else if (Array.Exists(operators, op => op == input))
        {
            operation = input;
            if (!string.IsNullOrEmpty(currentExpression))
            {
                currentExpression += $" {operation}";
            }
        }
        else if (input == "=")
        {
            Console.WriteLine($"Final result: {result}");
            Console.WriteLine($"Press any key to continue...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"There was an error in the program: {ex.Message}");
    }
}
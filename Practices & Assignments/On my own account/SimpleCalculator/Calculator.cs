
    public class Calculator
    {
        private double result;
        private string currentExpression;
        private bool isRunning;
        private readonly string[] operators = { "+", "-", "*", "/" };

        public Calculator()
        {
            result = 0;
            currentExpression = "";
            isRunning = true;
        }

        public void Run()
        {
            while (isRunning)
            {
                DisplayCalculator();
                ProcessInput();
            }
        }

        private void DisplayCalculator()
        {
            Console.Clear();
            Console.WriteLine("=== Simple Console Calculator ===");
            Console.WriteLine($"Current Expression: {currentExpression}");
            Console.WriteLine("Enter a number, choose an operation (+, -, *, /), type 'C' to clear, or type 'exit' to quit");
            DisplayNumericKeypad();
        }

        private void DisplayNumericKeypad()
        {
            Console.WriteLine("\n[7] [8] [9]  [+]");
            Console.WriteLine("[4] [5] [6]  [-]");
            Console.WriteLine("[1] [2] [3]  [*]");
            Console.WriteLine("[0] [.] [=]  [/]");
            Console.WriteLine("\nYour Input: ");
        }

        private void ProcessInput()
        {
            try
            {
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) return;

                switch (input.ToLower())
                {
                    case "exit":
                        HandleExit();
                        break;
                    case "c":
                        HandleClear();
                        break;
                    case "=":
                        HandleEvaluation();
                        break;
                    default:
                        HandleExpression(input);
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleError($"There was an error in the program: {ex.Message}");
            }
        }

        private void HandleExit()
        {
            isRunning = false;
            Console.Clear();
            SetConsoleColor(ConsoleColor.Blue, () =>
            {
                Console.WriteLine("Exiting calculator... Goodbye!");
            });
        }

        private void HandleClear()
        {
            result = 0;
            currentExpression = "";
            SetConsoleColor(ConsoleColor.DarkMagenta, () =>
            {
                Console.WriteLine("Expression cleared. Press any key to continue...");
            });
            Console.ReadKey();
        }

        private void HandleEvaluation()
        {
            if (string.IsNullOrEmpty(currentExpression)) return;

            try
            {
                result = Convert.ToDouble(new DataTable().Compute(currentExpression, null));
                Console.WriteLine($"Final result: {result}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                currentExpression = result.ToString();
            }
            catch (Exception ex)
            {
                HandleError($"Error in evaluation: {ex.Message}");
            }
        }

        private void HandleExpression(string input)
        {
            if (Array.Exists(operators, op => op == input) || double.TryParse(input, out _))
            {
                currentExpression += $"{input} ";
            }
            else
            {
                HandleError("Invalid input. Please try again");
            }
        }

        private void HandleError(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void SetConsoleColor(ConsoleColor color, Action action)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action();
            Console.ForegroundColor = originalColor;
        }
    }
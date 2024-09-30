// Declaración y asignación de variables de diferentes tipos.
int integerNumber = 10;
float decimalNumber = 20.5f;
string texto = "Hola Mundo";
bool esVerdadero = true;

// Imprimir valores de las variables.
Console.WriteLine("Valor de numerointeger: " + integerNumber);
Console.WriteLine("Valor de numeroDecimal: " + decimalNumber);
Console.WriteLine("Valor de texto: " + texto);
Console.WriteLine("Valor de esVerdadero: " + esVerdadero);

// Declarar una constante
const int numeroConstante = 100;
Console.WriteLine("Valor de la constante: " + numeroConstante);

//numeroConstante = 200; // Esto dará un error de compilación porque no se puede cambiar el valor de una constante.

// Operaciones con un integer.
int integer = 50;
Console.WriteLine("Valor inicial de integer: " + integer);

// Incrementar
integer++;
Console.WriteLine("Después de incrementar: " + integer);

// Decrementar
integer--;
Console.WriteLine("Después de decrementar: " + integer);

// Operaciones
integer += 20; // Sumar 20
Console.WriteLine("Después de sumar 20: " + integer);

// Declarar un float
float myFloat = 10152466.25f;
Console.WriteLine("Valor de miFloat: " + myFloat);

// Declarar un byte
byte myByte = (byte)(5 + myFloat); // Casting necesario ya que float no cabe en byte sin truncar.
Console.WriteLine("Valor de miByte (5 + miFloat): " + myByte); // Esto podría dar un valor truncado.

/* 
A continuación imprimimos la fecha y hora del sistema usando DateTime.
Esto nos da la fecha y la hora actual del sistema al ejecutar el código.
*/
DateTime currentDateTime = DateTime.Now;
Console.WriteLine("Fecha y hora actual del sistema: " + currentDateTime);
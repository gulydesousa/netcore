using System;
using static System.Console;

string name = typeof(Program).Namespace ?? "None!";

WriteLine($"Namespace:  {name}");

WriteLine("Hello, C#!");

object height_ = 1.88;
object name_ = "Amir";
WriteLine($"{name_} is {height_} meters tall");

int length = ((string)name_).Length;
WriteLine($"{name_} is {length} characters long");


dynamic something;
something = new[] { 6, 7, 4 };

something = 12;
something = "Amed";

WriteLine($"The length of something {something.Length}");
WriteLine($"The type of something {something.GetType()}");


WriteLine($"default(int) = {default(int)}");
WriteLine($"default(DateTime) = {default(DateTime)}");
WriteLine($"default(string) = {(default(string) == null ? "null" : default(string))}");
WriteLine($"default(bool) = {default(bool)}");

int number = 10;
WriteLine($"number = {number}");

number = default;
WriteLine($"number = {number}");

int apples = 12;
decimal price = 1.20M;

WriteLine("{0} apples cost {1:C} cents.", apples, price * apples);

WriteLine($"{apples} apples cost {price * apples:C} cents.");

string formatted = string.Format("{0} apples cost {1:C} cents.", apples, price * apples);

WriteLine(formatted);

//Primera columna alineada a la izquierda, la segunda a la derecha
WriteLine("{0,-10} {1,6}", "Fruta", "Count");
WriteLine("{0,-10} {1,6:N0}", nameof(apples), apples);

//Leer desde la consola:
WriteLine("Press any combination:");
ConsoleKeyInfo key = ReadKey();
WriteLine();
WriteLine("Key: {0}, Char: {1}, Modifiers: {2}", key.Key, key.KeyChar, key.Modifiers);

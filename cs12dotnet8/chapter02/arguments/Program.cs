
if (args.Length < 3)
{
    WriteLine("You must specify two colors and cursor size, e.g.");
    WriteLine("dotnet run red yellow 50");
    return;
}

int cursorSize = 0;
if (!int.TryParse(args[2], out cursorSize))
{
    Console.WriteLine("Invalid cursor size. Please provide a valid integer value.");
    return;
}

if (cursorSize < 1 || cursorSize > 100)
{
    WriteLine("The cursor size must be between 1 and 100.");
    return;
}
else
{
    try
    {
        CursorSize = int.Parse(args[2]);
    }
    catch
    {
        WriteLine("The current platform does not support changing the size of the cursor.");
    }
}

ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[0], true);
BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[1], true);

Console.WriteLine("There are  {0} arguments.", args.Length);

foreach (string arg in args)
{
    Console.WriteLine(arg);
}

ReadLine();

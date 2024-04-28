#region Exploring unary operators
using StackExchange.Profiling;

int a = 3;
int b = a++;
Console.WriteLine($"a is: {a}, b is: {b}");


int c = 3;
int d = ++c;
Console.WriteLine($"c is: {c}, d is: {d}");

#endregion


var animals = new Animal?[]
{
new Cat { Name = "Fluffy", Born = new DateTime(2010, 1, 1), Legs = 4 , IsDomestic=true},
new Cat { Name = "Whiskers", Born = new DateTime(2012, 3, 3)},
new Spider { Name = "Charlotte", Born = new DateTime(2015, 5, 5), IsPoisonous=true },
new Spider { Name = "Boris", Born = new DateTime(2018, 7, 7)},
null
};

string mensaje;

#region LOG EXAMPLE
//Path.Combine combina dos rutas en una sola
string logpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log.txt");
//TextWriterTraceListener es un listener que escribe en un archivo de texto
TextWriterTraceListener textWriterTraceListener = new(File.CreateText(logpath));
//Agrego el listener a la lista de listeners
Trace.Listeners.Add(textWriterTraceListener);
//Flush quiere decir que se vacia el buffer de salida despues de cada operacion de escritura
Trace.AutoFlush = true; //Flushes the output buffer after each write operation

#if DEBUG
Trace.WriteLine($"This is a debug message {DateTime.Now}");
#endif

Debug.WriteLine("Im a debug message");
Trace.WriteLine($"Im a trace message {DateTime.Now}");
#endregion


#region switch animals

foreach (Animal? animal in animals)
{
    /* The `switch (animal)` statement is used to evaluate the value of the `animal` variable and then
    execute the corresponding case based on the value of `animal`. Each `case` statement checks if
    the `animal` matches a specific pattern or condition, and if it does, the corresponding block of
    code is executed. If none of the cases match, the `default` case is executed. */
    switch (animal)
    {
        case Cat fourLeggedCat when fourLeggedCat.Legs == 4:
            mensaje = ($"{fourLeggedCat.Name} is a cat with 4 legs");
            break;
        case Cat wildCat when wildCat.IsDomestic == false:
            mensaje = ($"{wildCat.Name} is a wild cat");
            break;
        case Cat cat:
            mensaje = ($"{cat.Name} is a cat");
            break;
        default:
            mensaje = ($"{animal.Name} is an {animal.GetType().Name}");
            break;
        case Spider spider when spider.IsPoisonous:
            mensaje = ($"{spider.Name} is a poisonous spider");
            break;
        case null:
            mensaje = "No animal";
            break;

    }
    WriteLine(mensaje);
}

#endregion

#region switch expressions
//Explicacion de esta sintaxis
//Este switch es una expresion, por lo que no se necesita un break en cada case
//El switch se evalua y se asigna a la variable mensaje

foreach (Animal? animal in animals)
{
    mensaje = animal switch
    {
        //Cuando el animal es un gato con 4 patas
        Cat fourlegged when fourlegged.Legs == 4 => $"{fourlegged.Name} is a cat with 4 legs",
        //Cuando el animal es un gato salvaje
        Cat wildCat when wildCat.IsDomestic == false => $"{wildCat.Name} is a wild cat",
        //Cuando el animal es un gato
        Cat cat => $"{cat.Name} is a cat",
        //Cuando el animal es una araña venenosa
        Spider spider when spider.IsPoisonous => $"{spider.Name} is a poisonous spider",
        //Cuando el animal es nulo
        null => "No animal",
        //Cuando no cumple con ningun caso
        //_ es para el default descarte
        _ => $"{animal.Name} is an {animal.GetType().Name}"
    };
    WriteLine($"- {mensaje}");

}

#endregion

//Quiero parar el while cuando se presiona una tecla
do
{
    WriteLine("HotRealoading");
    await Task.Delay(2000);
} while (!Console.KeyAvailable);


 var profiler = MiniProfiler.StartNew("My Profiler Name");
using (profiler.Step("Main Work"))
{

    string settingsFile = "appsettings.json";
    string settingsPath = Path.Combine(Directory.GetCurrentDirectory(), settingsFile);
    WriteLine($"Processing:{settingsPath}");
    WriteLine($"--{settingsFile} contents --");
    WriteLine(File.ReadAllText(settingsPath));
    WriteLine("----");

    ConfigurationBuilder builder = new();
    builder.SetBasePath(Directory.GetCurrentDirectory());

    builder.AddJsonFile(settingsFile, optional: false, reloadOnChange: true);
    IConfigurationRoot configuration = builder.Build();

    TraceSwitch ts = new TraceSwitch(displayName: "PacktSwitch", description: "This switch is set via JSON config");

    configuration.GetSection("PacktSwitch").Bind(ts);

    WriteLine($"TraceSwitch Value: {ts.Value}");
    WriteLine($"TraceSwitch Level: {ts.Level}");

    Trace.WriteLineIf(ts.TraceError, "TraceError");
    Trace.WriteLineIf(ts.TraceWarning, "TraceWarning");
    Trace.WriteLineIf(ts.TraceVerbose, "TraceVerbose");
    Trace.WriteLineIf(ts.TraceInfo, "TraceInfo");

    int valor = 1;
    LogSourceDetails(valor == 0);



}

Console.WriteLine(profiler.RenderPlainText());
Debug.Close();
Trace.Close();

[Benchmark]
static void LogSourceDetails(bool condition,
    [CallerFilePath] string? file = null,
    [CallerLineNumber] int line = 0,
    [CallerMemberName] string? member = null,
    [CallerArgumentExpressionAttribute(nameof(condition))] string expression = "")
{
    Trace.WriteLine($"File: {file}, Line: {line}, Member: {member}, Expression: {expression}");
    if (condition)
    {
        WriteLine("Condition is true");
    }
    else
    {
        WriteLine("Condition is false");
    }
}


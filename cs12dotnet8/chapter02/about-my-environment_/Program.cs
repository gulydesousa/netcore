namespace AboutMyEnviroment_;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Environment.CurrentDirectory);
        Console.WriteLine(Environment.OSVersion.VersionString);
        Console.WriteLine("Namespace:{0}", typeof(Program).Namespace ?? "None!");

        Console.WriteLine($"Un millon: {1_000_000} ");
        Console.WriteLine($"Un millon: {10_000_00} ");

        Console.WriteLine($"int bytes: {sizeof(int)}, min value: {int.MinValue}, max value: {int.MaxValue}");
        Console.WriteLine($"long bytes: {sizeof(long)}, min value: {long.MinValue}, max value: {long.MaxValue}");
        Console.WriteLine($"double bytes: {sizeof(double)}, min value: {double.MinValue}, max value: {double.MaxValue}");
        Console.WriteLine($"decimal bytes: {sizeof(decimal)}, min value: {decimal.MinValue}, max value: {decimal.MaxValue}");

        decimal a = 0.1M;
        decimal b = 0.2M;

        Console.WriteLine($"decimal a: {a}, decimal b: {b}, a + b: {a + b}, equal: {(a + b == 0.3M ? "yes" : "no")}");

        double a0 = 0.1;
        double b0 = 0.2;

        Console.WriteLine($"double a: {a0}, double b: {b0}, a + b: {a0 + b0}, equal: {(a0 + b0 == 0.3 ? "yes" : "no")}");
        //Un bloque unsafe permite el uso de punteros
        //Para usar Half e Int128 tenemos que usar unsafe porque no son tipos de datos primitivos
        //usafe en estos casos es para permitir el uso de punteros y no para hacer el código inseguro
        unsafe
        {
            Console.WriteLine($"Half bytes: {sizeof(Half)}, Half min value: {Half.MinValue}, Half max value: {Half.MaxValue}");

            Console.WriteLine($"Int128 bytes: {sizeof(Int128)}, Int128 min value: {Int128.MinValue}, Int128 max value: {Int128.MaxValue}");
        }

    }
}

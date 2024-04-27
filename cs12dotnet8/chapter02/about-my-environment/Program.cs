using System.Reflection;
using System.Text;


namespace AboutMyEnviroment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Data.DataSet ds = new();
            HttpClient client = new();

            double heightInMeters = 1.88;

            Console.WriteLine(Env.CurrentDirectory);
            Console.WriteLine(Env.OSVersion.VersionString);
            Console.WriteLine("Namespace:{0}", typeof(Program).Namespace ?? "None!");

            Console.WriteLine($"The variable {nameof(heightInMeters)} has the value {heightInMeters}");
            Console.WriteLine(new string('-', 100));

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(char.ConvertFromUtf32(0x1F600));

            Console.WriteLine(RawInterpolatedJsonLiteral());

            Console.WriteLine($"Computer name: {Env.MachineName} says \"NO\"");

            Assembly? myApp = Assembly.GetEntryAssembly();
            if (myApp == null) return;

            foreach (AssemblyName assembyName in myApp.GetReferencedAssemblies())
            {
                Assembly a = Assembly.Load(assembyName);
                int methodCount = 0;
                foreach (Type t in a.GetTypes())
                {
                    methodCount += t.GetMethods().Length;
                }

                Console.WriteLine("{0:N0} types with {1:N0} methods in {2} assembly.",
                    a.DefinedTypes.Count(), methodCount, assembyName.Name);
            }
        }

        private static string RawInterpolatedJsonLiteral()
        {
            string name = "Jon Doe";

            string json = $$"""
				{
					"NAME": "{{name}}",
					"AGE": "41",
					"CALCULATION" = "{{10 + 5}}"
				}
				""";

            return json;
        }


    }
}

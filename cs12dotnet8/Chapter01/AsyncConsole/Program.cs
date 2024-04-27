using System.Xml;

HttpClient client = new();

HttpResponseMessage response = await client.GetAsync("https://www.apple.com");

WriteLine("Apple´s homepage has {0:N0} bytes", response.Content.Headers.ContentLength);

Console.WriteLine($"C# Language Version: {System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion()}");


// Crear una instancia de XmlDocument
XmlDocument xmlDoc = new XmlDocument();

// Puedes cargar un documento XML existente o crear uno nuevo
xmlDoc.LoadXml("<root><element>Contenido</element></root>");

// Realizar operaciones con el XmlDocument según tus necesidades

// Imprimir el contenido del documento XML
Console.WriteLine(xmlDoc.OuterXml);


string texto = "Alinear a la derecha";

// Alinear a la derecha agregando espacios a la izquierda
string textoAlineado = texto.PadLeft(30);

// Imprimir el resultado
Console.WriteLine(textoAlineado);
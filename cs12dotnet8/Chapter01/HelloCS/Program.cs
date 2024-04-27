string name = typeof(Program).Namespace ?? "None!";
Console.WriteLine($"Namespace:  {name}");

//#error version

//La línea "#error version" es una directiva del preprocesador que genera un error de compilación con el mensaje "version".
//Esta directiva se utiliza para detener intencionalmente el proceso de compilación
//e indicar que una versión específica o una condición no son compatibles.

//En este caso, parece que el desarrollador quiere indicar que hay un problema con la versión del código
//o algún requisito específico que debe abordarse antes de continuar.

//El código después de la directiva "#error version" no se ejecutará porque se detiene el proceso de compilación.
//Es común utilizar directivas del preprocesador como esta para proporcionar compilación condicional
//o indicar que ciertas partes del código requieren atención o modificación.

//En este caso, la directiva sirve como un marcador o recordatorio para abordar el problema de la versión antes de ejecutar el código.
//Una vez que se resuelva el problema de la versión, el código se puede descomentar y ejecutar correctamente.


Console.WriteLine("Hello, C#!");


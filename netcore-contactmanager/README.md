# Netcore Contacts Manager Solution
> [!NOTE]
> Este proyecto forma parte del curso de Udemy Asp.Net Core 8 (.NET 8) | True Ultimate Guide, y es un recurso excelente para comprender mejor el desarrollo de aplicaciones con Asp.Net Core 8.
> - [x] [Asp.Net Core 8 (.NET 8) | True Ultimate Guide](https://neoris.udemy.com/course/asp-net-core-true-ultimate-guide-real-project/learn/lecture/34726404?start=1#overview)

## Finalidad del Repositorio

La finalidad principal de este repositorio es servir como plataforma para interactuar con **Copilot Enterprise** y **Advanced Security** en un entorno de proyectos reales.

[Video en Workplace](https://neoris.workplace.com/work/landing/input/?next=https%253A%252F%252Fneoris.workplace.com%252Fnd%252F%253Fgroups%25252F1298399547717449%25252Fpermalink%25252F1408916869999049%25252F%2526work_newsfeed_token%253DS%25253A_I100023025081469%25253A1408916869999049%25253A949560486500082%2526aref%253D1706661365402812%2526medium%253Demail%2526mid%253D61032c6d9c9eaG5af95916f088G61033106fccbcG1d72%2526n_m%253Dguly.desousa%252540neoris.com%2526lloc%253Dview_on_facebook_btn%2526rms%253Dv2%2526irms%253Dtrue%2526d)

**Copilot Enterprise** ofrece una experiencia mejorada en el desarrollo de software al proporcionar sugerencias inteligentes y comentarios útiles durante el proceso de revisión de código. Esto no solo optimiza la escritura de código, sino que también mejora la calidad y eficiencia del desarrollo.

Por otro lado, **Advanced Security** juega un papel fundamental en la identificación proactiva y mitigación de vulnerabilidades en el software. Esto garantiza que las aplicaciones desarrolladas no solo sean eficientes, sino también robustas y seguras, lo que es crucial en un entorno donde la seguridad de los datos y la protección contra amenazas son prioridades importantes.

Este repositorio se utiliza como un recurso práctico para demostrar cómo estas herramientas pueden ser integradas y aprovechadas de manera efectiva en el desarrollo de aplicaciones complejas y seguras. Aunque el código y la solución en sí misma son parte del repositorio, la verdadera prioridad es destacar la utilidad y eficacia de las herramientas mencionadas en un contexto práctico y aplicable.

## Cómo empezar
Para seguir la demostración de este proyecto, necesitarás realizar los siguientes pasos:

### Acceso a GitHub Empresarial de Neoris
Asegúrate de tener acceso al **GitHub Empresarial** de Neoris. Si aún no tienes acceso, solicítalo a través de los canales correspondientes.

### Descargar y configurar el proyecto
- Una vez que tengas acceso, puedes descargar el proyecto `Netcore Contact Manager` para empezar a trabajar con él. 

- Para obtener información detallada sobre la configuración e instalación del proyecto, consulta la sección [Instalación](#instalación) más abajo en este documento.

### Establecer el repositorio: Opciones y Consideraciones
**Creación de un Repositorio en la Organización**<br>
- La demostración incluye una guia paso a paso para la creación de un nuevo repositorio dentro de una organización en GitHub, enfocado especialmente para este proyecto. Sigue estos pasos para configurar tu propio repositorio en el entorno de Neoris.


**Creación de un Repositorio Personal**<br>
- Como alternativa, tienes la posibilidad de crear tu repositorio bajo tu usuario **persona**. 
- Es importante considerar que algunas funcionalidades específicas, tales como el **code scanning** (análisis de código), se encuentran disponibles únicamente en los repositorios de organizaciones. 
- Para los repositorios personales, la función de **Advanced Security** no está activada por defecto. 
- Para habilitar **Advanced Security** dirígete a la sección de configuración de tu repositorio, bajo **"Configuración / Seguridad del código y análisis"**.
<br>
<br>

## Prompts usados en la demostracion

- *¿Para qué sirve este repositorio?*
- *Resumen del Repositorio*
- *¿Qué tipo de base de datos usa este proyecto?*
- *¿Este proyecto utiliza alguna librería o paquete que no es de microsoft?*
- *¿Cómo está organizado el código? Explica la arquitectura del proyecto.*
- *¿Qué frameworks web se utilizan en este proyecto?*
- *¿Qué servicios utiliza esta aplicación?*
- *Genera un workflow para que se compile el proyecto y se ejecuten las pruebas unitarias.*
- *¿Dónde está definida la funcionalidad para AddPerson?*
- *Puedes darme tips para optimizar la seguridad esta aplicación?*
- *Aparte del uso de clases, que otros elementos de programación c# puedo usar para representar mi modelo?*
- *En que consiste el docset de GitHub?*
- *Me falta alguna prueba unitaria para personsAdderService?*
- *Me puedes proponer las pruebas para estos casos que faltan?*
- *Como puedo probar este método?*
- *Explica las diferencias en este Pull Request*
- *Puedes darme un resumen del PR [Descricion/ID Pull Request]*

## Limitaciones de Copilot Enterprise
Copilot Enterprise se encuentra en una fase beta con avances continuos. Algunas de las limitaciones actuales incluyen:

- La funcionalidad para establecer un conjunto de documentos como contexto de una conversación está disponible solo para un número limitado de clientes en la beta. La creación de conjuntos de documentos se habilitará para todos los clientes de la beta en breve.

- Copilot emplea búsqueda semántica de código para mejorar la calidad y precisión de las respuestas. Actualmente, existe un límite suave de 50 repositorios por cliente, pero estamos trabajando para incrementar este límite próximamente.

- Según la documentación, se reconoce que pueden surgir inexactitudes y cierta lentitud en los commits que incluyen más de 30 archivos, con la posibilidad de que algunos archivos no se describan automáticamente.


## Instalación
Estos son los pasos para ejecutar `Netcore Contact Manager` en tu máquina local para propósitos de desarrollo y pruebas.

1. Asegúrate de tener lo siguiente instalado en tu máquina de desarrollo:

- .NET Core 8
- SQL Server

2. Clona el repositorio

```
git clone https://github.com/DDC-NEORIS/netcore-contactmanager.git
```

3. Navega a la carpeta del proyecto
```
cd netcore-contactmanager
```

4. Restaura los paquetes NuGet
```
dotnet restore
```

5. Cambia la cadena de conexión en **ContactsManager.UI** `appsettings.json` para que apunte a tu base de datos SQL Server:
   
```json

"AllowedHosts": "*",
  "ConnectionStrings": {
   //Conexión a la base de datos
   "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testContactsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
 }

```

6. Instala las herramientas necesarias para ejecutar migraciones::
   - dotnet tool install --global dotnet-ef
   - dotnet tool install --global dotnet-ef --version 8.0.0


7 Desde el directorio **ContactsManager.UI** ejecuta el comando:
```
dotnet ef database update
```
| <!-- -->      | <!-- -->      |
|:-------------:|:-------------:|
| Esto creará la base de datos usada por la solución | ![image](https://github.com/DDC-NEORIS/netcore-contactmanager/assets/139492750/9f3c6807-abbd-4af0-982f-1021a3735400) |
  

8. Ejecuta el proyecto
```
dotnet run
```
## Uso

`Netcore Contact Manager` es una aplicación web de gestión de contactos. Puedes agregar, editar, eliminar y ver contactos.

## Contribuir

Las contribuciones son siempre bienvenidas. Por favor lee las [guías de contribución](CONTRIBUTING.md) para obtener información sobre cómo contribuir al proyecto.

## Licencia

Este proyecto está bajo la licencia MIT - ve el archivo [LICENSE](LICENSE.md) para más detalles.


## Fecha de Actualización

Última actualización: 28 de febrero de 2024
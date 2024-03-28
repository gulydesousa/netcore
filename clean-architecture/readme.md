# Clean Architecture

1. Construir la solución: 
```
dotnet build C:\Gdesousa\netcore\CleanArchitecture\src\src.sln
```

2. Mostrar el resultado del build

```
1. dotnet build --verbosity normal
2. dotnet build --verbosity detailed
```

## Comandos usados para crear la solución

1. Version de .NET
```
dotnet --version
```

2. Creamos en global.json apuntando a esa version de .NET
```
dotnet new globaljson --sdk-version 8.0.100
```

3. Creamos la solución
```
dotnet new sln --name CleanArchitecture
```

## Clean Architecture: Domain

1. Creamos un proyecto de librería 
```
dotnet new classlib -o src/CleanArchitecture/CleanArchitecture.Domain
```

2. Vincularlo a la solución
```
dotnet sln add CleanArchitecture/CleanArchitecture.Domain/CleanArchitecture.Domain.csproj
```

3. Para poder disparar un evento cuando cambia el estado de una entidad :  **mediatR.Contracts**: 
```
dotnet add package MediatR.Contracts
```

## Clean Architecture: Application

1. Creamos un proyecto de librería 
```
 dotnet new classlib -o src/CleanArchitecture/CleanArchitecture.Application
 ```

 2. Vincularlo a la solución
```
 dotnet sln add CleanArchitecture/CleanArchitecture.Application/CleanArchitecture.Application.csproj
```

3. Application depende de Domain, agregamos la referencia
```
dotnet add CleanArchitecture/CleanArchitecture.Application/CleanArchitecture.Application.csproj reference CleanArchitecture/CleanArchitecture.Domain/CleanArchitecture.Domain.csproj

```
4. Para poder disparar un evento cuando cambia el estado de una entidad :  **mediatR.Contracts**: 
```
src\CleanArchitecture\CleanArchitecture.Application> dotnet add package MediatR.Contracts
```

5. Para poder usar `Microsoft.Extensions.DependencyInjection`

```
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
````

6. Instalar `Dapper` para la parte de BBDD desde el directorio `CleanArchitecture.Application`
````
$ cd src/CleanArchitecture/CleanArchitecture.Application
$ dotnet add package Dapper --version 2.1.35 
````

7. Instalar en `CleanArchitecture.Application` el paquete de logs de Microsoft `Microsoft.Extensions.Logging.Abstractions`
````
$ dotnet add package Microsoft.Extensions.Logging.Abstractions
````

8. Instalar en `CleanArchitecture.Application` el paquete de fluentValidation de Microsoft `FluentValidation.DependencyInjectionExtensions`
````
$ dotnet add package FluentValidation.DependencyInjectionExtensions
````

## Clean Architecture: Infrastructure

1. Creamos un proyecto de librería 
```
 dotnet new classlib -o src/CleanArchitecture/CleanArchitecture.Infrastructure
 ```

  2. Vincularlo a la solución
```
 dotnet sln add CleanArchitecture/CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj
```

3. Infrastructure depende de Application, agregamos la referencia
```
dotnet add CleanArchitecture/CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj reference CleanArchitecture/CleanArchitecture.Application/CleanArchitecture.Application.csproj
```


3. Instalar el paquete que nos permite leer el json.configuration `Microsoft.Extensions.Configuration.Abstractions`
```
dotnet add package Microsoft.Extensions.Configuration.Abstractions
```

4. Instalar el paquete del **Entity Framework**
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package EFCore.NamingConventions
```

4. Instalar el paquete para la autenticación
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```
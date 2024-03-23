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

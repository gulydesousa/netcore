# 👩‍💻 https://github.com/markjprice/cs12dotnet8

### 💾 https://github.com/markjprice/cs12dotnet8/tree/main/code/PracticalApps

### 📚 https://github.com/markjprice/cs12dotnet8/blob/main/docs/B19586_Appendix.pdf

## 🛢 Database First
Se crea la base de datos y a partir de de ella se genera el modelo de datos.

## 📘 Capitulo 12
https://github.com/markjprice/cs12dotnet8/blob/main/docs/sql-server/README.md#chapter-12---introducing-web-development-using-aspnet-core

```pws
dotnet ef dbcontext scaffold "Server=GULYDESOUSA;Database=Northwind;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --namespace Northwind.EntityModels --data-annotations
```
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Vehiculos;
public static class VehiculoErrors
{
    //Esta clase define los errores relacionados con los vehículos.
    //Los objetos de tipo Error son inmutables, por lo que se pueden reutilizar en toda la aplicación.
    //Se usa la convención de que los errores se definen como propiedades estáticas en una clase estática.
    
    public static Error NotFound = new Error(
        "Vehiculo.NotFound",
        "No existe el vehículo buscado por este id");

}

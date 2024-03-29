using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Vehiculos;

//Principio de responsabilidad única
//Sealed: porque no queremos que esta clase sea heredada por otra
public sealed class Vehiculo : Entity
{
    private Vehiculo() { }
    public Vin? Vin { get; private set; }
    public Modelo? Modelo { get; private set; }
    public Direccion? Direccion { get; private set; }
    public Moneda? Precio { get; private set; }
    public Moneda? Mantenimiento { get; private set; }

    //internal set: solo se puede modificar desde el mismo ensamblado
    public DateTime FechaUltimoAlquiler { get; internal set; }
    public List<Accesorio> Accesorios { get; private set; } = new List<Accesorio>();

    public Vehiculo(Guid id, Vin vin, Modelo modelo, Direccion direccion
                  , Moneda precio, Moneda mantenimiento
                  , DateTime fechaUltimoAlquiler): base(id)
    {
        this.Vin = vin;
        this.Modelo = modelo;
        this.Direccion = direccion;
        this.Precio = precio;
        this.Mantenimiento = mantenimiento;
        this.FechaUltimoAlquiler = fechaUltimoAlquiler;
    }
}   
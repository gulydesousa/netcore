using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Alquileres;

public class PrecioService
{
    public PrecioDetalle CalcularPrecio(Vehiculo vehiculo, DateRange periodo)
    {
        var tipoMoneda = vehiculo.Precio!.TipoMoneda;
        var precioPorPeriodo = new Moneda(periodo.CantidadDias * vehiculo.Precio.Monto, tipoMoneda);

        decimal porcentageCharge = 0;

        foreach (var accesorio in vehiculo.Accesorios)
        {
            //Switch expression
            //Se utiliza para evaluar una expresión y devolver un valor en función de una coincidencia            
            porcentageCharge += accesorio switch
            {
                Accesorio.AppleCar or Accesorio.AndroidCar => 0.05m,
                Accesorio.AireAcondicionado => 0.1m,
                Accesorio.Mapas => 0.01m,
                _ => 0
            };
        }
        var accesoriosCharges = Moneda.Zero();
        if (porcentageCharge > 0)
        {
            accesoriosCharges = new Moneda(precioPorPeriodo.Monto * porcentageCharge, tipoMoneda);
        }

        var precioTotal = Moneda.Zero();
        precioTotal += precioPorPeriodo;
        if (!vehiculo!.Mantenimiento!.IsZero())
        {
            precioTotal += vehiculo.Mantenimiento;
        }

        precioTotal += accesoriosCharges;
        return new PrecioDetalle(precioPorPeriodo, vehiculo.Mantenimiento, accesoriosCharges, precioTotal);
    }
}

using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres
{
    public static class AlquilerErrors
    {
        public static Error NotFound = new Error(
          "Alquiler.NotFound"
        , "No se encontró el alquiler");

        //Representa cuando queremos reservar un vehiculo que ya está reservado
        public static Error Overlap = new Error(
          "Alquiler.Overlap"
          , "El vehiculo está siendo usado por al menos otro cliente en las mismas fechas");


          public static Error NotReserved = new Error(    
            "Alquiler.NotReserved"
            , "El alquiler no está reservado");

        public static Error NotConfirmed = new Error(
            "Alquiler.NotConfirmed"
            , "El alquiler no está confirmado");

        public static Error AlreadyStarted = new Error(
            "Alquiler.AlreadyStarted"
            , "El alquiler ya ha comenzado");
    }
}
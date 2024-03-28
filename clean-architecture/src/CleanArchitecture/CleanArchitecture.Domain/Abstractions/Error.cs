namespace CleanArchitecture.Domain.Abstractions;

//Que diferencia hay entre una clase y un record?
//Un record es inmutable, es decir, no se puede modificar una vez creado
//Una clase es mutable, es decir, se puede modificar una vez creado
//Cuando elegir un record y cuando una clase?
//Si necesitas un objeto inmutable, es decir, que no se pueda modificar una vez creado, entonces usa un record
//Si necesitas un objeto mutable, es decir, que se pueda modificar una vez creado, entonces usa una clase
//En este caso, como Error es un objeto inmutable, se usa un record
//Porque dices que es inmutable?
//Porque no tiene métodos que modifiquen sus propiedades, es decir, no tiene métodos que modifiquen su estado
//Por lo tanto, una vez creado, no se puede modificar
public record Error(string Code, string Description)
{
    //Estos son errores comunes que se pueden usar en cualquier parte del código
    public static Error None = new Error(string.Empty, string.Empty);

    //Como llamo a este error?
    //Error.NotFound
    public static Error NullValue = new Error("Error.NullValue", "Un valor NULL fue encontrado");
}
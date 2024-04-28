#region Exploring unary operators
int a = 3;
int b = a++;
Console.WriteLine($"a is: {a}, b is: {b}");


int c = 3;
int d = ++c;
Console.WriteLine($"c is: {c}, d is: {d}");

#endregion


var animals = new Animal?[]
{
new Cat { Name = "Fluffy", Born = new DateTime(2010, 1, 1), Legs = 4 , IsDomestic=true},
new Cat { Name = "Whiskers", Born = new DateTime(2012, 3, 3)},
new Spider { Name = "Charlotte", Born = new DateTime(2015, 5, 5), IsPoisonous=true },
new Spider { Name = "Boris", Born = new DateTime(2018, 7, 7)},
null
};

string mensaje;


#region switch animals

foreach (Animal? animal in animals)
{
    switch (animal)
    {
        case Cat fourLeggedCat when fourLeggedCat.Legs == 4:
            mensaje = ($"{fourLeggedCat.Name} is a cat with 4 legs");
            break;
        case Cat wildCat when wildCat.IsDomestic == false:
            mensaje = ($"{wildCat.Name} is a wild cat");
            break;
        case Cat cat:
            mensaje = ($"{cat.Name} is a cat");
            break;
        default:
            mensaje = ($"{animal.Name} is an {animal.GetType().Name}");
            break;
        case Spider spider when spider.IsPoisonous:
            mensaje = ($"{spider.Name} is a poisonous spider");
            break;
        case null:
            mensaje = "No animal";
            break;

    }
    WriteLine(mensaje);
}

#endregion

#region switch expressions
//Explicacion de esta sintaxis
//Este switch es una expresion, por lo que no se necesita un break en cada case
//El switch se evalua y se asigna a la variable mensaje

foreach (Animal? animal in animals)
{
    mensaje = animal switch
    {
        //Cuando el animal es un gato con 4 patas
        Cat fourlegged when fourlegged.Legs == 4 => $"{fourlegged.Name} is a cat with 4 legs",
        //Cuando el animal es un gato salvaje
        Cat wildCat when wildCat.IsDomestic == false => $"{wildCat.Name} is a wild cat",
        //Cuando el animal es un gato
        Cat cat => $"{cat.Name} is a cat",
        //Cuando el animal es una araña venenosa
        Spider spider when spider.IsPoisonous => $"{spider.Name} is a poisonous spider",
        //Cuando el animal es nulo
        null => "No animal",
        //Cuando no cumple con ningun caso
        //_ es para el default descarte
        _ => $"{animal.Name} is an {animal.GetType().Name}"
    };
    WriteLine($"- {mensaje}");

}

#endregion
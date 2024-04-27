using System;

public class Animal
{

    public string Name { get; set; }
    public DateTime Born { get; set; }
    public int Legs { get; set; }

}

public class Cat: Animal
{
    public bool  IsDomestic { get; set; }
}

public class Spider: Animal
{
    public bool IsPoisonous { get; set; }
}


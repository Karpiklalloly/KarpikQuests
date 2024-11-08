namespace HowTo._5._Custom_Quest;

public class Item
{
    public string Name { get; set; }
}

public class Apple : Item
{
    public Apple()
    {
        Name = "Apple";
    }
}

public class Sword : Item
{
    public Sword()
    {
        Name = "Sword";
    }
}
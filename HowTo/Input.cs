namespace HowTo;

public static class Input
{
    public static int Int()
    {
        int value;
        while (!int.TryParse(Console.ReadLine(), out value)) { }
        return value;
    }

    public static bool Boolean()
    {
        bool value;
        while (!bool.TryParse(Console.ReadLine(), out value)) { }
        return value;
    }

    public static bool Boolean(string t, string f)
    {
        while (true)
        {
            var str = Console.ReadLine();
            if (str == t) return true;
            if (str == f) return false;
        }
    }

    public static string String()
    {
        return Console.ReadLine()!;
    }
}
namespace MoveReplace;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class SerializeThis : Attribute
{
    public string Name;
    public bool IsReference = false;

    public SerializeThis(string name)
    {
        Name = name;
    }
}
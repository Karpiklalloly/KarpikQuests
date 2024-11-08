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

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class DoNotSerializeThis : Attribute
{
    
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class Property : Attribute
{
    
}
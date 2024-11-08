using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace.Changers;

public class NewtonsoftJsonChanger : IChanger
{
    public SyntaxNode OnSerializeThis(SerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        var attributeName = "JsonProperty";
        var name = "PropertyName";
        switch (member)
        {
            case FieldDeclarationSyntax field:
                field = root.Find(field);
                if (!field.HasAttribute(attributeName))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref field, attributeName,
                        new AttributeAdder.AttributeParam(name, attr.Name));
                }
                break;
            case PropertyDeclarationSyntax property:
                property = root.Find(property);
                if (!property.HasAttribute(attributeName))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref property, attributeName,
                        new AttributeAdder.AttributeParam(name, attr.Name));
                }
                break;
        }

        return root;
    }

    public SyntaxNode OnDoNotSerializeThis(DoNotSerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        var name = "JsonIgnore";
        switch (member)
        {
            case FieldDeclarationSyntax field:
            {
                field = root.Find(field);
                if (!field.HasAttribute(name))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref field, name);
                }
        
                break;
            }
            case PropertyDeclarationSyntax property:
            {
                property = root.Find(property);
                if (!property.HasAttribute(name))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref property, name);
                }
        
                break;
            }
        }

        return root;
    }

    public SyntaxNode OnProperty(Property attr, SyntaxNode root, SyntaxNode member)
    {
        var name = "JsonIgnore";
        switch (member)
        {
            case FieldDeclarationSyntax field:
            {
                field = root.Find(field);
                if (!field.HasAttribute(name))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref field, name);
                }
        
                break;
            }
            case PropertyDeclarationSyntax property:
            {
                property = root.Find(property);
                if (!property.HasAttribute(name))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref property, name);
                }

                break;
            }
        }

        return root;
    }

    public SyntaxNode OnSyntaxNode(SyntaxNode root, SyntaxNode member)
    {
        return root;
    }

    public string GetUsings()
    {
        return "using Newtonsoft.Json;\n";
    }
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace.Changers;

public class TextJsonChanger : IChanger
{
    public SyntaxNode OnSerializeThis(SerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        var name = "JsonPropertyName";
        
        switch (member)
        {
            case FieldDeclarationSyntax field:
                field = root.Find(field);
                if (!field.HasAttribute(name))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref field, name,
                        new AttributeAdder.AttributeParam(null, attr.Name));
                }
                
                break;
            case PropertyDeclarationSyntax property:
                property = root.Find(property);
                if (!property.HasAttribute(name))
                {
                    root = AttributeAdder.AddCustomAttribute(root, ref property, name,
                        new AttributeAdder.AttributeParam(null, attr.Name));
                }
                break;
        }

        return root;
    }

    public SyntaxNode OnDoNotSerializeThis(DoNotSerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        return root;
    }

    public SyntaxNode OnProperty(Property attr, SyntaxNode root, SyntaxNode member)
    {
        return root;
    }

    public SyntaxNode OnSyntaxNode(SyntaxNode root, SyntaxNode member)
    {
        return root;
    }
    
    public string GetUsings()
    {
        return "using System.Text.Json.Serialization;\n";
    }
}
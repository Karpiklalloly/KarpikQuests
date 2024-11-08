using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace.Changers;

public class TextJsonChanger : IChanger
{
    public SyntaxNode OnSerializeThis(SerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        root = member switch
        {
            FieldDeclarationSyntax field => AttributeAdder.AddCustomAttribute(root, ref field, "JsonPropertyName",
                new AttributeAdder.AttributeParam(null, attr.Name)),
            PropertyDeclarationSyntax property => AttributeAdder.AddCustomAttribute(root, ref property,
                "JsonPropertyName", new AttributeAdder.AttributeParam(null, attr.Name)),
            _ => root
        };

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
}
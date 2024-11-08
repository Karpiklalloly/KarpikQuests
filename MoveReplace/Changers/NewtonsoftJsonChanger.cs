using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace.Changers;

public class NewtonsoftJsonChanger : IChanger
{
    public SyntaxNode OnSerializeThis(SerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        root = member switch
        {
            FieldDeclarationSyntax field => AttributeAdder
                .AddCustomAttribute(root, ref field, "JsonProperty",
                    new AttributeAdder.AttributeParam("PropertyName", attr.Name)),
            
            PropertyDeclarationSyntax property => AttributeAdder.
                AddCustomAttribute(root, ref property, "JsonProperty",
                    new AttributeAdder.AttributeParam("PropertyName", attr.Name)),
            _ => root
        };

        return root;
    }

    public SyntaxNode OnDoNotSerializeThis(DoNotSerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        root = member switch
        {
            FieldDeclarationSyntax field => AttributeAdder.AddCustomAttribute(root, ref field, "JsonIgnore"),
            PropertyDeclarationSyntax property => AttributeAdder.AddCustomAttribute(root, ref property, "JsonIgnore"),
            _ => root
        };

        return root;
    }

    public SyntaxNode OnProperty(Property attr, SyntaxNode root, SyntaxNode member)
    {
        root = member switch
        {
            FieldDeclarationSyntax field => AttributeAdder.AddCustomAttribute(root, ref field, "JsonIgnore"),
            PropertyDeclarationSyntax property => AttributeAdder.AddCustomAttribute(root, ref property, "JsonIgnore"),
            _ => root
        };
        
        return root;
    }

    public SyntaxNode OnSyntaxNode(SyntaxNode root, SyntaxNode member)
    {
        return root;
    }
}
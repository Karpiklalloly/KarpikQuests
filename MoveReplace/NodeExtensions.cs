using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace;

public static class NodeExtensions
{
    public static IEnumerable<T> All<T>(this SyntaxNode root)
    {
        return root.DescendantNodes().OfType<T>();
    }

    public static string Name(this AttributeSyntax attribute)
    {
        return attribute.Name.NormalizeWhitespace().ToFullString();
    }
    
    //public static IEnumerable<T> With<T>(IEnumerable<T> root)
    public static string Value(this AttributeArgumentSyntax ats)
    {
        return ats.Expression.NormalizeWhitespace().ToFullString();
    }
    
    public static string? Name(this AttributeArgumentSyntax ats)
    {
        return ats.NameEquals?.Name.Identifier.Text;
    }
    
    public static bool HasAttribute(this MemberDeclarationSyntax node, string attributeName)
    {
        return node.AttributeLists.Any(x => x.Attributes.Any(x => x.Name() == attributeName));
    }
    
    public static AttributeSyntax GetAttribute(this MemberDeclarationSyntax node, string attributeName)
    {
        return node.AttributeLists
            .SelectMany(e => e.Attributes)
            .First(e => e.Name() == attributeName);
    }

    public static AttributeSyntax AddParameter(this AttributeSyntax attribute, string name, string value)
    {
        var param = $"{name} = {value}";
        
        var attributeArgument = SyntaxFactory.AttributeArgument(
            SyntaxFactory.LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Token(
                    default,
                    SyntaxKind.StringLiteralToken, 
                    param,
                    param, 
                    default)));

        var otherList = new SeparatedSyntaxList<AttributeArgumentSyntax>();
        otherList = otherList.Add(attributeArgument);
        var argumentList = SyntaxFactory.AttributeArgumentList(otherList);
        return SyntaxFactory.Attribute(attribute.Name, argumentList);
    }

    public static string TypeName(this FieldDeclarationSyntax node)
    {
        return node.Type().ToString();
    }

    public static TypeSyntax Type(this FieldDeclarationSyntax node)
    {
        return node.Declaration.Type;
    }

    public static FieldDeclarationSyntax? Find(this SyntaxNode root, FieldDeclarationSyntax field)
    {
        return AttributeAdder.Find(root, field);
    }

}
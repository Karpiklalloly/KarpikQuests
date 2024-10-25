using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace;

public static class AttributeAdder
{
    public static SyntaxNode AddCustomAttribute(SyntaxNode root, ref FieldDeclarationSyntax syntaxNode, string attributeName, params AttributeParam[] attributeParams)
    {
        var attributeSyntax = SyntaxFactory.Attribute(SyntaxFactory.ParseName(attributeName));

        foreach (var attributeParam in attributeParams)
        {
            attributeSyntax = attributeSyntax.AddParameter(attributeParam.Name, attributeParam.Value);
        }

        var memberDeclaration = Find(root, syntaxNode);

        if (memberDeclaration == null) return root;
        
        var newClassDeclaration = memberDeclaration.AddAttributeLists(SyntaxFactory.AttributeList().AddAttributes(attributeSyntax));
        syntaxNode = newClassDeclaration;
        return root.ReplaceNode(memberDeclaration, newClassDeclaration);
    }
    
    public static SyntaxNode AddCustomAttribute(SyntaxNode root, ref PropertyDeclarationSyntax syntaxNode, string attributeName, params AttributeParam[] attributeParams)
    {
        var attributeSyntax = SyntaxFactory.Attribute(SyntaxFactory.ParseName(attributeName));

        foreach (var attributeParam in attributeParams)
        {
            attributeSyntax = attributeSyntax.AddParameter(attributeParam.Name, attributeParam.Value);
        }

        var node = syntaxNode;

        PropertyDeclarationSyntax? memberDeclaration = root.DescendantNodes()
            .OfType<PropertyDeclarationSyntax>()
            .FirstOrDefault(x => x.IsEquivalentTo(node));

        if (memberDeclaration == null) return root;
        
        var newClassDeclaration = memberDeclaration.AddAttributeLists(SyntaxFactory.AttributeList().AddAttributes(attributeSyntax));
        syntaxNode = newClassDeclaration;
        return root.ReplaceNode(memberDeclaration, newClassDeclaration);
    }

    public static bool Equal(FieldDeclarationSyntax t1, FieldDeclarationSyntax t2)
    {
        if (t1.GetType() != t2.GetType())
        {
            return false; 
        }
        
        var equal = true;
        
        for (int i = 0; i < t1.Declaration.Variables.Count; i++)
        {
            var v1 = t1.Declaration.Variables[i];
            var v2 = t2.Declaration.Variables[i];
        
            if (v1.Identifier.Text != v2.Identifier.Text)
            {
                equal = false;
                break;
            }
        
            if (v1.Parent.GetType() != v2.Parent.GetType())
            {
                equal = false;
                break;
            }
            
            
        }
        
        return equal;
    }

    public static FieldDeclarationSyntax? Find(SyntaxNode root, FieldDeclarationSyntax node)
    {
        return root.DescendantNodes()
            .OfType<FieldDeclarationSyntax>()
            .FirstOrDefault(x =>
                x.Declaration.Variables.Count == node.Declaration.Variables.Count
                && Equal(x, node));
    }

    public class AttributeParam
    {
        public string Name;
        public string Value;

        public AttributeParam(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
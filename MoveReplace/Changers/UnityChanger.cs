using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace.Changers;

public class UnityChanger : IChanger
{
    public SyntaxNode OnSerializeThis(SerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        var name = attr.IsReference ? "SerializeReference" : "SerializeField";
        if (member is FieldDeclarationSyntax field)
        {
            field = root.Find(field);
            if (!field.HasAttribute(name))
            {
                root = AttributeAdder.AddCustomAttribute(
                    root,
                    ref field,
                    name);
            }
        }

        return root;
    }

    public SyntaxNode OnDoNotSerializeThis(DoNotSerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        return root;
    }

    public SyntaxNode OnProperty(Property attr, SyntaxNode root, SyntaxNode member)
    {
        var name = "CreateProperty";
        if (member is PropertyDeclarationSyntax property)
        {
            property = root.Find(property);
            if (!property.HasAttribute(name))
            {
                root = AttributeAdder.AddCustomAttribute(
                    root,
                    ref property,
                    name);
            }
        }
        
        return root;
    }

    public SyntaxNode OnSyntaxNode(SyntaxNode root, SyntaxNode member)
    {
        if (member is FieldDeclarationSyntax field)
        {
            field = root.Find(field);
            if (field.TypeName().Contains("Dictionary"))
            {
                var type = field.Type() as GenericNameSyntax;
                FieldDeclarationSyntax newField = field.WithDeclaration(field.Declaration.WithType(
                    TypeSyntaxFactory.GetTypeSyntax(
                        "SerializableDictionary",
                        type.TypeArgumentList.Arguments.ToArray()
                    )));

                field = root.Find(field);
                root = root.ReplaceNode(field, newField);
            }
        }

        return root;
    }

    public string GetUsings()
    {
        return "using UnityEngine;\n" +
               "using Karpik.UIExtension;\n" +
               "using Unity.Properties;\n";
    }
}
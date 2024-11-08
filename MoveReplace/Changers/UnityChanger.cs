using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace.Changers;

public class UnityChanger : IChanger
{
    public SyntaxNode OnSerializeThis(SerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        if (member is FieldDeclarationSyntax field)
        {
            root = AttributeAdder.AddCustomAttribute(
                root,
                ref field,
                attr.IsReference ? "SerializeReference" : "SerializeField");
        }

        return root;
    }

    public SyntaxNode OnDoNotSerializeThis(DoNotSerializeThis attr, SyntaxNode root, SyntaxNode member)
    {
        return root;
    }

    public SyntaxNode OnProperty(Property attr, SyntaxNode root, SyntaxNode member)
    {
        if (member is PropertyDeclarationSyntax property)
        {
            root = AttributeAdder.AddCustomAttribute(
                root,
                ref property,
                "CreateProperty");
        }
        
        return root;
    }

    public SyntaxNode OnSyntaxNode(SyntaxNode root, SyntaxNode member)
    {
        if (member is FieldDeclarationSyntax field)
        {
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
}
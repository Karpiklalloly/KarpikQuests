using Microsoft.CodeAnalysis;

namespace MoveReplace.Changers;

public interface IChanger
{
    public SyntaxNode OnSerializeThis(SerializeThis attr, SyntaxNode root, SyntaxNode member);
    public SyntaxNode OnDoNotSerializeThis(DoNotSerializeThis attr, SyntaxNode root, SyntaxNode member);
    public SyntaxNode OnProperty(Property attr, SyntaxNode root, SyntaxNode member);
    public SyntaxNode OnSyntaxNode(SyntaxNode root, SyntaxNode member);
    public string GetUsings();
}
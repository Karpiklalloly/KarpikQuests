using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace;

public class UsingAdder
{
    public (SyntaxTree, SyntaxTree) UpdateUsingDirectivesForChanges((SyntaxTree, SyntaxTree) change)
    {
        var qualifiedName = SyntaxFactory.ParseName(" Data.Application.Interfaces");
        var usingDirective = SyntaxFactory.UsingDirective(qualifiedName);
        var rootNode = change.Item2.GetRoot() as CompilationUnitSyntax;
        
        if (!rootNode.Usings.Select(d => d.Name.ToString()).Any(u => u == qualifiedName.ToString()))
        {
            rootNode = rootNode.AddUsings(usingDirective);
            change.Item2 = rootNode.SyntaxTree;
        }

        return change;
    }
}
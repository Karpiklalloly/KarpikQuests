using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace;

public class Runner
{
    private Dictionary<Type, Action<object>> _actions = new();
    private List<Func<SerializeThis, SyntaxNode, SyntaxNode, SyntaxNode>> _onSerializeThis = new();
    private List<Func<DoNotSerializeThis, SyntaxNode, SyntaxNode, SyntaxNode>> _onDoNotSerializeThis = new();
    private List<Func<Property, SyntaxNode, SyntaxNode, SyntaxNode>> _onProperty = new();

    private SyntaxNode _root;
    
    public Runner()
    {
        
    }

    public Runner OnAttribute<T>(Action<AttributeSyntax> action) where T : Attribute
    {
        _actions[typeof(T)] = x => action((AttributeSyntax)x);
        return this;
    }

    public Runner OnSerializeThis(Func<SerializeThis, SyntaxNode, SyntaxNode, SyntaxNode> func)
    {
        _onSerializeThis.Add(func);
        return this;
    }
    
    public Runner OnDoNotSerializeThis(Func<DoNotSerializeThis, SyntaxNode, SyntaxNode, SyntaxNode> func)
    {
        _onDoNotSerializeThis.Add(func);
        return this;
    }
    
    public Runner OnProperty(Func<Property, SyntaxNode, SyntaxNode, SyntaxNode> func)
    {
        _onProperty.Add(func);
        return this;
    }

    public SyntaxNode Run(SyntaxTree tree)
    {
        _root = tree.GetRoot();
        var nodes = _root.DescendantNodes();

        foreach (var node in nodes)
        {
            if (node is MemberDeclarationSyntax member)
            {
                ProcessMember(member);
            }
        }

        return _root;
    }

    private void ProcessMember(MemberDeclarationSyntax member)
    {
        foreach (var pair in _actions)
        {
            if (member.HasAttribute(pair.Key.Name))
            {
                pair.Value.Invoke(member.GetAttribute(pair.Key.Name));
            }
        }
        
        if (member.HasAttribute("SerializeThis"))
        {
            var serializeThis = member.GetAttribute("SerializeThis");
            var arguments = serializeThis.ArgumentList.Arguments;

            var n = arguments.First().Value();
            var serializeThisAttribute = new SerializeThis(n.Substring(1, n.Length - 2));

            foreach (var argument in arguments)
            {
                switch (argument.Name())
                {
                    case nameof(SerializeThis.IsReference):
                        serializeThisAttribute.IsReference = argument.Value() == "true";
                        break;
                }
            }

            foreach (var action in _onSerializeThis)
            {
                _root = action.Invoke(serializeThisAttribute, _root, member);
            }
        }
            
        if (member.HasAttribute("DoNotSerializeThis"))
        {
            var doNotSerializeThisAttribute = new DoNotSerializeThis();
            foreach (var action in _onDoNotSerializeThis)
            {
                _root = action.Invoke(doNotSerializeThisAttribute, _root, member);
            }
        }
            
        if (member.HasAttribute("Property"))
        {
            var property = new Property();
            foreach (var action in _onProperty)
            {
                _root = action.Invoke(property, _root, member);
            }
        }
    }
}
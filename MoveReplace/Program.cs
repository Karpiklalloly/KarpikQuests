using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace;

class Program
{
    static void Main(string[] args)
    {
        var solutionPath = args[0];

        var from = Path.Combine(solutionPath, "KarpikQuests");
        var to = Path.Combine(solutionPath, "KarpikQuestsUnity", "Core");

        FileManipulator.MoveIfPossible("Karpik.Quests.asmdef", to, solutionPath);
        Directory.Delete(Path.Combine(solutionPath, "KarpikQuestsUnity"), true);
        FileManipulator.CopyDirectory(from, to, true);
        Directory.Delete(Path.Combine(to, "bin"), true);
        Directory.Delete(Path.Combine(to, "obj"), true);
        File.Delete(Path.Combine(to, ".editorconfig"));
        File.Delete(Path.Combine(to, "KarpikQuests.csproj"));
        File.Delete(Path.Combine(to, "Newtonsoft.Json.dll"));

        var files = Directory.GetFiles(to, "*.cs", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var text = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(text);
            
            var root = tree.GetRoot();

            var fields = root.All<FieldDeclarationSyntax>().ToArray();

            bool changed = false;

            for (var index = 0; index < fields.Length; index++)
            {
                var field = fields[index];
                if (field.HasAttribute("SerializeThis"))
                {
                    var serializeThis = field.GetAttribute("SerializeThis");
                    var arguments = serializeThis.ArgumentList.Arguments;

                    var serializeThisAttribute = new SerializeThis(arguments.First().ToString());

                    foreach (var argument in arguments)
                    {
                        switch (argument.Name())
                        {
                            case nameof(SerializeThis.IsReference):
                                serializeThisAttribute.IsReference = argument.Value() == "true";
                                break;
                            case nameof(SerializeThis.IsProperty):
                                serializeThisAttribute.IsProperty = argument.Value() == "true";
                                break;
                        }
                    }

                    changed = true;
                    
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

                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref field,
                        serializeThisAttribute.IsReference ? "SerializeReference" : "SerializeField");

                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref field,
                        "JsonProperty",
                        new AttributeAdder.AttributeParam("PropertyName", serializeThisAttribute.Name));


                }
                else if (field.HasAttribute("DoNotSerializeThis"))
                {
                    changed = true;

                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref field,
                        "JsonIgnore");
                }
            }

            var properties = root.All<PropertyDeclarationSyntax>().ToArray();

            for (var index = 0; index < properties.Length; index++)
            {
                var property = properties[index];
                if (property.HasAttribute("Property"))
                {
                    changed = true;
                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref property,
                        "CreateProperty");
                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref property,
                        "JsonIgnore");
                }
                
                if (property.HasAttribute("DoNotSerializeThis"))
                {
                    changed = true;
                    
                    if (!property.HasAttribute("JsonIgnore"))
                    {
                        root = AttributeAdder.AddCustomAttribute(
                            root,
                            ref property,
                            "JsonIgnore");
                    }
                }
            }

            if (changed)
            {
                File.WriteAllText(file, "using UnityEngine;\n"
                                        + "using Karpik.UIExtension;\n"
                                        + "using Unity.Properties;\n"
                                        + "using Newtonsoft.Json;\n"
                                        + root.SyntaxTree.ToString());
            }
            
        }

        FileManipulator.MoveIfPossible("Karpik.Quests.asmdef", solutionPath, to);

        Console.WriteLine("Complete");
    }
}
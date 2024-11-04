using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoveReplace;

class Program
{
    static void Main(string[] args)
    {
        string solutionPath = null;
        if (args.Length > 0)
        {
            solutionPath = args[0];
        }
        else
        {
            solutionPath = @"C:\Users\artem\source\repos\KarpikQuests\";
        }

        var from = Path.Combine(solutionPath, "KarpikQuests");
        var to = Path.Combine(solutionPath, "KarpikQuestsUnity");
        var to2 = Path.Combine(solutionPath, "Builded", "Core");

        FileManipulator.MoveIfPossible("Karpik.Quests.asmdef", to, solutionPath);

        Do(from, to);
        Do(from, to2);
        
        FileManipulator.MoveIfPossible("Karpik.Quests.asmdef", solutionPath, to);
        
        Console.WriteLine("Complete");
    }

    static void Do(string from, string to)
    {
        Directory.Delete(to, true);
        Directory.CreateDirectory(to);
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

                    changed = true;
                    
#if UNITY
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
#endif
#if NEWTONSOFTJSON
                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref field,
                        "JsonProperty",
                        new AttributeAdder.AttributeParam("PropertyName", serializeThisAttribute.Name));
#endif
#if TEXTJSON
                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref field,
                        "JsonPropertyName",
                        new AttributeAdder.AttributeParam(null, serializeThisAttribute.Name));
#endif


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
#if UNITY
                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref property,
                        "CreateProperty");
#endif
#if NEWTONSOFTJSON
                    root = AttributeAdder.AddCustomAttribute(
                        root,
                        ref property,
                        "JsonIgnore");
#endif
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
                File.WriteAllText(file,
                    #if UNITY
                    "using UnityEngine;\n" +
                    "using Karpik.UIExtension;\n" +
                    "using Unity.Properties;\n" +
#endif
#if NEWTONSOFTJSON
                    "using Newtonsoft.Json;\n" +
#endif
#if TEXTJSON
                    "using System.Text.Json.Serialization;\n" +        
#endif

                    root.SyntaxTree.ToString());
            }
            
        }
    }
}
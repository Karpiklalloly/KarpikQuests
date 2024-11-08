using Microsoft.CodeAnalysis.CSharp;
using MoveReplace.Changers;

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

        IChanger[] changers =
        [
#if UNITY
            new UnityChanger(),
#endif
#if NEWTONSOFTJSON
            new NewtonsoftJsonChanger(),
#endif
#if TEXTJSON
            new TextJsonChanger(),
#endif
        ];
        

        foreach (var file in files)
        {
            var text = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(text);
            
            Runner runner = new Runner();
            foreach (var changer in changers)
            {
                runner.OnSerializeThis(changer.OnSerializeThis);
                runner.OnDoNotSerializeThis(changer.OnDoNotSerializeThis);
                runner.OnProperty(changer.OnProperty);
            }

            var root = runner.Run(tree);
            
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

        Console.WriteLine();
    }
}
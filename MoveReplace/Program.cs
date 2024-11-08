﻿using System.Text;
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
            solutionPath = @"..\..\..\..\..\KarpikQuests\";
        }

        var from = Path.Combine(solutionPath, "KarpikQuests");
        var to = Path.Combine(solutionPath, "KarpikQuestsUnity");
        var to2 = Path.Combine(solutionPath, "Builded", "Core");

        FileManipulator.MoveIfPossible("Karpik.Quests.asmdef", to, solutionPath);

        Do(from, to, new UnityChanger(), new NewtonsoftJsonChanger());
        Do(from, to2, new NewtonsoftJsonChanger());
        
        FileManipulator.MoveIfPossible("Karpik.Quests.asmdef", solutionPath, to);
        
        Console.WriteLine("Complete");
    }

    static void Do(string from, string to, params IChanger[] changers)
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
            
            Runner runner = new Runner();
            foreach (var changer in changers)
            {
                runner.OnSerializeThis(changer.OnSerializeThis);
                runner.OnDoNotSerializeThis(changer.OnDoNotSerializeThis);
                runner.OnProperty(changer.OnProperty);
            }

            var root = runner.Run(tree);
            
            StringBuilder builder = new StringBuilder();
            foreach (var changer in changers)
            {
                builder.Append(changer.GetUsings());
            }
            builder.Append(root.SyntaxTree.ToString());

            File.WriteAllText(file, builder.ToString());

        }

        Console.WriteLine();
    }
}
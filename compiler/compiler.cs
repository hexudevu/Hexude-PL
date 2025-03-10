using System;
using System.Collections.Generic;
using Utils;
namespace CompilerSpace {
    class Compiler {
        public static string FileBody = "hxd";
        public static CompiledObject Сompile(string[] args) {
            CompiledObject compiledObject = new CompiledObject();
            CF.WriteLine("Compiling...");
            compileObject(ref compiledObject, args);
            CF.WriteLine("Compiling code...:", ConsoleColor.Cyan);
            foreach (string arg in args)
            {
                CF.WriteLine($"[{Array.IndexOf(args, arg)}]: {arg}", ConsoleColor.DarkGray);
            }
            CF.WriteLine("Ready, returning <compiledObject>.", ConsoleColor.Green);
            return compiledObject;
        }
        private static void compileObject(ref CompiledObject obj, string[] args) {
            string fullCode = "";
            foreach (string arg in args)
            {
                fullCode += arg + '\n';
            }
            List<(Tokenizer.TokenType, string)> tokens = Tokenizer.Tokenize(fullCode);
            CF.WriteLine("Separating to Tokens...", ConsoleColor.White);
            foreach ((Tokenizer.TokenType, string) token in tokens)
            {
                if (token.Item1 == Tokenizer.TokenType.EndOfFile) 
                    CF.WriteLine($"[{token.Item1}]", ConsoleColor.Magenta);
                else
                {
                    CF.Write($"[{token.Item1}]: ", ConsoleColor.DarkMagenta);
                    CF.WriteLine($"{token.Item2}", ConsoleColor.Magenta);
                }
            }
        }
    }

    class CompiledObject {
        public void Execute() {

        }
    }
}
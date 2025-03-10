using System;
using System.Collections.Generic;
using Utils;
namespace CompilerSpace {
    class Compiler {
        public static string FileBody = "hexude";
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
            List<Token> tokens = Tokenizer.Tokenize(fullCode);
            CF.WriteLine("Separating to Tokens...");
            foreach (Token token in tokens)
            {
                if (token.Type == TokenType.EndOfFile) 
                    CF.WriteLine($"[{token.Type}]", ConsoleColor.Magenta);
                else
                {
                    CF.Write($"[{token.Type}]: ", ConsoleColor.DarkMagenta);
                    CF.WriteLine($"{token.Value}", ConsoleColor.Magenta);
                }
            }
        }
    }

    class CompiledObject {
        public void Execute() {

        }
    }
}
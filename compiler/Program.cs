using System;
using System.IO;

namespace CompilerSpace {
    internal class Program {
        static void Main(string[] args) {
            string pathToProject = "../../../..";
            string[] arguments = File.ReadAllLines($"{pathToProject}/exampleCode.{Compiler.FileBody}");
            Compiler.Сompile(arguments);
        }
    }
}
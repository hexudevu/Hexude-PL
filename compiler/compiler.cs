using System;
namespace CompilerSpace {
    class Compiler {
        public static string FileBody = "hxd";
        public static CompiledObject Сompile(string[] args) {
            CompiledObject compiledObject = new CompiledObject();
            foreach (string arg in args)
            {
                System.Console.WriteLine(arg);
            }
            return compiledObject;
        }
    }

    class CompiledObject {

    }
}
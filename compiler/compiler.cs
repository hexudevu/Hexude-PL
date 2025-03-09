using System;
namespace CompilerSpace {
    class Compiler {
        public static string FileBody = "hxd";
        public static CompiledObject Сompile(string[] args) {
            CompiledObject compiledObject = new CompiledObject();
            compileObject(ref compiledObject, args);
            foreach (string arg in args)
            {
                System.Console.WriteLine(arg);
            }
            return compiledObject;
        }
        private static void compileObject(ref CompiledObject obj, string[] args) {
            
        }
    }

    class CompiledObject {
        public void Execute() {

        }
    }
}
using System;

namespace Compiler {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Let's test project, and write something first.");
            int some = 2;
            Console.Write($"result {some}^2 = {Math.Pow(some, 2)}");
        }
    }
}
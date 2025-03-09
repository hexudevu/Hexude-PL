using System;
namespace Utils {
    public class CF { //CF = Console Features
        public static void WriteLine(string text, ConsoleColor foreColor = ConsoleColor.White, ConsoleColor backColor = ConsoleColor.Black) 
        {
            ConsoleColor pastForeColor = Console.ForegroundColor;
            ConsoleColor pastBackColor = Console.BackgroundColor;
            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;
            Console.WriteLine(text);
            Console.ForegroundColor = pastForeColor;
            Console.BackgroundColor = pastBackColor;
        }
        public static void Write(string text, ConsoleColor foreColor = ConsoleColor.White, ConsoleColor backColor = ConsoleColor.Black) 
        {
            ConsoleColor pastForeColor = Console.ForegroundColor;
            ConsoleColor pastBackColor = Console.BackgroundColor;
            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;
            Console.Write(text);
            Console.ForegroundColor = pastForeColor;
            Console.BackgroundColor = pastBackColor;
        }
    }
}
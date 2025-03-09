using System;
using System.Collections.Generic;

namespace Utils {
    public class Tokenizer {
        public enum TokenType
        {
            Identifier,    // переменная, имя функции, type
            Number,        // числа + числа с плавающей точкой
            Operator,      // +, -, *, /, ^, or, and
            Punctuation,   // ; , ( ) { }
            Keyword,       // if, while, return
            String,        // "чтото в кавычках"
            Comment,       // # или #[[ ]]
            EndOfFile      // конец файла
        }

        public static List<(TokenType, string)> Tokenize(string text)
        {
            List<(TokenType, string)> tokens = new List<(TokenType, string)>();

            return tokens;
        }

    }
}
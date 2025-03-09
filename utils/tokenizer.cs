using System;
using System.Collections.Generic;

namespace Utils {
    class Tokenizer {
        public enum TokenType
        {
            Identifier,    // переменная, имя функции, type
            Number,        // числа (пр-р, 123, 3.14)
            Operator,      // +, -, *, /
            Punctuation,   // ; , ( ) { }
            Keyword,       // if, while, return
            String,        // "Hello"
            Comment,       // # или #[[ ]]
            EndOfFile      // конец файла
        }

        public static List<(TokenType, string)> Tokenize(string text)
        {
            List<(TokenType, string)> tokens = new List<(TokenType, string)>();

            // Например, добавим токен "if" как ключевое слово
            tokens.Add((TokenType.Keyword, "if"));

            return tokens;
        }

    }
}
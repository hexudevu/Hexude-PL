using System;
using System.Collections.Generic;

namespace Utils {
    public class Tokenizer {
        public enum TokenType
        {
            none,           // None | like null
            Identifier,     // переменная, имя функции, type
            Number,         // числа + числа с плавающей точкой
            Operator,       // +, -, *, /, ^, or, and
            Punctuation,    // ; , ( ) { }
            Keyword,        // if, while, return
            String,         // "строки"
            Comment,        // # или #[[ ]]
            EndOfFile       // конец файла
        }

        public static List<(TokenType, string)> Tokenize(string text)
        {
            List<(TokenType, string)> tokens = new List<(TokenType, string)>();
            string currentToken = "";
            TokenType currentType = TokenType.none;

            //добавить токен
            void addToken() {
                if (!string.IsNullOrEmpty(currentToken)) {
                    tokens.Add((currentType, currentToken));
                    currentToken = "";
                    currentType = TokenType.none;
                }
            }
            void addAdditionalToken(TokenType type, string token) {
                tokens.Add((type, token));
            }

            int skipTo = -1;
            //скипнуть выполнение до индекса
            void skip(int index) {
                if (!(skipTo != -1 && skipTo >= index))
                    skipTo = index;
            }

            //перебор по символам и логика токенайзера
            for (int i = 0; i < text.Length; i++) {
                if (skipTo != -1) {
                    if (skipTo > i) {
                        continue;
                    } else {
                        skipTo = -1;
                    }
                }
                char x = text[i];
                switch (x) {
                    //разделение токенов
                    case ' ':
                        addToken();
                        break;
                    //обработка операторов #1
                    case '+': case '-': case '*': case '/': case '^': case '=':
                        addToken();
                        addAdditionalToken(TokenType.Operator, x.ToString());
                        break;
                    //оптимизация проверок + остальное
                    default:
                        //обработка чисел
                        if (char.IsDigit(x)) {
                            if (string.IsNullOrEmpty(currentToken))
                                currentType = TokenType.Number;
                            for (int j = i; j < text.Length; j++) {
                                char y = text[j];
                                if (char.IsDigit(y)) {
                                    currentToken += y;
                                    continue;
                                } else {
                                    addToken();
                                    skip(j);
                                    break;
                                }
                            }
                        }
                        break;
                }
            }

            addToken();

            return tokens;
        }

    }
}
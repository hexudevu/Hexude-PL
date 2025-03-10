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
                //skip logic
                if (skipTo != -1) {
                    if (skipTo > i) {
                        continue;
                    } else {
                        skipTo = -1;
                    }
                }
                //обработка символа
                char x = text[i];
                switch (x) {
                    //разделение токенов
                    case ' ':
                        addToken();
                        break;
                    //обработка арифметических операторов #1
                    case '+': case '-': case '*': case '/': case '^':
                        addToken();
                        addAdditionalToken(TokenType.Operator, x.ToString());
                        break;
                    //обработка логических операторов сравнений #1
                    case '>': case '<':
                        if (text[i+1] == '=') {
                            addToken();
                            addAdditionalToken(TokenType.Operator, x+"=");
                            skip(i+2);
                        } else {
                            addToken();
                            addAdditionalToken(TokenType.Operator, x.ToString());
                        }
                        break;
                    //обработка логических операторов сравнений #2 + арифметическое равно
                    case '=': case '!':
                        if (text[i+1] == '=') {
                            addToken();
                            addAdditionalToken(TokenType.Operator, x+"=");
                            skip(i+2);
                        } else if (x == '=') {
                            addToken();
                            addAdditionalToken(TokenType.Operator, "=");
                        }
                        break;
                    //обработка пунктуационных символов
                    case '(': case ')': case '{': case '}': case '[': case ']': case ';': case ',':
                        addToken();
                        addAdditionalToken(TokenType.Punctuation, x.ToString());
                        break;
                    //обработка конца строки
                    case '\n':
                        addToken();
                        break;
                    //оптимизация проверок + остальное
                    default:
                        //обработка чисел
                        if (char.IsDigit(x)) {
                            if (string.IsNullOrEmpty(currentToken))
                                currentType = TokenType.Number;
                            bool dot = false;
                            for (int j = i; j < text.Length; j++) {
                                char y = text[j];
                                if (char.IsDigit(y) || (!dot && y == '.')) {
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
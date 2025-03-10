using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Utils {
    public class Tokenizer {
        public enum TokenType
        {
            none,           // None | like null                 | true
            Identifier,     // переменная, имя функции, type    | true
            Number,         // числа + числа с плавающей точкой | true
            Operator,       // +, -, *, /, ^, or, and           | true
            Punctuation,    // ; , ( ) { }                      | true
            Keyword,        // if, while, return                | true
            String,         // "строки"                         | true
            Comment,        // ~! OR ~!{    }~                  | true
            EndOfFile       // конец файла                      | true
        }

        public static string[] keywords = {
            "if", "elseif", "else",
            "while", "for", "foreach",
            "func", "return", 
            "var", "const", "let",
        };

        public static List<(TokenType, string)> Tokenize(string text)
        {
            List<(TokenType, string)> tokens = new List<(TokenType, string)>();
            string currentToken = "";
            TokenType currentType = TokenType.none;

            //добавить символ в текущий токен
            void addSymbol(char symbol) {
                if (string.IsNullOrEmpty(currentToken))
                    currentToken = symbol.ToString();
                else
                    currentToken += symbol; 
            }

            //переосмысление типа
            void rethinkType() {
                if (keywords.Contains(currentToken) && currentType != TokenType.String) {
                    currentType = TokenType.Keyword;
                }
                else if (char.IsLetter(currentToken[0]) && currentType != TokenType.String) {
                    currentType = TokenType.Identifier;
                }
            }

            //добавить токен
            void addToken() {
                if (!string.IsNullOrEmpty(currentToken)) {
                    rethinkType();
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
                //endOfFile logic
                if (i == text.Length-1) {
                    addToken();
                    addAdditionalToken(TokenType.EndOfFile, null);
                    break;
                }
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
                    //обработка строчных типов
                    case '"':
                        for (int j = i+1; j < text.Length; j++) {
                            char y = text[j];
                            if (y == '"') {
                                addToken();
                                addAdditionalToken(TokenType.String, text.Substring(i+1, j-i-1));
                                skip(j+1);
                                break;
                            }
                        }
                        break;
                    //обработка комментариев + позже побитовый оператор
                    case '~':
                        char waitFor = '\n';
                        if (text[i+1] == '!' && text[i+2] != '{')
                            addToken();
                        else if (text[i+1] == '!' && text[i+2] == '{') {
                            addToken();
                            waitFor = '}';
                        }
                        for (int j = i+1; j < text.Length; j++) {
                            char y = text[j];
                            if (y == waitFor && waitFor=='\n') {
                                addAdditionalToken(TokenType.Comment, text.Substring(i+2, j-i-2));
                                skip(j);
                                break;
                            } else if (y == waitFor && waitFor=='}') {
                                addAdditionalToken(TokenType.Comment, text.Substring(i+2, j-i-1));
                                skip(j+1);
                                break;
                            }
                        }
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
                                    if (y=='.') dot = true;
                                    currentToken += y;
                                    continue;
                                } else {
                                    addToken();
                                    skip(j);
                                    break;
                                }
                            }
                        }
                        //обработка ключевых слов
                        else if (char.IsLetter(x)) {
                            addSymbol(x);
                        }
                        break;
                }
            }

            addToken();

            return tokens;
        }

    }
}
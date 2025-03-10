using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils {
    public enum TokenType
    {
        none,           // None                             | true
        Identifier,     // переменная, имя функции          | true
        Integer,        // int number                       | true
        Float,          // float number                     | true
        String,         // "строки"                         | true
        Range,          // пр-р: 2:10                       | true
        Operator,       // +, -, *, /, ^                    | true 
        LogicOperator,  // or, and, not                     | true
        UnaryOperator,  // x++; x--;                        | true
        Comparison,     // >, <, >=...                      | true
        Assignment,     // =, +=, -=...                     | true
        Bracket,        // ( ); { }; [ ];                   | true
        Punctuation,    // ; , .                            | true
        Literal,        // true, false, nil                 | true
        Keyword,        // if, while, return                | true
        FunctionCall,   // built-in функции                 | true
        Comment,        // ~! OR ~!{    }~                  | true
        EndOfFile       // конец файла                      | true
    }
    public class Token {
        public string Value {get; private set;}
        public TokenType Type {get; private set;}
        
        public Token(TokenType type, string value) {
            Type = type;
            Value = value;
        }
    }
    public class Tokenizer {
        public static string[] literals = {
            "true", "false", "nil"
        };
        public static string[] keywords = {
            "if", "elseif", "else", "in",
            "while", "for", "foreach",
            "func", "return", 
            "var", "const", "let",
        };
        public static string[] builtins = {
            "LogLine", "Log", 
            "min", "max", "clamp",
            "random"
        };
        public static string[] logicOperators = {
            "or", "and", "not"
        };
        public static List<Token> Tokenize(string text)
        {
            List<Token> tokens = new List<Token>();
            string currentToken = "";
            TokenType currentType = TokenType.none;

            //сокращение индекса мб
            int clampIndex(int index) {
                return Math.Clamp(index, 0, text.Length-1);
            }
            //добавить символ в текущий токен
            void addSymbol(char symbol) {
                if (string.IsNullOrEmpty(currentToken))
                    currentToken = symbol.ToString();
                else
                    currentToken += symbol; 
            }

            //переосмысление типа
            void rethinkType() {
                if (currentType == TokenType.String) return;
                if (keywords.Contains(currentToken))
                    currentType = TokenType.Keyword;
                else if (literals.Contains(currentToken))
                    currentType = TokenType.Literal;
                else if (builtins.Contains(currentToken))
                    currentType = TokenType.FunctionCall;
                else if (logicOperators.Contains(currentToken))
                    currentType = TokenType.LogicOperator;
                else if (char.IsLetter(currentToken[0]))
                    currentType = TokenType.Identifier;
            }

            //добавить токен
            void addToken() {
                if (!string.IsNullOrEmpty(currentToken)) {
                    rethinkType();
                    tokens.Add(new Token(currentType, currentToken));
                    currentToken = "";
                    currentType = TokenType.none;
                }
            }
            void addAdditionalToken(TokenType type, string token) {
                tokens.Add(new Token(type, token));
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
                    //обработка арифметических операторов #1 + унарные операторов + присвоения #1
                    case '+': case '-':
                        addToken();
                        if (text[i+1] == x) {
                            addAdditionalToken(TokenType.UnaryOperator, x.ToString()+x.ToString());
                            skip(i+2);
                        } else if (text[i+1] == '=') {
                            addAdditionalToken(TokenType.Assignment, x.ToString()+'=');
                            skip(i+2);
                        } else
                            addAdditionalToken(TokenType.Operator, x.ToString());
                        break;
                    //обработка арифметических операторов #2 + присвоения #2
                    case '*': case '/': case '^':
                        addToken();
                        if (text[i+1] == '=') {
                            addAdditionalToken(TokenType.Assignment, x.ToString()+'=');
                            skip(i+2);
                        } else
                            addAdditionalToken(TokenType.Operator, x.ToString());
                        break;
                    //обработка логических операторов сравнений #1
                    case '>': case '<':
                        if (text[i+1] == '=') {
                            addToken();
                            addAdditionalToken(TokenType.Comparison, x+"=");
                            skip(i+2);
                        } else {
                            addToken();
                            addAdditionalToken(TokenType.Comparison, x.ToString());
                        }
                        break;
                    //обработка логических операторов сравнений #2 + арифметическое равно
                    case '=': case '!':
                        if (text[i+1] == '=') {
                            addToken();
                            addAdditionalToken(TokenType.Comparison, x+"=");
                            skip(i+2);
                        } else if (x == '=') {
                            addToken();
                            addAdditionalToken(TokenType.Assignment, "=");
                        }
                        break;
                    //обработка скобок
                    case '(': case ')': case '{': case '}': case '[': case ']':
                        addToken();
                        addAdditionalToken(TokenType.Bracket, x.ToString());
                        break;
                    //обработка пунктуации
                    case ';': case ',': case '.':
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
                    //обработка диапазонов
                    case ':': 
                        int lastTokenIndex = tokens.Count-1;
                        Token lastToken = tokens[lastTokenIndex];
                        if (string.IsNullOrEmpty(currentToken) && text[i-1]!=' ' && lastToken.Type == TokenType.Integer) {
                            string min = lastToken.Value;
                            tokens.RemoveAt(lastTokenIndex);
                            currentToken+=min+':';
                            for (int j = i+1; j < text.Length; j++) {
                                char y = text[j];
                                if (char.IsDigit(y))
                                    currentToken+=y;
                                else {
                                    currentType = TokenType.Range;
                                    addToken();
                                    skip(j);
                                    break;
                                }
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
                                addAdditionalToken(TokenType.Comment, text.Substring(i+2, clampIndex(j-i-2)));
                                skip(j);
                                break;
                            } else if (y==waitFor && text[j+1]=='~' && waitFor=='}') {
                                addAdditionalToken(TokenType.Comment, text.Substring(i+2, clampIndex(j-i-1)));
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
                                currentType = TokenType.Integer;
                            bool dot = false;
                            for (int j = i; j < text.Length; j++) {
                                char y = text[j];
                                if (char.IsDigit(y) || (!dot && y == '.')) {
                                    if (y=='.') {
                                        dot = true;
                                        currentType = TokenType.Float;
                                    }
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
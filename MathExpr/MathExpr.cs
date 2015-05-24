using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace MathExpr
{
    public class Evaler
    {
        public static float Eval(string exprString)
        {
            return Eval(Parser.Parse(exprString));
        }

        public static float Eval(string[] postfixNotationTokens)
        {
            Stack<float> stack = new Stack<float>();
            for (var i = 0; i < postfixNotationTokens.Length; i++)
            {
                var tok = postfixNotationTokens[i];
                if (!Utils.isOp(tok))
                {
                    stack.Push(float.Parse(tok));
                }
                else
                {
                    var x = stack.Pop();
                    var y = stack.Pop();

                    switch (tok)
                    {
                        case "*":
                            stack.Push(x * y);
                            break;
                        case "/":
                            stack.Push(x / y);
                            break;
                        case "+":
                            stack.Push(x + y);
                            break;
                        case "-":
                            stack.Push(x - y);
                            break;
                        case "%":
                            stack.Push(x % y);
                            break;
                    }
                }
            }
            return stack.Pop();
        }
    }

    public class Parser
    {
        public static string[] Parse(string exprString)
        {
            return ToPostfixNotation(Tokenize(exprString));
        }

        internal static string[] Tokenize(string input)
        {
            int pos = 0;
            int len = input.Length;
            List<string> tokens = new List<string>();

            while (pos < len)
            {
                string tok = input[pos++].ToString();

                int lastIndex = tokens.Count - 1;

                switch (tok)
                {
                    case "-":
                    case "+":
                        bool isUnary = lastIndex < 0 || !Utils.isNum(tokens[lastIndex]);

                        if (isUnary)
                        {
                            tokens.Add(tok + "1");
                            tokens.Add("*");
                        }
                        else
                        {
                            tokens.Add(tok);
                        }
                        break;
                    case "*":
                    case "/":
                    case "%":
                        if (lastIndex >= 0 && !Utils.isNum(tokens[lastIndex]))
                        {
                            // raise error
                        }
                        tokens.Add(tok);
                        break;
                    case "(":
                        tokens.Add(tok);
                        break;
                    case ")":
                        if (lastIndex < 0 || !Utils.isNum(tokens[lastIndex]))
                        {
                            // raise error
                        }

                        tokens.Add(tok);
                        break;
                    case " ":
                        break;
                    default:
                        if (lastIndex >= 0 && Utils.isNum(tokens[lastIndex]))
                        {
                            tokens[lastIndex] += tok;
                            break;
                        }

                        tokens.Add(tok);
                        break;
                }

            }

            return tokens.ToArray();
        }


        internal static string[] ToPostfixNotation(string[] tokens)
        {
            int pos = 0;
            int len = tokens.Length;
            Stack<string> stack = new Stack<string>();
            List<string> postfix = new List<string>();
            Dictionary<string, int> binaryPrec = new Dictionary<string, int>()
            {
                { "+", 1 },
                { "-", 1 },
                { "*", 2 },
                { "%", 2 },
            };

            while (pos < len)
            {
                var tok = tokens[pos++];

                var isBinaryOp = binaryPrec.ContainsKey(tok);

                if (!isBinaryOp)
                {
                    if (tok == "(")
                    {
                        stack.Push(tok);
                    }
                    else if (tok == ")")
                    {
                        while (true)
                        {
                            string n = stack.Pop();
                            if (n == "(")
                            {
                                break;
                            }
                            postfix.Add(n);
                        }
                    }
                    else
                    {
                        postfix.Add(tok);
                    }
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(tok);
                    }
                    else
                    {
                        string prev = stack.Peek();
                        if (binaryPrec.ContainsKey(prev) && binaryPrec[prev] > binaryPrec[tok])
                        {
                            postfix.Add(prev);
                        }
                        stack.Push(tok);
                    }
                    
                }
            }
            while (stack.Count > 0)
            {
                postfix.Add(stack.Pop());
            }

            return postfix.ToArray();
        }
    }

    internal class Utils
    {
        public static bool isOp(string tok)
        {
            return tok == "-" || tok == "+" || tok == "*" || tok == "/" && tok == "%";
        }

        public static bool maybeUnaryOp(string tok)
        {
            return tok == "-" || tok == "+";
        }

        public static bool isNum(string str)
        {
            return Regex.IsMatch(str, @"[-+]?[0-9]+\.?[0-9]*");
        }
    }
}

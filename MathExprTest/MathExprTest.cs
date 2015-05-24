using System;
using NUnit.Framework;

using MathExpr;

namespace MathExprTest
{
    [TestFixture()]
    public class ParserTest
    {
        [Test()]
        [TestCase("3 +(1+ 2 +-1) *3", new string[]{ "3", "+", "(", "1", "+", "2", "+", "-1", "*", "1", ")", "*", "3" })]
        [TestCase("3 +(1+ 2 +- 1) * 3", new string[]{ "3", "+", "(", "1", "+", "2", "+", "-1", "*", "1", ")", "*", "3" })]
        [TestCase("3 +    (  1+ 2 + - 1) *3", new string[]{ "3", "+", "(", "1", "+", "2", "+", "-1", "*", "1", ")", "*", "3" })]

        [TestCase("3 / 2 *5    (  1+ 2 + - 1) *3", new string[]{ "3", "/", "2", "*", "5", "(", "1", "+", "2", "+", "-1", "*", "1", ")", "*", "3" })]

        [TestCase("2 + (-3)", new string[]{ "2", "+", "(", "-1", "*", "3", ")" })]
        [TestCase("2 +-40", new string[]{ "2", "+", "-1", "*", "40" })]
        [TestCase("-1", new string[]{ "-1", "*", "1" })]
        [TestCase("(-3)", new string[]{ "(", "-1", "*", "3", ")" })]

        [TestCase("(12)", new string[]{ "(", "12", ")" })]
        [TestCase("(12.52)", new string[]{ "(", "12.52", ")" })]
        [TestCase("-3 + - 12.52", new string[]{ "-1", "*", "3", "+", "-1", "*", "12.52" })]
        [TestCase("+3 + - 12.52", new string[]{ "+1", "*", "3", "+", "-1", "*", "12.52" })]
        [TestCase("++--3", new string[]{ "+1", "*", "+1", "*", "-1", "*", "-1", "*", "3" })]
        public void TestTokenize(string input, string[] expected)
        {
            CollectionAssert.AreEqual(expected, Parser.Tokenize(input));
        }

        [Test()]
        [TestCase(new string[]{ "3", "+", "(", "1", "+", "2", "+", "1", ")", "*", "5" }, new string[]{ "3", "1", "2", "1", "+", "+", "5", "*", "+" })]
        public void TestToPostfixNotation(string[] input, string[] expected)
        {
            CollectionAssert.AreEqual(expected, Parser.ToPostfixNotation(input));
        }

        [Test()]
        [TestCase("3 + 2 * 5.2", 13.4f)]
        public void TestEval(string input, float expected)
        {
            Assert.AreEqual(expected, Parser.Eval(input), 0.0001f);
        }
    }
}

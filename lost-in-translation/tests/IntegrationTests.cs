namespace tests {
    using System;
    using System.IO;
    using System.Text;

    using FluentAssertions;

    using foobar;

    using FParsec.CSharp;

    using Xunit;

    public class IntegrationTests {
        [Fact] void HelloWorld() =>
            OutputOf("println 'hello world'")
            .Should().BeEquivalentTo(
                "hello world",
                "");

        [Fact] void HelloBreakWorld() =>
            OutputOf(
                "println 'hello'",
                "println 'world'")
            .Should().BeEquivalentTo(
                "hello",
                "world",
                "");

        public class VarDecl {
            [Fact] void Int() => OutputOf("int i = 42").Should().BeEquivalentTo("");

            [Fact] void NegativeInt() => OutputOf("int neg = -42").Should().BeEquivalentTo("");

            [Fact] void Float() => OutputOf("float f = 0.42").Should().BeEquivalentTo("");

            [Fact] void NegativeFloat() => OutputOf("float neg = -0.42").Should().BeEquivalentTo("");

            [Fact] void String() => OutputOf("string str = 'foo'").Should().BeEquivalentTo("");

            [Fact] void EmptyString() => OutputOf("string empty = ''").Should().BeEquivalentTo("");

            [Fact] void CannotAssignStringToInt() =>
                OutputOf("int i = 'foo'")
                .Should().StartWith("The string value 'foo' cannot be assigned to int variable 'i'.");

            [Fact] void CannotAssignIntToString() =>
                OutputOf("string s = 42")
                .Should().StartWith("The int value 42 cannot be assigned to string variable 's'.");

            [Fact] void CannotAssignIntToFloat() =>
                OutputOf("float f = 3")
                .Should().StartWith("The int value 3 cannot be assigned to float variable 'f'.");
        }

        public class PlusExpr {
            [Fact] void AddsTwoInts() =>
                OutputOf(
                    "int x = 1",
                    "int y = 2",
                    "println x + y")
                .Should().StartWith("3");

            [Fact] void AddsFloatToInt() =>
                OutputOf(
                    "int x = 1",
                    "float y = 0.23",
                    "println x + y")
                .Should().StartWith("1.23");

            [Fact] void AddsIntToFloat() =>
                OutputOf(
                    "float x = 0.23",
                    "int y = 1",
                    "println x + y")
                .Should().StartWith("1.23");

            [Fact] void AddsTwoFloats() =>
                OutputOf(
                    "float x = 0.23",
                    "float y = 0.77",
                    "println x + y")
                .Should().StartWith("1");

            [Fact] void ConcatsStrings() =>
                OutputOf(
                    "string s1 = 'foo'",
                    "string s2 = 'bar'",
                    "println s1 + s2")
                .Should().StartWith("foobar");

            [Fact] void CannotAddStringToInt() =>
                OutputOf(
                    "int x = 1",
                    "string y = '2'",
                    "println x + y")
                .Should().StartWith("Cannot apply operator '+' to left int value 1 and right string value '2'.");

            [Fact] void CannotAddIntToString() =>
                OutputOf(
                    "string x = '2'",
                    "int y = 1",
                    "println x + y")
                .Should().StartWith("Cannot apply operator '+' to left string value '2' and right int value 1.");
        }

        [Fact] void ArithmeticExpr() =>
            OutputOf("println ((1 + 2) + (3 + (4.5 + 6.7)))")
            .Should().StartWith("17.2");

        private static string[] OutputOf(params string[] scriptLines) {
            var script = string.Join(Environment.NewLine, scriptLines);
            var ast = Parser.Parse(script).GetResult();

            var output = new StringBuilder();
            using var stdout = new StringWriter(output);
            var rte = RTE.WithPrelude(stdout);

            Interpreter.Interpret(ast, rte);

            return output.ToString().Split(Environment.NewLine);
        }
    }
}

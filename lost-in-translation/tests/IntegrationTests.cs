namespace tests {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

        [Fact] void AddOp() =>
            OutputOf(
                "int x = -1",
                "float y = 0.23",
                "println x + y")
            .Should().StartWith("-0.77");

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

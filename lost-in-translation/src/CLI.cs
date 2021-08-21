namespace foobar {
    using System;

    using FParsec.CSharp;

    public static class CLI {
        public static void Main() {
            var ast = Parser
                .Parse(
                    "string x = 123\n" +
                    "float y = 0.456\n" +
                    "float z = x + 'test'\n" +
                    "println z")
                .GetResult();
            var rte = RTE.WithPrelude(stdout: Console.Out);
            Interpreter.Interpret(ast, rte);
        }
    }
}

namespace foobar {
    using FParsec.CSharp;

    public static class CLI {
        public static void Main() {
            var ast = Parser
                .Parse(
                    "int x = 123\n" +
                    "float y = 0.456\n" +
                    "float z = x + y\n" +
                    "println z")
                .GetResult();

            Interpreter.Interpret(ast, RTE.Builtin);
        }
    }
}

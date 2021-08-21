namespace foobar {
    using System.Collections.Generic;
    using Microsoft.FSharp.Core;
    using Microsoft.FSharp.Collections;

    using FParsec;
    using FParsec.CSharp;

    using static FParsec.CharParsers;
    using static FParsec.CSharp.CharParsersCS;
    using static FParsec.CSharp.PrimitivesCS;

    using StringParser = Microsoft.FSharp.Core.FSharpFunc<FParsec.CharStream<Microsoft.FSharp.Core.Unit>, FParsec.Reply<string>>;

    public static class Parser {
        private static readonly FSharpFunc<CharStream<Unit>, Reply<FSharpList<Stmnt>>> ScriptParser;

        private static readonly HashSet<string> keywords = new() { "int", "float", "string", "void" };

        static Parser() {
            static StringParser notReserved(string id) => keywords.Contains(id) ? Zero<string>() : Return(id);

            var ws = SkipManyChars(c => c is ' ' or '\t');
            var ws1 = SkipMany1Chars(c => c is ' ' or '\t');

            var identifier1 = Choice(Letter, CharP('_'));
            var identifierRest = Choice(Letter, CharP('_'), Digit);
            var identifier = Purify(Many1Chars(identifier1, identifierRest)).AndTry(notReserved)
                .Lbl("identifier");

            var expression = new OPPBuilder<Unit, Expr, Unit>()
                .WithOperators(ops => ops.AddInfix("+", 10, ws, AddExpr.Of))
                .WithTerms(term =>
                    Choice(
                        Between(CharP('(').And(ws), term, CharP(')').And(ws)),
                        Between('\'', ManyChars(c => c is not '\'' and not '\r' and not '\n'), '\'')
                            .And(ws).Map(StringLiteral.Of).Lbl("string literal"),
                        Int.AndTry(NotFollowedBy(CharP('.'))).And(ws).Map(IntLiteral.Of).Lbl("integer"),
                        Float.And(ws).Map(FloatLiteral.Of).Lbl("decimal number"),
                        identifier.And(ws).Map(VarRef.Of))
                    .Lbl("expression"))
                .Build();

            var typeDecl =
                Choice(
                    Skip("int").Map(() => Type.Int),
                    Skip("float").Map(() => Type.Float),
                    Skip("string").Map(() => Type.String),
                    Skip("void").Map(() => Type.Void))
                .Lbl("type specifier");

            var varDecl = typeDecl.And(ws1).And(identifier).And(ws)
                .And(Skip('=')).And(ws).And(expression)
                .Map(Flat)
                .Map(VarDecl.Of)
                .Lbl("variable declaration");

            var funcArgs = Many1(expression, sep: ws1).Lbl("argument list");
            var funcCall = identifier.And(ws1).And(funcArgs)
                .Map(FuncCall.Of)
                .Lbl("function call");

            ScriptParser = Many(Choice(varDecl, funcCall), sep: Newline, canEndWithSep: true).And(EOF);
        }

        public static ParserResult<FSharpList<Stmnt>, Unit> Parse(string text) => ScriptParser.Run(text);
    }
}

namespace foobar {
    using System;
    using System.Linq;
    using Microsoft.FSharp.Collections;

    public class Interpreter : Visitor {
        private RTE rte;

        private Value retVal = Value.Void;

        public Interpreter(RTE rte) => this.rte = rte;

        public static void Interpret(FSharpList<Stmnt> stmnts, RTE rte) {
            var interpreter = new Interpreter(rte);

            foreach (var stmnt in stmnts) {
                stmnt.Accept(interpreter);
            }
        }

        public void Visit(FuncCall x) => retVal = rte.Funcs[x.Id].Execute(x.Args.Select(Eval));

        public void Visit(VarDecl x) {
            rte = rte.Add(new Var(x.Id, Eval(x.Value)));
            retVal = Value.Void;
        }

        public void Visit(StringLiteral x) => retVal = new(Type.String, x.Value);

        public void Visit(IntLiteral x) => retVal = new(Type.Int, x.Value);

        public void Visit(FloatLiteral x) => retVal = new(Type.Float, x.Value);

        public void Visit(VarRef x) => retVal = rte.Vars[x.Id].Value;

        public void Visit(AddExpr x) {
            var left = Eval(x.Left);
            var right = Eval(x.Right);
            retVal = (left.Type, right.Type) switch {
                (Type.Int,   Type.Int)   => new(Type.Int,   left.IntVal   + right.IntVal),
                (Type.Int,   Type.Float) => new(Type.Float, left.IntVal   + right.FloatVal),
                (Type.Float, Type.Int)   => new(Type.Float, left.FloatVal + right.IntVal),
                (Type.Float, Type.Float) => new(Type.Float, left.FloatVal + right.FloatVal),
                _ => throw new Exception($"Cannot add left {left} and right {right}.")
            };
        }

        private Value Eval(Expr expr) {
            expr.Accept(this);
            return retVal;
        }
    }
}

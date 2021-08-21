namespace foobar {
    using System.Linq;
    using Microsoft.FSharp.Collections;

    public class Interpreter : Visitor {
        private RTE rte;

        private Value retVal = Value.Void;

        public Interpreter(RTE rte) => this.rte = rte;

        public static void Interpret(FSharpList<Stmnt> stmnts, RTE rte) {
            var interpreter = new Interpreter(rte);

            try {
                foreach (var stmnt in stmnts) {
                    stmnt.Accept(interpreter);
                }
            } catch (RuntimeError err) {
                rte.StdOut.WriteLine(err.Message);
            }
        }

        public void Visit(FuncCall funcCall) => retVal = rte.Funcs[funcCall.Id].Execute(funcCall.Args.Select(Eval));

        public void Visit(VarDecl varDecl) {
            var varVal = Eval(varDecl.Value);

            if (varVal.Type != varDecl.Type) throw new RuntimeError($"The {varVal} cannot be assigned to {varDecl}.");

            rte = rte.Add(new Var(varDecl.Id, varVal));
            retVal = Value.Void;
        }

        public void Visit(StringLiteral stringLit) => retVal = new(Type.String, stringLit.Value);

        public void Visit(IntLiteral intLit) => retVal = new(Type.Int, intLit.Value);

        public void Visit(FloatLiteral floatLit) => retVal = new(Type.Float, floatLit.Value);

        public void Visit(VarRef varRef) => retVal = rte.Vars[varRef.Id].Value;

        public void Visit(AddExpr addExpr) {
            var left = Eval(addExpr.Left);
            var right = Eval(addExpr.Right);
            retVal = (left.Type, right.Type) switch {
                (Type.Int,   Type.Int)   => new(Type.Int,   left.Int   + right.Int),
                (Type.Int,   Type.Float) => new(Type.Float, left.Int   + right.Float),
                (Type.Float, Type.Int)   => new(Type.Float, left.Float + right.Int),
                (Type.Float, Type.Float) => new(Type.Float, left.Float + right.Float),
                _ => throw new RuntimeError($"Cannot add left {left} and right {right}.")
            };
        }

        private Value Eval(Expr expr) {
            expr.Accept(this);
            return retVal;
        }
    }
}

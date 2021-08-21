namespace foobar {
    using Microsoft.FSharp.Collections;

    public interface Visitor {
        void Visit(FuncCall funcCall);
        void Visit(VarDecl varDecl);
        void Visit(StringLiteral stringLit);
        void Visit(IntLiteral intLit);
        void Visit(FloatLiteral floatLit);
        void Visit(VarRef varRef);
        void Visit(PlusExpr plusExpr);
    }

    public interface Visitable { void Accept(Visitor v); }

    public interface Stmnt: Visitable { }

    public interface Expr: Visitable { }

    public record FuncCall(string Id, FSharpList<Expr> Args) : Stmnt {
        public static Stmnt Of(string id, FSharpList<Expr> args) => new FuncCall(id, args);
        public void Accept(Visitor v) => v.Visit(this);
    }

    public record VarDecl(Type Type, string Id, Expr Value) : Stmnt {
        public static Stmnt Of(Type type, string id, Expr value) => new VarDecl(type, id, value);
        public void Accept(Visitor v) => v.Visit(this);
        public override string ToString() => $"{Type.ToString().ToLower()} variable '{Id}'";
    }

    public record StringLiteral(string Value) : Expr {
        public static Expr Of(string value) => new StringLiteral(value);
        public void Accept(Visitor v) => v.Visit(this);
    }

    public record IntLiteral(int Value) : Expr {
        public static Expr Of(int value) => new IntLiteral(value);
        public void Accept(Visitor v) => v.Visit(this);
    }

    public record FloatLiteral(double Value) : Expr {
        public static Expr Of(double value) => new FloatLiteral(value);
        public void Accept(Visitor v) => v.Visit(this);
    }

    public record VarRef(string Id) : Expr {
        public static Expr Of(string id) => new VarRef(id);
        public void Accept(Visitor v) => v.Visit(this);
    }

    public record PlusExpr(Expr Left, Expr Right) : Expr {
        public static Expr Of(Expr left, Expr right) => new PlusExpr(left, right);
        public void Accept(Visitor v) => v.Visit(this);
    }
}

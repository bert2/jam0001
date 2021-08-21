﻿namespace foobar {
    using Microsoft.FSharp.Collections;

    public interface Visitor {
        void Visit(FuncCall x);
        void Visit(VarDecl x);
        void Visit(StringLiteral x);
        void Visit(IntLiteral x);
        void Visit(FloatLiteral x);
        void Visit(VarRef x);
        void Visit(AddExpr x);
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

    public record AddExpr(Expr Left, Expr Right) : Expr {
        public static Expr Of(Expr left, Expr right) => new AddExpr(left, right);
        public void Accept(Visitor v) => v.Visit(this);
    }
}

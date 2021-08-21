namespace foobar {
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;

    public enum Type { Void, Int, Float, String }

    public record Param(string Id, Type Type);

    public record Var(string Id, Value Value);

    public record Value(Type Type, object? RawVal) {
        public static readonly Value Void = new(Type.Void, null);

        public string StringVal => RawVal as string ?? throw new Exception($"The {this} is not a string.");

        public int IntVal => RawVal as int? ?? throw new Exception($"The {this} is not an int.");

        public double FloatVal => RawVal as double? ?? throw new Exception($"The {this} is not a float.");

        public override string ToString() {
            string PrintType() => Type.ToString().ToLower();
            string PrintVal() => Type switch { Type.Void => "", Type.String => $" '{RawVal}'", _ => $" {RawVal}" };
            return $"{PrintType()} value{PrintVal()}";
        }
    }
    public record Func(string Id, Param[] Params, Func<IEnumerable<Value>, Value> Execute);

    public record RTE(ImmutableDictionary<string, Var> Vars, ImmutableDictionary<string, Func> Funcs, TextWriter StdOut) {
        public static RTE WithPrelude(TextWriter stdout) => new(
            Vars: ImmutableDictionary<string, Var>.Empty,
            Funcs: ImmutableDictionary<string, Func>.Empty
                .Add("println", new Func("println", new[] { new Param("s", Type.String) }, args => {
                    stdout.WriteLine(args.Single().RawVal);
                    return Value.Void;
                })),
            stdout);

        public RTE Add(Var v) => this with { Vars = Vars.Add(v.Id, v) };

        public RTE Add(Func f) => this with { Funcs = Funcs.Add(f.Id, f) };
    }
}

namespace foobar {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Vars = System.Collections.Immutable.ImmutableDictionary<string, Var>;
    using Funcs = System.Collections.Immutable.ImmutableDictionary<string, Func>;

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

    public record RTE(Vars Vars, Funcs Funcs, TextWriter StdOut) {
        public static RTE WithPrelude(TextWriter stdout) => new RTE(Vars.Empty, Funcs.Empty, stdout)
            .Add(new Func("println", Array.Empty<Param>(), args => {
                stdout.WriteLine(args.Single().RawVal);
                return Value.Void;
            }));

        public RTE Add(Var v) => this with { Vars = Vars.Add(v.Id, v) };

        public RTE Add(Func f) => this with { Funcs = Funcs.Add(f.Id, f) };
    }
}

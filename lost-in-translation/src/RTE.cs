namespace foobar {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Vars = System.Collections.Immutable.ImmutableDictionary<string, Var>;
    using Funcs = System.Collections.Immutable.ImmutableDictionary<string, Func>;

    public enum Type { Void, Comment, Int, Float, String }

    public record Param(string Id, Type Type);

    public record Var(string Id, Value Value);

    public record Func(string Id, Param[] Params, Func<IEnumerable<Value>, Value> Execute);

    public record Value(Type Type, object? Raw) {
        public static readonly Value Void = new(Type.Void, null);

        public string String => Raw as string ?? throw new Exception($"The {this} is not a string.");

        public int Int => Raw as int? ?? throw new Exception($"The {this} is not an int.");

        public double Float => Raw as double? ?? throw new Exception($"The {this} is not a float.");

        public override string ToString() {
            string PrintType() => Type.ToString().ToLower();
            string PrintVal() => Type switch { Type.Void => "", Type.String => $" '{Raw}'", Type.Comment => $" '{Raw}'", _ => $" {Raw}" };
            return $"{PrintType()} value{PrintVal()}";
        }
    }

    public record RTE(Vars Vars, Funcs Funcs, TextWriter StdOut) {
        public static RTE WithPrelude(TextWriter stdout) => new RTE(Vars.Empty, Funcs.Empty, stdout)
            .Add(new Func("println", Array.Empty<Param>(), args => {
                stdout.WriteLine(args.Single().Raw);
                return Value.Void;
            }));

        public RTE Add(Var v) => this with { Vars = Vars.Add(v.Id, v) };

        public RTE Add(Func f) => this with { Funcs = Funcs.Add(f.Id, f) };
    }
}

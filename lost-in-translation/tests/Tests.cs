#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1018 // Add accessibility modifiers (or vice versa).

namespace tests {
    using FluentAssertions;
    using FParsec.CSharp;
    using Xunit;

    using foobar;

    using static foobar.Parser;

    public class Tests {
        [Fact] void HelloWorld() =>
            Parse("println 'hello world'").GetResult()
            .Should().ContainSingle()
            .Which.Should().BeOfType<FuncCall>().Which.Should()
                .Satisfy<FuncCall>(x => x.Id.Should().Be("println")).And
                .Satisfy<FuncCall>(x => x.Args
                    .Should().ContainSingle()
                    .Which.Should().BeOfType<StringLiteral>()
                    .Which.Value.Should().Be("hello world"));

        [Fact] void StringVar() =>
            Parse("string str = 'test'").GetResult()
            .Should().ContainSingle()
            .Which.Should().BeOfType<VarDecl>().Which.Should()
                .Satisfy<VarDecl>(x => x.Type.Should().Be(Type.String)).And
                .Satisfy<VarDecl>(x => x.Id.Should().Be("str")).And
                .Satisfy<VarDecl>(x => x.Value
                    .Should().BeOfType<StringLiteral>()
                    .Which.Value.Should().Be("test"));

        [Fact] void IntVar() =>
            Parse("int num = 123").GetResult()
            .Should().ContainSingle()
            .Which.Should().BeOfType<VarDecl>().Which.Should()
                .Satisfy<VarDecl>(x => x.Type.Should().Be(Type.Int)).And
                .Satisfy<VarDecl>(x => x.Id.Should().Be("num")).And
                .Satisfy<VarDecl>(x => x.Value
                    .Should().BeOfType<IntLiteral>()
                    .Which.Value.Should().Be(123));

        [Fact] void FloatVar() =>
            Parse("float num = 123.456").GetResult()
            .Should().ContainSingle()
            .Which.Should().BeOfType<VarDecl>().Which.Should()
                .Satisfy<VarDecl>(x => x.Type.Should().Be(Type.Float)).And
                .Satisfy<VarDecl>(x => x.Id.Should().Be("num")).And
                .Satisfy<VarDecl>(x => x.Value
                    .Should().BeOfType<FloatLiteral>()
                    .Which.Value.Should().Be(123.456));

        [Fact] void VariableAssignmentAndUsage() =>
            Parse(
                "string msg = 'hello world'\n" +
                "println msg").GetResult()
            .Should().SatisfyRespectively(
                ln1 => ln1.Should().BeOfType<VarDecl>().Which.Should()
                    .Satisfy<VarDecl>(x => x.Type.Should().Be(Type.String)).And
                    .Satisfy<VarDecl>(x => x.Id.Should().Be("msg")).And
                    .Satisfy<VarDecl>(x => x.Value
                        .Should().BeOfType<StringLiteral>()
                        .Which.Value.Should().Be("hello world")),
                ln2 => ln2.Should().BeOfType<FuncCall>().Which.Should()
                    .Satisfy<FuncCall>(x => x.Id.Should().Be("println")).And
                    .Satisfy<FuncCall>(x => x.Args
                        .Should().ContainSingle()
                        .Which.Should().BeOfType<VarRef>()
                        .Which.Id.Should().Be("msg")));
    }
}

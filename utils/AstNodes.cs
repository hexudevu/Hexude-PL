using System;
using Utils;
namespace AstNodes {
    abstract class AstNode {    public abstract override string ToString(); }
    class IntNode : AstNode {
        public int Value { get; }
        public IntNode(int value) => Value = value;
        public override string ToString() => Value.ToString();
    }
    class FloatNode : AstNode {
        public double Value { get; }
        public FloatNode(double value) => Value = value;
        public override string ToString() => Value.ToString();
    }
    class BoolNode : AstNode {
        public bool Value { get; }
        public BoolNode(bool value) => Value = value;
        public override string ToString() => Value.ToString();
    }
    class StringNode : AstNode {
        public string Value { get; }
        public StringNode(string value) => Value = value;
        public override string ToString() => Value;
    }
    class NullNode : AstNode {
        public override string ToString() => "null";
    }
    class VariableNode : AstNode {
        public string Name { get; }
        public VariableNode(string name) => Name = name;
        public override string ToString() => Name;
    }
    class BinaryOpNode : AstNode {
        public string Operator { get; }
        public AstNode Left { get; }
        public AstNode Right { get; }
        public BinaryOpNode(string op, AstNode left, AstNode right) {
            Operator = op;
            Left = left;
            Right = right;
        }
        public override string ToString() => $"({Left} {Operator} {Right})";
    }
}
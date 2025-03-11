using System;
using System.Collections.Generic;
using Utils;
namespace Utils {
    public enum UnaryNotation { Prefix, Postfix }
}
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
    class FunctionArgument<Type> : AstNode {
        public Type Value { get; }
        public FunctionArgument(Type value) => Value = value;
        public override string ToString() => Value?.ToString() ?? "null";
    }
    class FunctionCallNode : AstNode {
        public string Name { get; }
        public AstNode[] Arguments { get; }
        public FunctionCallNode(string name, AstNode[] arguments) {
            Name = name;
            Arguments = arguments;
        }
        public override string ToString() {
            string returning = Name;
            string argsToString = "";
            foreach (AstNode arg in Arguments) {
                argsToString+=arg.ToString()+", ";
            }
            argsToString.Substring(0, argsToString.Length-2);
            returning += $"({argsToString})";
            return returning;
        }
    }
    //class ConditionNode : AstNode {
        //public string Condition;
        //public AstNode Left, Right;
    //}
    class IfNode : AstNode {
        public AstNode Condition;
        public AstNode ThenBranch;
        public AstNode ElseBranch;
        public IfNode(AstNode condition, AstNode thenBranch, AstNode elseBranch = null) 
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
        public override string ToString() => ElseBranch != null
            ? $"if ({Condition}) {{ {ThenBranch} }} else {{ {ElseBranch} }}"
            : $"if ({Condition}) {{ {ThenBranch} }}";
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
    class UnaryOpNode : AstNode {
        public AstNode Operand { get; }
        public string Operator { get; }
        public UnaryNotation Notation { get; }
        public UnaryOpNode(AstNode operand, string op, UnaryNotation notation) {
            Operand = operand;
            Operator = op;
            Notation = notation;
        } 
        public override string ToString() {
            string returning;
            if (Notation == UnaryNotation.Prefix)
                returning = Operator + Operand.ToString();
            else
                returning = Operand.ToString() + Operator;
            return returning;
        }
    }
}
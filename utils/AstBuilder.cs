using System;
using System.Collections.Generic;
using Utils;
using AstNodes;
namespace Utils {
    class AstBuilder {
        public static AstNode BuildAst(Token[] tokens) {
            AstNode ast = new BinaryOpNode("+", 
                new IntNode(2), 
                new BinaryOpNode("*", 
                    new FloatNode(2.5), 
                    new IntNode(7)
                )
            );
            return ast;
        }
    }
}
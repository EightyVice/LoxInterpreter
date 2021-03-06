using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal interface IExpressionVisitor<T>
	{
		T VisitBinary(BinaryExpression binaryExpression);
		T VisitUnary(UnaryExpression unaryExpression);
		T VisitLiteral(LiteralExpression literalExpression);
		T VisitGrouping(GroupingExpression groupingExpression);
		T VisitVariable(VariableExpresion variableExpresion);
		T VisitAssignment(AssignmentExpression assignmentExpression);
		T VisitLogical(LogicalExpression logicalExpression);
		T VisitFunctionalCall(CallExpression callExpression);
	}

	internal abstract class Expression
	{
		public abstract T Accept<T>(IExpressionVisitor<T> visitor);
	}


	internal class BinaryExpression : Expression
	{
		public readonly Expression Left;
		public readonly Token Operator;
		public readonly Expression Right;

		public BinaryExpression(Expression Left, Token Operator, Expression Right)
		{
			this.Left = Left;
			this.Operator = Operator;
			this.Right = Right;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitBinary(this);
		}
	}

	internal class LiteralExpression : Expression
	{
		public readonly object Literal;
		public LiteralExpression(object literal)
		{
			Literal = literal;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitLiteral(this);
		}
	}

	internal class GroupingExpression : Expression
	{
		public Expression innerExpression;

		public GroupingExpression(Expression expr)
		{
			innerExpression = expr;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitGrouping(this);
		}
	}

	internal class UnaryExpression : Expression
	{
		public readonly Token Operator;
		public readonly Expression Operand;

		public UnaryExpression(Token op, Expression operand)
		{
			Operator = op;
			Operand = operand;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitUnary(this);
		}
	}


	internal class CallExpression : Expression
	{
		public readonly Expression Callee;
		public readonly Token Parenthesis;
		public readonly List<Expression> Arguments;

		public CallExpression(Expression callee, Token parenthesis, List<Expression> arguments)
		{
			Callee = callee;
			Parenthesis = parenthesis;
			Arguments = arguments;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitFunctionalCall(this);
		}
	}

	internal class VariableExpresion : Expression
	{
		public Token vartoken;

		public VariableExpresion(Token token)
		{
			vartoken = token;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitVariable(this);
		}
	}

	internal class AssignmentExpression : Expression
	{
		public readonly Token lhs;
		public readonly Expression value;

		public AssignmentExpression(Token lhs, Expression value)
		{
			this.lhs = lhs;
			this.value = value;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitAssignment(this);
		}
	}

	
	internal class LogicalExpression : Expression
	{
		public readonly Expression left;
		public readonly Token op;
		public readonly Expression right;

		public LogicalExpression(Expression lhs, Token Operator, Expression rhs)
		{
			left = lhs;
			right = rhs;
			op = Operator;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor)
		{
			return visitor.VisitLogical(this);
		}
	}

}

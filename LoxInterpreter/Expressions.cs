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
}

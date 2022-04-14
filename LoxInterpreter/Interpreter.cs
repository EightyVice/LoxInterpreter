using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal class Interpreter : IExpressionVisitor<object>, IStatementVisitor
	{
		private readonly Environment environment;

		public Interpreter()
		{
			environment = new Environment();
		}

		private object Evaluate(Expression expression)
		{
			return expression.Accept(this);
		}

		public void Interpret(List<Statement> statements)
		{
			foreach (Statement statement in statements)
			{
				statement.Accept(this);
			}
		}
		public object VisitBinary(BinaryExpression binaryExpression)
		{
			object left  = Evaluate(binaryExpression.Left);
			object right = Evaluate(binaryExpression.Right);

			switch (binaryExpression.Operator.Type)
			{
				case TokenType.BangEqaul: return !isEqual(left, right);
				case TokenType.EqualEqual: return isEqual(left, right);
				case TokenType.Greater:
					return (double)left > (double)right;
				case TokenType.GreaterEqual:
					return (double)left >= (double)right;
				case TokenType.Smaller:
					return (double)left < (double)right;
				case TokenType.SmallerEqual:
					return (double)left <= (double)right;
				case TokenType.Minus:
					return (double)left - (double)right;
				case TokenType.Slash:
					return (double)left / (double)right;
				case TokenType.Asterisk:
					return (double)left * (double)right;
				case TokenType.Plus:
					if(left is double && right is double) return (double)left + (double)right;
					if(left is string && right is string) return (string)left + (string)right;
					break;
			}
			return null;
		}

		private bool isEqual(object a, object b)
		{
			return a == b;
		}

		public object VisitGrouping(GroupingExpression groupingExpression)
		{
			return Evaluate(groupingExpression.innerExpression);
		}

		public object VisitLiteral(LiteralExpression literalExpression)
		{
			return literalExpression.Literal;
		}

		public object VisitUnary(UnaryExpression unaryExpression)
		{
			object operand = Evaluate(unaryExpression.Operand);

			switch (unaryExpression.Operator.Type)
			{
				case TokenType.Minus:
					return -(double)operand;
				case TokenType.Bang:
					return !isTruthy(operand);
			}
			return null;
		}

		private bool isTruthy(object operand)
		{
			if (operand == null) return false;
			if (operand is bool) return (bool)operand;
			return true;
		}

		public void VisitExpressionStatement(ExpressionStatement statement)
		{
			object value = Evaluate(statement.expression);
			Console.WriteLine(value);
		}

		public void VisitPrintStatement(PrintStatement printStatement)
		{
			object value = Evaluate(printStatement.expression);
			Console.WriteLine(value);
		}

		public void VisitVarDeclarationStatement(VarDeclarationStatement varStatement)
		{
			environment.Define(varStatement.name, Evaluate(varStatement.initializer));
		}

		public object VisitVariable(VariableExpresion variableExpresion)
		{
			return environment.Get(variableExpresion.vartoken);
		}
	}
}

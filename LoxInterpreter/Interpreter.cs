using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal class Interpreter : IExpressionVisitor<object>, IStatementVisitor
	{
		public readonly Environment globals = new Environment();
		private Environment environment;

		private class Globals
		{
			public class Clock : LoxCallable
			{
				int LoxCallable.Arity { get => 0; }

				object LoxCallable.Call(Interpreter interpreter, List<object> arguments)
				{
					return (double)DateTimeOffset.Now.ToUnixTimeSeconds();
				}
			}

			public class Flush : LoxCallable
			{
				int LoxCallable.Arity { get => 0; }

				object LoxCallable.Call(Interpreter interpreter, List<object> arguments)
				{
					Console.Clear();
					return null;
				}
			}

			public class Input : LoxCallable
			{
				int LoxCallable.Arity { get => 0; }

				object LoxCallable.Call(Interpreter interpreter, List<object> arguments)
				{
					return Console.ReadLine();
				}
			}
		}
		public Interpreter()
		{
			globals.Define("clock", new Globals.Clock());
			globals.Define("flush", new Globals.Flush());
			globals.Define("input", new Globals.Input());

			environment = globals;
		}



		private object Evaluate(Expression expression)
		{
			if (expression == null)
				return null;
			else
				return expression.Accept(this);
		}

		public void Interpret(List<Statement> statements)
		{
			foreach (Statement statement in statements)
			{
				statement.Accept(this);
			}
		}

		private void Execute(Statement statement)
		{
			statement.Accept(this);
		}

		private void castBinary(object lhs, object rhs)
		{
			if (lhs.GetType() == rhs.GetType())
				return;

			if (lhs is string && rhs is not string)
			{
				rhs = Convert.ToString(rhs);
			}
			else
			{
				if(lhs is double && rhs is not double)
				{
					rhs = Convert.ToDouble(rhs);
				}
			}
			
		}
		public object VisitBinary(BinaryExpression binaryExpression)
		{
			object left  = Evaluate(binaryExpression.Left);
			object right = Evaluate(binaryExpression.Right);
			castBinary(left, right);

			switch (binaryExpression.Operator.Type)
			{
				case TokenType.BangEqaul: return !isEqual(left, right);
				case TokenType.EqualEqual: return isEqual(left, right);
				case TokenType.Greater:
					return Convert.ToDouble(left) > Convert.ToDouble(right);
				case TokenType.GreaterEqual:
					return Convert.ToDouble(left) >= Convert.ToDouble(right);
				case TokenType.Smaller:
					return Convert.ToDouble(left) < Convert.ToDouble(right);
				case TokenType.SmallerEqual:
					return Convert.ToDouble(left) <= Convert.ToDouble(right);
				case TokenType.Minus:
					return Convert.ToDouble(left) - Convert.ToDouble(right);
				case TokenType.Slash:
					return Convert.ToDouble(left) / Convert.ToDouble(right);
				case TokenType.Asterisk:
					return Convert.ToDouble(left) * Convert.ToDouble(right);
				case TokenType.Plus:
					if(left is double && right is double) return (double)left + (double)right;
					if(left is string && right is string) return (string)left + (string)right;
					break;
			}
			return null;
		}

		private bool isEqual(object a, object b)
		{
			if(a == b) return true;
			return a.Equals(b);
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
			//Console.WriteLine(value ?? "Nil");
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

		public object VisitAssignment(AssignmentExpression assignmentExpression)
		{
			object value = Evaluate(assignmentExpression.value);
			environment.Assign(assignmentExpression.lhs, value);
			return value;
		}

		public void visitBlockStatement(BlockStatement blockStatement)
		{
			ExecuteBlock(blockStatement.statements, new Environment(environment));
		}

		public void ExecuteBlock(List<Statement> statements, Environment environment)
		{
			Environment previous = this.environment;

			try
			{
				this.environment = environment;

				foreach (var stmt in statements)
				{
					stmt.Accept(this);
				}
			}
			finally
			{
				this.environment = previous;
			}
		}

		public void VisitIfStatement(IfStatement ifStatement)
		{
			if (isTruthy(Evaluate(ifStatement.condition)))
				Execute(ifStatement.thenBranch);
			else if(ifStatement.elseBranch != null)
				Execute(ifStatement.elseBranch);
			else
				return;
		}

		public object VisitLogical(LogicalExpression logicalExpression)
		{
			object left = Evaluate(logicalExpression.left);

			if (logicalExpression.op.Type == TokenType.Or)
			{
				if (isTruthy(left)) return left;
			}
			else if (logicalExpression.op.Type == TokenType.And)
			{
				if (!isTruthy(left)) return left;
			}

			return Evaluate(logicalExpression.right);
		}

		public void visitWhileStatement(WhileStatement whileStatement)
		{
			while (isTruthy(Evaluate(whileStatement.condition)))
			{
				Execute(whileStatement.body);
			}
			return;
		}

		public object VisitFunctionalCall(CallExpression callExpression)
		{
			object callee = Evaluate(callExpression.Callee);
			List<object> arguments = new List<object>();
			foreach(Expression argument in callExpression.Arguments)
			{
				arguments.Add(Evaluate(argument));
			}

			if (callee is not LoxCallable)
				throw new LoxRunTimeException("Invalid call invocation.", callExpression.Parenthesis);

			LoxCallable function = (LoxCallable)callee;
			if (arguments.Count != function.Arity)
				throw new LoxRunTimeException($"Expected {function.Arity} arguments but got {arguments.Count}.");

			return function.Call(this, arguments);
		}

		public void visitFunctionStatement(FunctionStatement functionStatement)
		{
			LoxFunction function = new LoxFunction(functionStatement, environment);
			environment.Define(functionStatement.Name.Lexeme, function);
		}

		public void visitReturnStatement(ReturnStatement returnStatement)
		{
			object value = null;
			if (returnStatement.Value != null) value = Evaluate(returnStatement.Value);
			throw new ReturnInvocation(value);
		}
	}
}

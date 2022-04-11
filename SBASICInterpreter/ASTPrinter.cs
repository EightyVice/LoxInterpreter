using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal class ASTPrinter : IExpressionVisitor<string>
	{
		public void Print(Expression expression)
		{
			expression.Accept(this);
		}
		public string VisitBinary(BinaryExpression binaryExpression)
		{
			throw new NotImplementedException();
		}

		public string VisitGrouping(GroupingExpression groupingExpression)
		{
			throw new NotImplementedException();
		}

		public string VisitLiteral(LiteralExpression literalExpression)
		{
			throw new NotImplementedException();
		}

		public string VisitUnary(UnaryExpression unaryExpression)
		{
			throw new NotImplementedException();
		}
	}
}

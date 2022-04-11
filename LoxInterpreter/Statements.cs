using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal interface IStatementVisitor<T>
	{

	}

	internal abstract class Statement
	{
		public abstract T Accept<T>(IStatementVisitor<T> visitor);
	}

	internal class PrintStatement : Statement
	{
		private readonly Expression expr;

		public PrintStatement(Expression expr)
		{
			this.expr = expr;
		}

		public override T Accept<T>(IStatementVisitor<T> visitor)
		{
			throw new NotImplementedException();
		}
	}

	internal class ExpressionStatement : Statement
	{
		private Expression expr;

		public ExpressionStatement(Expression expr)
		{
			this.expr = expr;
		}

		public override T Accept<T>(IStatementVisitor<T> visitor)
		{
			throw new NotImplementedException();
		}
	}
}

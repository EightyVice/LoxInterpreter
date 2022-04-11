using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal interface IStatementVisitor
	{
		void VisitExpressionStatement(ExpressionStatement statement);
		void VisitPrintStatement(PrintStatement printStatement);
	}

	internal abstract class Statement
	{
		public abstract void Accept(IStatementVisitor visitor);
	}

	internal class PrintStatement : Statement
	{
		public readonly Expression expression;

		public PrintStatement(Expression expr)
		{
			this.expression = expr;
		}

		public override void Accept(IStatementVisitor visitor)
		{
			visitor.VisitPrintStatement(this);
		}
	}

	internal class ExpressionStatement : Statement
	{
		public readonly Expression expression;

		public ExpressionStatement(Expression expr)
		{
			this.expression = expr;
		}

		public override void Accept(IStatementVisitor visitor)
		{
			visitor.VisitExpressionStatement(this);
		}
	}
}

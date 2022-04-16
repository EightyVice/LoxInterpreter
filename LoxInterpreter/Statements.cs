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
		void VisitVarDeclarationStatement(VarDeclarationStatement varDeclarationStatement);
		void visitBlockStatement(BlockStatement blockStatement);
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

	internal class VarDeclarationStatement : Statement
	{
		public readonly string name;
		public readonly Expression initializer;

		public VarDeclarationStatement(string varname, Expression init)
		{
			name = varname;
			initializer = init;
		}

		public override void Accept(IStatementVisitor visitor)
		{
			visitor.VisitVarDeclarationStatement(this);
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

	internal class BlockStatement : Statement
	{
		public readonly List<Statement> statements;

		public BlockStatement(List<Statement> innerStatements)
		{
			statements = innerStatements;
		}
		public override void Accept(IStatementVisitor visitor)
		{
			visitor.visitBlockStatement(this);
		}
	}
}

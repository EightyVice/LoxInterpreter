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
		void VisitIfStatement(IfStatement ifStatement);
		void visitWhileStatement(WhileStatement whileStatement);
		void visitFunctionStatement(FunctionStatement functionStatement);
		void visitReturnStatement(ReturnStatement returnStatement);

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

	internal class IfStatement : Statement
	{
		public readonly Expression condition;
		public readonly Statement thenBranch;
		public readonly Statement elseBranch;

		public IfStatement(Expression condition, Statement thenBranch, Statement elseBranch)
		{
			this.condition = condition;
			this.thenBranch = thenBranch;
			this.elseBranch = elseBranch;
		}

		public override void Accept(IStatementVisitor visitor)
		{
			visitor.VisitIfStatement(this);
		}
	}

	internal class WhileStatement : Statement
	{
		public readonly Expression condition;
		public readonly Statement body;

		public WhileStatement(Expression condition, Statement body)
		{
			this.condition = condition;
			this.body = body;
		}
		public override void Accept(IStatementVisitor visitor)
		{
			visitor.visitWhileStatement(this);
		}
	}

	internal class FunctionStatement : Statement
	{
		public readonly Token Name;
		public readonly List<Token> Parameters;
		public readonly List<Statement> Body;

		public FunctionStatement(Token name, List<Token> parameters, List<Statement> body)
		{
			Name = name;
			Parameters = parameters;
			Body = body;
		}

		public override void Accept(IStatementVisitor visitor)
		{
			visitor.visitFunctionStatement(this);
		}
	}

	internal class ReturnStatement : Statement
	{
		public readonly Token Keyword;
		public readonly Expression Value;

		public ReturnStatement(Token keyword, Expression value)
		{
			Keyword = keyword;
			Value = value;
		}

		public override void Accept(IStatementVisitor visitor)
		{
			visitor.visitReturnStatement(this);
		}
	}
}

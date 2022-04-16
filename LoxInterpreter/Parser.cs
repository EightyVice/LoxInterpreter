/*
	Grammar in Bakus-Naur Notation
	==============================
	declaration    → classDecl
				   | funDecl
				   | varDecl
				   | statement ;

	classDecl      → "class" IDENTIFIER ( "<" IDENTIFIER )?
					 "{" function* "}" ;
	funDecl        → "fun" function ;
	varDecl        → "var" IDENTIFIER ( "=" expression )? ";" ;				
	statement  : exprStmt
					| printStmt ;
	exprStmt   : expression ";" ;
	printStmt  : "print" expression ";" ;
	expression : assignemt;
	assignment : IDENTIFIER "=" ASSIGNMENT
	equality   : comparison ( ( "!=" | "==" ) comparison )* ;
	comparison : term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
	term       : factor ( ( "-" | "+" ) factor )* ;
	factor     : unary ( ( "/" | "*" ) unary )* ;
	unary      : ( "!" | "-" ) unary
			   | primary ;
	primary    : NUMBER | STRING | "true" | "false" | "nil"
			   | "(" expression ")" ;		
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{

	/// <summary>
	/// A Manual LL(1) Recursive-Descent Parser
	/// </summary>
	internal class Parser
	{
		private List<Token> tokens;
		private int position = 0;
		private Token Peek()
		{
			return tokens[position];
		}

		
		private bool AtEnd()
		{
			return Peek().Type == TokenType.EOF;
		}

		private bool Check(TokenType type)
		{
			if (AtEnd()) return false;
			return Peek().Type == type;
		}
		private bool Match(params TokenType[] tokenTypes)
		{
			foreach(TokenType type in tokenTypes)
			{
				if (Check(type))
				{
					Advance();
					return true;
				}
			}
			return false;
		}

		private Token Advance()
		{
			if(!AtEnd()) position++;
			return Previous();
		}

		private Token Consume(TokenType type, string message)
		{
			if (Check(type)) return Advance();
			throw new LoxRunTimeException(message, Advance()); 
		}
		private Token Previous()
		{
			return tokens[position - 1];
		}

		private List<Statement> ParseBlock() {
			List<Statement> statements = new List<Statement>();

			while (!Check(TokenType.RightBrace) && !AtEnd())
			{
				statements.Add(ParseDeclaration());
			}
			Consume(TokenType.RightBrace, "Expect '}' after block");
			return statements;
		}
		public List<Statement> Parse(List<Token> Tokens)
		{


			tokens = Tokens;
			List<Statement> result = new List<Statement>();
			position = 0;

			while (!AtEnd())
			{
				result.Add(ParseDeclaration());
			}

			return result;
		}

		private Statement ParseDeclaration()
		{
			if (Match(TokenType.Var)) return ParseVariableDeclaration();
			return ParseStatement();
		}

		private Statement ParseVariableDeclaration()
		{
			Token name = Consume(TokenType.Identifier, "Expect a variable name.");

			Expression initializer = null;
			if (Match(TokenType.Equal))
				initializer = ParseExpression();
			else
				initializer = new LiteralExpression(null);

			Consume(TokenType.Semicolon, "Expect a ';' after variable declaration.");

			return new VarDeclarationStatement(name.Lexeme, initializer);
		}

		private Statement ParseStatement()
		{
			if (Match(TokenType.Print)) return ParsePrintStatement();
			if (Match(TokenType.LeftBrace)) return ParseBlockStatement();
			if (Match(TokenType.If)) return ParseIfStatement();
			if (Match(TokenType.While)) return ParseWhileStatement();
			return ParseExpresionStatement();
		}

		private Statement ParseWhileStatement()
		{
			Consume(TokenType.LeftParenthesis, "Expect '(' after 'while'.");
			Expression expression = ParseExpression();
			Consume(TokenType.RightParenthesis, "Expect ')' after condition.");
			Statement body = ParseStatement();
			return new WhileStatement(expression, body);
		}

		private Statement ParseIfStatement()
		{
			Consume(TokenType.LeftParenthesis, "Expect '(' after 'if'.");
			Expression condition = ParseExpression();
			Consume(TokenType.RightParenthesis, "Expect ')' after if condition.");

			Statement thenBranch = ParseStatement();
			Statement elseBranch = null;

			if(Match(TokenType.Else))
				elseBranch = ParseStatement();

			return new IfStatement(condition, thenBranch, elseBranch);
		}

		private Statement ParseBlockStatement()
		{
			return new BlockStatement(ParseBlock());
		}

		private Statement ParseExpresionStatement()
		{
			Expression expr = ParseExpression();
			Consume(TokenType.Semicolon, "Expect ';' after expression.");
			return new ExpressionStatement(expr);
		}

		private Statement ParsePrintStatement()
		{
			Consume(TokenType.LeftParenthesis, "Expect '('.");
			Expression expr = ParseExpression();
			Consume(TokenType.RightParenthesis, "Expect ')'.");
			Consume(TokenType.Semicolon, "Expect ';' after value.");
			return new PrintStatement(expr);
		}

		private Expression ParseExpression()
		{
			return ParseAssignment();
		}

		private Expression ParseAssignment()
		{
			Expression expression = ParseOR();

			if (Match(TokenType.Equal))
			{
				Token lhs = Previous();
				Expression rhs = ParseAssignment();

				if(expression is VariableExpresion)
				{
					Token name = ((VariableExpresion)expression).vartoken;
					return new AssignmentExpression(name, rhs);
				}

				throw new LoxRunTimeException("Invalid Assignment target.", lhs);
			}
			return expression;
		}

		private Expression ParseOR()
		{
			Expression expression = ParseAND();

			while (Match(TokenType.Or))
			{
				Token op = Previous();
				Expression right = ParseAND();
				expression = new LogicalExpression(expression, op, right);
			}

			return expression;
		}

		private Expression ParseAND()
		{
			Expression expression = ParseEquality();

			while (Match(TokenType.And))
			{
				Token op = Previous();
				Expression right = ParseEquality();
				expression = new LogicalExpression(expression, op, right);
			}
			
			return expression;
		}

		private Expression ParseEquality()
		{
			Expression expr = ParseComparison();

			while(Match(TokenType.BangEqaul, TokenType.EqualEqual))
			{
				Token op = Previous();
				Expression right = ParseComparison();
				expr = new BinaryExpression(expr, op, right);
			}
			return expr;
		}

		private Expression ParseComparison()
		{
			Expression expr = ParseTerm();

			while(Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Smaller, TokenType.SmallerEqual))
			{
				Token op = Previous();
				Expression right = ParseTerm();
				expr = new BinaryExpression(expr, op, right);
			}
			return expr;
		}


		private Expression ParseTerm()
		{
			Expression expr = ParseFactor();

			while (Match(TokenType.Plus, TokenType.Minus))
			{
				Token op = Previous();
				Expression right = ParseFactor();
				expr = new BinaryExpression(expr, op, right);
			}
			return expr;
		}

		private Expression ParseFactor()
		{
			Expression expr = ParseUnary();

			while (Match(TokenType.Slash, TokenType.Asterisk))
			{
				Token op = Previous();
				Expression right = ParseUnary();
				expr = new BinaryExpression(expr, op, right);
			}
			return expr;
		}

		private Expression ParseUnary()
		{
			if(Match(TokenType.Bang, TokenType.Minus))
			{
				Token op = Previous();
				Expression operand = ParseUnary();
				return new UnaryExpression(op, operand);
			}
			return ParsePrimary();
		}

		private Expression ParsePrimary()
		{
			if (Match(TokenType.False)) return new LiteralExpression(false);
			if (Match(TokenType.True)) return new LiteralExpression(true);
			if (Match(TokenType.Nil)) return new LiteralExpression(null);

			if (Match(TokenType.Number, TokenType.String)) return new LiteralExpression(Previous().Literal);

			if (Match(TokenType.Identifier)) return new VariableExpresion(Previous());
			if (Match(TokenType.LeftParenthesis))
			{
				Expression expr = ParseExpression();
				Advance();
				return new GroupingExpression(expr);
			}
			return null;
		}
	}
}

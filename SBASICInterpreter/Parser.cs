
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

		private Token Previous()
		{
			return tokens[position - 1];
		}

		public Expression Parse(List<Token> Tokens)
		{
			/*
				Grammar in Bakus-Naur Notation
				==============================

				expression : equality ;
				equality   : comparison ( ( "!=" | "==" ) comparison )* ;
				comparison : term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
				term       : factor ( ( "-" | "+" ) factor )* ;
				factor     : unary ( ( "/" | "*" ) unary )* ;
				unary      : ( "!" | "-" ) unary
				           | primary ;
				primary    : NUMBER | STRING | "true" | "false" | "nil"
				           | "(" expression ")" ;		
			 */

			tokens = Tokens;
			Expression result = null;
			position = 0;

			while (!AtEnd())
			{
				result = ParseExpression();
			}

			return result;
		}

		private Expression ParseExpression()
		{
			return ParseEquality();
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

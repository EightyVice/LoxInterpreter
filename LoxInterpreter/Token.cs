using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal enum TokenType
	{
		// ( ) , ; 
		LeftParenthesis, RightParenthesis, RightBrace, LeftBrace,
		Comma, Dot, Semicolon, Colon, Asterisk, Slash, Minus, Plus,

		Bang, BangEqaul,
		Equal, EqualEqual,
		Smaller, SmallerEqual,
		Greater, GreaterEqual,


		Identifier, String, Number,

		// Keywords
		And, Class, Else, False, Fun, For, If, Nil, Or,
		Print, Return, Super, This, True, Var, While,

		EOF,
	}

	internal class Token
	{
		public TokenType Type { get; }
		public object? Literal { get; }
		public string Lexeme { get; }
		public long Position { get; }

		public Token(TokenType type, object? literal, string lexeme, long position)
		{
			Type = type;
			Literal = literal;
			Lexeme = lexeme;
			Position = position;
		}

		public override string ToString()
		{
			return $"{Type}: [{Lexeme}]";
		}
	}
}

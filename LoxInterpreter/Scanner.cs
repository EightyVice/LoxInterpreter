using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal class Scanner
	{
		public bool HasError { get;}

	 
		private List<Token> tokens;
		
		private string text;
		public int Position { get; private set; }


		public char Peek()
		{
			if (Position < text.Length)
				return text[Position];
			else
				return '\0';
		}

		public char PeekNext()
		{
			if (Position + 1 != text.Length)
				return text[Position + 1];
			else
				return '\0';
		}
		public char GetChar()
		{
			if (Position < text.Length)
			{
				char ret = text[Position];
				Position++;
				return ret;
			}
			else throw new EndOfStreamException();
		}

		public void Step()
		{
			Position++;
		}

		public string ReadIdentifier()
		{

			int start = Position - 1;
			while (char.IsLetterOrDigit(Peek())) Step();

			return text.Substring(start, Position - start);
		}
		public double ReadNumber()
		{
			int start = Position - 1;
			while (char.IsNumber(Peek())) Step();

			if (Peek() == '.' && char.IsNumber(PeekNext()))
			{
				Step();

				while (char.IsNumber(Peek())) Step();
			}
			double number = double.Parse(text.Substring(start, Position - start));
			return number;
		}

		public string ReadString(char c)
		{
			int start = Position;
			while (Peek() != c)
			{
				Step();
				if (Peek() != c && Position + 1 == text.Length)
				{
					Step(); // skip the last character
					return null;
				}
			}
			string str = text.Substring(start, Position - start);
			Step(); // skip the closing quotation
			return str;
		}

		public char this[int index]
		{
			get { return text[index]; }
		}

		public Scanner()
		{
			tokens = new List<Token>();
		}


		private Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
		{
			{"and",    TokenType.And},
			{"class",  TokenType.Class},
			{"else",   TokenType.Else},
			{"false",  TokenType.False},
			{"for",    TokenType.For},
			{"fun",    TokenType.Fun},
			{"if",     TokenType.If},
			{"nil",    TokenType.Nil},
			{"or",     TokenType.Or},
			{"print",  TokenType.Print},
			{"return", TokenType.Return},
			{"super",  TokenType.Super},
			{"this",   TokenType.This},
			{"true",   TokenType.True},
			{"while",  TokenType.While},
			{"var",    TokenType.Var}
		};

		private void addToken(TokenType tokenType, string lexeme, object? literal)
		{
			tokens.Add(new Token(tokenType, literal, lexeme, Position));
		}


		public List<Token> Scan(string line)
		{
			tokens.Clear();
			text = line;
			Position = 0;
			
			char c;
			while(Position < line.Length)
			{
				c = GetChar();
				switch (c)
				{
					case '(': addToken(TokenType.LeftParenthesis, "(", null); break;
					case ')': addToken(TokenType.RightParenthesis, ")", null); break;
					case '{': addToken(TokenType.LeftBrace, "{", null); break;
					case '}': addToken(TokenType.RightBrace, "}", null); break;
					case ',': addToken(TokenType.Comma, ",", null); break;
					case '.': addToken(TokenType.Dot, ".", null); break;
					case '-': addToken(TokenType.Minus, "-", null); break;
					case '+': addToken(TokenType.Plus, "+", null); break;
					case ';': addToken(TokenType.Semicolon, ";", null); break;
					case '*': addToken(TokenType.Asterisk, "*", null); break;
					case '/': if (Peek() == '/') return tokens; else addToken(TokenType.Slash, "\\", null); break;
					case '>': if (Peek() == '=') { addToken(TokenType.GreaterEqual, ">=", null); Step();} else { addToken(TokenType.Greater, ">", null); } break;
					case '<': if (Peek() == '=') { addToken(TokenType.SmallerEqual, "<=", null); Step();} else { addToken(TokenType.Smaller, "<", null); } break;
					case '=': if (Peek() == '=') { addToken(TokenType.EqualEqual, "==", null);	 Step();} else { addToken(TokenType.Equal, "=", null);	 } break;
					case '!': if (Peek() == '=') { addToken(TokenType.BangEqaul, "!=", null);	 Step(); } else { addToken(TokenType.Bang, "!", null);	 } break;
					case ' ':
					case '\r':
					case '\t':
					case '\n': 
						break;
					case '\'': 
					case '\"':
						string string_literal = ReadString(c);
						if(string_literal == null)
						{
							Console.WriteLine("[X] Unclosed String!");
						}
						else
						{
							addToken(TokenType.String, $"\"{string_literal}\"", string_literal);
						}
						break;
					default:
						// Identifiers
						if (char.IsLetter(c) || c == '_')
						{
							string id = ReadIdentifier();
							if (keywords.ContainsKey(id))
								addToken(keywords[id], id, null);
							else
								addToken(TokenType.Identifier, id, null);
						}
						else if (char.IsNumber(c))
						{
							double number = ReadNumber();
							addToken(TokenType.Number, number.ToString(), number);
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("[X] Unexpected character '{0}' at char {1}", c, Position);
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine(line); Console.WriteLine("^".PadLeft(Position));
							Console.ResetColor();
						}
						break;

				}
			}
			addToken(TokenType.EOF, null, null);
			return tokens;
		}


	}
}

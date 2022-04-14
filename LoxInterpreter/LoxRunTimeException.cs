using System.Runtime.Serialization;

namespace LoxInterpreter
{
	[Serializable]
	internal class LoxRunTimeException : Exception
	{
		public Token Token { get; private set; }
		public LoxRunTimeException(string message) : base(message) { }
		public LoxRunTimeException(string? message, Token token) : base(message) { Token = token; }
	}
}
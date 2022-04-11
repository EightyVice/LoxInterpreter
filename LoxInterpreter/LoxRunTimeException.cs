using System.Runtime.Serialization;

namespace LoxInterpreter
{
	[Serializable]
	internal class LoxRunTimeException : Exception
	{

		public LoxRunTimeException(string? message, Token token) : base(message) { }
	}
}
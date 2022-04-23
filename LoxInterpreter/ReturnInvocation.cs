using System.Runtime.Serialization;

namespace LoxInterpreter
{
	[Serializable]
	internal class ReturnInvocation : LoxRunTimeException
	{
		public object value;
		public ReturnInvocation(object value) : base(null, null)
		{
			this.value = value;
		}

		public ReturnInvocation(string message) : base(message)
		{
		}
	}
}
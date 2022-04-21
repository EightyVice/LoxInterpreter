namespace LoxInterpreter
{
	internal interface LoxCallable
	{
		public int Arity { get;}
		object Call(Interpreter interpreter, List<object> arguments);
	}

	internal class LoxFunction : LoxCallable
	{

		private readonly FunctionStatement declaration;

		public LoxFunction(FunctionStatement declaration)
		{
			this.declaration = declaration;
		}

		int LoxCallable.Arity { get => throw new NotImplementedException(); }

		public object Call(Interpreter interpreter, List<object> arguments)
		{
			Environment environment = new Environment(interpreter.globals);
			for (int i = 0; i < declaration.Parameters.Count; i++)
			{
				environment.Define(declaration.Parameters[i].Lexeme, arguments[i]);
			}

			interpreter.ExecuteBlock(declaration.Body, environment);
			return null;
		}
	}
}
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
		private readonly Environment closure;

		public int Arity { get; }

		public LoxFunction(FunctionStatement declaration, Environment closure)
		{
			this.declaration = declaration;
			this.closure = closure;

			Arity = declaration.Parameters.Count;
		}


		public object Call(Interpreter interpreter, List<object> arguments)
		{
			Environment environment = new Environment(closure);
			for (int i = 0; i < declaration.Parameters.Count; i++)
			{
				environment.Define(declaration.Parameters[i].Lexeme, arguments[i]);
			}
			try
			{
				interpreter.ExecuteBlock(declaration.Body, environment);
			}
			catch (ReturnInvocation returnValue)
			{
				return returnValue.value;
			}
			return null;
		}
	}
}
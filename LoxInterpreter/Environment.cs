using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal class Environment
	{
		private readonly Dictionary<string, object> variables = new Dictionary<string, object>();
		
		public void Define(string name, object value)
		{
			variables.Add(name, value);
		}

		public object Get(Token name)
		{
			if (variables.ContainsKey(name.Lexeme))
				return variables[name.Lexeme];
			else
				throw new LoxRunTimeException($"Using of undefined variable {name.Lexeme}", name);
		}
	}
}

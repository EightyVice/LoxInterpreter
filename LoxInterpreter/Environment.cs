using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal class Environment
	{
		private readonly Environment enclosing;

		private readonly Dictionary<string, object> variables = new Dictionary<string, object>();
	
		public Environment(Environment enclosingScope)
		{
			enclosing = enclosingScope;
		}

		public Environment()
		{
			enclosing = null;
		}
		public void Define(string name, object value)
		{
			if (!variables.ContainsKey(name))
				variables.Add(name, value);
			else
				throw new LoxRunTimeException($"Redefinition of variable '{name}'");
		}

		public object Get(Token name)
		{
			if (variables.ContainsKey(name.Lexeme))
				return variables[name.Lexeme];
			
			if(enclosing != null) return enclosing.Get(name);
			throw new LoxRunTimeException($"Using of undefined variable {name.Lexeme}", name);
		}

		internal void Assign(Token lhs, object value)
		{
			if (variables.ContainsKey(lhs.Lexeme))
			{
				variables[lhs.Lexeme] = value;
				return;
			}

			if(enclosing != null)
			{
				enclosing.Assign(lhs, value);
			}

			throw new LoxRunTimeException($"Undefined varialble '{lhs.Lexeme}'.", lhs);
		}
	}
}

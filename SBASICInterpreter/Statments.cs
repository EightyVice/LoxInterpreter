using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal interface IStatement
	{
		public void Execute()
		{

		}
	}

	internal class PrintStatement : IStatement
	{
		private string text;

		public PrintStatement(string Text)
		{
			text = Text;
		}

		void IStatement.Execute()
		{
			Console.WriteLine(text);
		}
	}
}

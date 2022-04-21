using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
	internal class Lox
	{
		public bool HasError { get;}

		private Scanner Scanner = new Scanner();
		private Parser Parser = new Parser();
		private Interpreter Interpreter = new Interpreter();
		private string sourceCode;

		public Lox()
		{
			HasError = false;
			
		}
		private bool printlexemes = false;
		private bool printast = false;

		internal void InterpretFile(string fileName)
		{
			sourceCode = File.ReadAllText(fileName);
			var tokens = Scanner.Scan(sourceCode);
			var ast = Parser.Parse(tokens);
			Interpreter.Interpret(ast);
		}
		internal void InterpretLine(string line)
		{
			if (line == ".prntlx")
			{
				printlexemes = !printlexemes;
				return;
			}
			if (line == ".prntast") {
				printast = !printast;
				return;
			}

			var tokens = Scanner.Scan(line);
			if (Scanner.HasError)
				return;

			if (printlexemes)
			{
				ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Magenta, ConsoleColor.Blue };
				int colorIndex = 0;
				Console.Write("   ");
				foreach (var token in tokens)
				{
					ConsoleColor color = colors[colorIndex % colors.Length];
					Console.ForegroundColor = color;
					Console.Write(token);
					Console.Write(" ");
					colorIndex++;
				}

				Console.WriteLine();
				Console.ResetColor();
			}
			try
			{
				Console.ForegroundColor = ConsoleColor.Green;
				var statements = Parser.Parse(tokens);
				Interpreter.Interpret(statements);
				Console.ResetColor();
			}
			catch (LoxRunTimeException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error: {ex.Message}");
				Console.WriteLine(line);
				Console.WriteLine($"^{new string('~', ex.Token.Lexeme.Length - 1)}".PadLeft((int)ex.Token.Position));
				Console.ResetColor();
			}
			Console.ResetColor();

		}
	}
}

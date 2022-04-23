#define REPL 
namespace LoxInterpreter
{
	internal class Program
	{
		static void Main(string[] args)
		{

#if REPL
			Console.ResetColor();
			Console.WriteLine("Lox REPL Interpreter");
			Console.WriteLine("By: Zeyad Ahmed");
			Console.WriteLine("Enter .help for help");
			Console.WriteLine("--------------------------");
			Console.WriteLine();

			Lox interpreter = new Lox();
			string line;
			while (true)
			{
				Console.Write("$> ");
				line = Console.ReadLine();
				if(line != "")
					interpreter.InterpretLine(line);
			}

#else
			try
			{
				Lox interpreter = new Lox();
				interpreter.InterpretFile("program.lox");
			}
			catch (LoxRunTimeException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error: {ex.Message} @ '{ex.Token.Lexeme}'Char: {ex.Token.Position}");
				Console.ResetColor();
			}

#endif

		} 
	}
}
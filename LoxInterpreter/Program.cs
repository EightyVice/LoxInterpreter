namespace LoxInterpreter
{
	internal class Program
	{
		static void Main(string[] args)
		{


			Console.WriteLine("Lox REPL Interpreter");
			Console.WriteLine("By: Zeyad Ahmed");
			Console.WriteLine();


			Interpreter interpreter = new Interpreter();
			string line;
			while (true)
			{
				Console.Write("$> ");
				line = Console.ReadLine();
				if(line != "")
					interpreter.InterpretLine(line);
			}
		}
	}
}
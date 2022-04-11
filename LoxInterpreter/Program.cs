﻿namespace LoxInterpreter
{
	internal class Program
	{
		static void Main(string[] args)
		{


			Console.WriteLine("Lox REPL Interpreter");
			Console.WriteLine("By: Zeyad Ahmed");
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
		}
	}
}
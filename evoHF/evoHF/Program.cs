using System.Text;

namespace evoHF
{
	internal class Program
	{
		public static Dictionary<char, String> MorseDict = new Dictionary<char, String>()
			{
				{'A' , ".-"},
				{'B' , "-..."},
				{'C' , "-.-."},
				{'D' , "-.."},
				{'E' , "."},
				{'F' , "..-."},
				{'G' , "--."},
				{'H' , "...."},
				{'I' , ".."},
				{'J' , ".---"},
				{'K' , "-.-"},
				{'L' , ".-.."},
				{'M' , "--"},
				{'N' , "-."},
				{'O' , "---"},
				{'P' , ".--."},
				{'Q' , "--.-"},
				{'R' , ".-."},
				{'S' , "..."},
				{'T' , "-"},
				{'U' , "..-"},
				{'V' , "...-"},
				{'W' , ".--"},
				{'X' , "-..-"},
				{'Y' , "-.--"},
				{'Z' , "--.."},
				{'0' , "-----"},
				{'1' , ".----"},
				{'2' , "..---"},
				{'3' , "...--"},
				{'4' , "....-"},
				{'5' , "....."},
				{'6' , "-...."},
				{'7' , "--..."},
				{'8' , "---.."},
				{'9' , "----."},
			};
		public static bool _sound = false;
		public static Translator _translator = new Translator();
		public static CommandHandler _handler = new CommandHandler();

		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Welcome! Type \"help\" to list available commands.");
			Command command;

			while (true)
			{
				Console.Write(">");
				command = new Command(Console.ReadLine());
				_handler.EvaluateCommand(command);
			}
		}
	}
}

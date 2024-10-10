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

		// '/' for letter spacing, ' ' for word spacing? 
		// WHAT DO I USE FOR SIGN SPACING????

		public static bool _sound = false;
		public static Translator _translator = new Translator();
		public static CommandHandler _handler = new CommandHandler();

		static void Main(string[] args)
		{
			


            Console.WriteLine("Welcome! Type \"help\" to list available commands.");

			Command command;

			do
			{
				Console.Write(">");
				command = new Command(Console.ReadLine());
				_handler.EvaluateCommand(command);
			} while (command.type!=CommandType.Exit);

		}


		//input scanning with recursion
		static bool ReadConfig()
		{
			char _read;

			if (!char.TryParse(Console.ReadLine(),out _read)) {
				return ReadConfig();
			};

			switch (_read)
			{
				case 'n':
					return false;
				case 'y':
					return true;
				default:
					return ReadConfig();
			}
		}
	}
}

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

		static void Main(string[] args)
		{
			Decoder _decoder = new Decoder();

			if (ReadConfig())
			{
				Console.WriteLine("Reading morse-signs from file...");
				try
				{
					string[] _decodeData = File.ReadAllLines("decodeParams.txt");

					if (_decodeData[0].Equals(_decodeData[1]))
					{
						Console.WriteLine("Reading failed!\nShort and long signs cannot be the same!");
					}
					else
					{
						Console.WriteLine("Reading successful!");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Reading failed:\n{ex.Message}");
					Console.WriteLine("Resorting to manual reading...");
				}
			}
			else
			{
				ReadStart(_decoder);
			}

			Console.WriteLine(_decoder);

            Console.ReadKey();
		}

		static void ReadStart(Decoder decoder)
		{
			Console.Write("Short sign character: ");
			decoder._shortSign=decoder.ReadSign();
			Console.Write("Long sign character: ");
			decoder._longSign = decoder.ReadSign();
		}



		//input scanning with recursion
		static bool ReadConfig()
		{
			char _read;
			Console.WriteLine("Read morse-signs from config? [y/n]");

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

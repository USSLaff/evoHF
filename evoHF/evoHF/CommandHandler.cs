using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Globalization;

namespace evoHF
{
	internal class CommandHandler
	{
		public void EvaluateCommand(Command command)
		{
			switch (command.type)
			{
				case CommandType.Help:
					HelpCommand();
					break;
				case CommandType.Translator:
					TranslatorCommand(command);
					break;
				case CommandType.Encode:
					Encode(command);
					break;
				case CommandType.Sound:
					SoundCommand(command);
					break;
				case CommandType.Config:
					ConfigCommand(command);
					break;
				case CommandType.Clear:
					ClearCommand();
					break;
				case CommandType.Exit:
					ExitCommand();
					break;
				case CommandType.Null:
				default:
					Console.WriteLine($"Command not recognised: {command.text[0]}");
					break;
			}
		}
		void TranslatorCommand(Command command)
		{
			int commandParams = command.text.Length;
			if (commandParams != 2 && commandParams != 4)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid parameter count. <-translator [get/set]->");
				Console.ForegroundColor = ConsoleColor.White;
				return;
			}

			switch (command.text[1])
			{
				case "get":
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(Program._translator);
					Console.ForegroundColor = ConsoleColor.White;

					break;
				case "set":

					if (command.text.Length != 4)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid parameter count.\n<-translator set [shortSign] [longSign]->");
						Console.ForegroundColor = ConsoleColor.White;

						return;
					}

					char shortS;
					char longS;

					if (!char.TryParse(command.text[2], out shortS) || !char.TryParse(command.text[3], out longS))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid sign paramater. Must be a single character.");
						Console.ForegroundColor = ConsoleColor.White;

						return;
					};

					if (shortS == longS)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Short and long signs cannot be the same.");
						Console.ForegroundColor = ConsoleColor.White;

						return;
					}

					Program._translator._shortSign = shortS;
					Program._translator._longSign = longS;

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"New translator parameters:\nShort sign : {shortS}\nLong sign  : {longS}");
					Console.ForegroundColor = ConsoleColor.White;



					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid parameters.");
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}

		}
		void Encode(Command command)
		{
			if (command.text.Length<3 || command.text[1].Length==0 || command.text[2].Length==0)
			{
                Console.WriteLine("Invalid parameters.");
				return;
			}

			string _encoded = "";

			switch (command.text[1])
			{
				case "file":
					if (!File.Exists($"{command.text[2]}.txt"))
					{	
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("File does not exist.");
						Console.ForegroundColor = ConsoleColor.White;
						return;
					}

					string _loadFile = File.ReadAllText($"{command.text[2]}.txt").ToUpper();
					foreach (char item in _loadFile)
					{
						if (Program.MorseDict.ContainsKey(item))
						{
							string tempMorse = Program.MorseDict[item];
							tempMorse.Replace('.' , Program._translator._shortSign);
							tempMorse.Replace('-' , Program._translator._longSign);

							_encoded += $"{tempMorse} ";
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine($"Character: \"{item}\" not present in MORSE dictionary. Aborting...");
							Console.ForegroundColor = ConsoleColor.White;

							return;
						}
					}

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"Encoding: {_loadFile}");
					Console.WriteLine($"Result: {_encoded}");
					Console.WriteLine($"Saving encoded text to {command.text[2]}_MORSE.txt.");
					File.WriteAllText($"{command.text[2]}_MORSE.txt",_encoded);
					Console.ForegroundColor = ConsoleColor.White;

					break;

				case "text":

					List<string> _lines = command.text.ToList();
					_lines.RemoveRange(0, 2);
					string _fullText = "";

					for (int i = 0; i < _lines.Count; i++)
					{
						_fullText += $"{_lines[i].ToUpper()}";
						if (i!=_lines.Count-1)
						{
							_fullText += " ";
						}
					}
					_encoded = "";

					foreach (char item in _fullText)
					{
						if (Program.MorseDict.ContainsKey(item))
						{
							string tempMorse = Program.MorseDict[item];
							tempMorse = tempMorse.Replace('.', Program._translator._shortSign);
							tempMorse = tempMorse.Replace('-', Program._translator._longSign);

							_encoded += $"{tempMorse} ";

							
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine($"Character: \"{item}\" not present in MORSE dictionary. Aborting...");
							Console.ForegroundColor = ConsoleColor.White;

							return;
						}
					}

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"Encoding: \"{_fullText}\"");
					Console.WriteLine($"Result: {_encoded}");
					Console.WriteLine($"Saving encoded text to text_MORSE.txt.");
					File.WriteAllText($"text_MORSE.txt", _encoded);
					Console.ForegroundColor = ConsoleColor.White;

					break;

				default:
                    Console.WriteLine("how");
                    break;
			}
		}
		void ConfigCommand(Command command)
		{
			switch (command.text.Length)
			{
				case 1:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Load: Loads the config file's translator parameters into the current translator.");
					Console.WriteLine("Save: Saves the current translator's parameters into the destination file.");
					Console.WriteLine("Read: Prints the parameters of the config file on the console.");
					Console.ForegroundColor = ConsoleColor.White;
					break;

				case 2:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("File name is required. <-config [subcommand] [filename].json->");
					Console.ForegroundColor = ConsoleColor.White;
					break;

				case 3:
					if (command.text[2].Length == 0)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("File name cannot be empty.");
						Console.ForegroundColor = ConsoleColor.White;
						break;
					}
					EvaluateConfigCommand(command);
					break;

				default:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("can u dont");
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}
		}
		void EvaluateConfigCommand(Command command)
		{
			switch (command.text[1])
			{

				case "load":
					if (!File.Exists($"{command.text[2]}.json"))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("File does not exist.");
						Console.ForegroundColor = ConsoleColor.White;
						break;
					}

					string _loadCfg = File.ReadAllText($"{command.text[2]}.json");
					Translator temp;
					try
					{
						temp = JsonSerializer.Deserialize<Translator>(_loadCfg);
					}
					catch (JsonException e)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid config parameters.");
						Console.ForegroundColor = ConsoleColor.White;
						break;
					}

					if (temp._shortSign == temp._longSign)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid config parameters.");
						Console.ForegroundColor = ConsoleColor.White;
						break;
					}

					Program._translator._shortSign = temp._shortSign;
					Program._translator._longSign = temp._longSign;

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Translator parameters loaded.");
					Console.WriteLine(temp);
					Console.ForegroundColor = ConsoleColor.White;
					break;

				case "read":
					if (!File.Exists($"{command.text[2]}.json"))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("File does not exist.");
						Console.ForegroundColor = ConsoleColor.White;
						break;
					}
					string readCfg = File.ReadAllText($"{command.text[2]}.json");

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(JsonSerializer.Deserialize<Translator>(readCfg));
					Console.ForegroundColor = ConsoleColor.White;
					break;

				case "save":
					string json = JsonSerializer.Serialize(Program._translator);
					File.WriteAllText($"{command.text[2]}.json", json);
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"Parameters saved on {command.text[2]}.");
					Console.ForegroundColor = ConsoleColor.White;
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("what did u even do");
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}

			Console.ForegroundColor = ConsoleColor.White;
		}
		void HelpCommand ()
		{
			
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("<-help->");
			Console.WriteLine("<-encode [file/text] [filename/\"text\".txt]->");
			Console.WriteLine("<-decode [file/text] [filename/\"text\".txt]->");
			Console.WriteLine("<-translator [get/set]->");
			Console.WriteLine("<-config [load/read/save] [filename]->");
			Console.WriteLine("<-sound [1/0]->");
			Console.WriteLine("<-clear->");
			Console.WriteLine("<-exit->");
			Console.ForegroundColor= ConsoleColor.White;
		}
		void SoundCommand(Command command)
		{
			if (command.text.Length != 2)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid amount of parameters. <-sound [1/0]->");
				Console.ForegroundColor = ConsoleColor.White;

				return;
			}
			switch (command.text[1])
			{
				case "1":
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Sounds enabled.");

					Program._sound = true;
					break;
				case "0":
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Sounds disabled.");

					Program._sound = false;
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid parameters. [1=on,0=off]");

					break;
			}
			Console.ForegroundColor = ConsoleColor.White;
		}
		void ExitCommand()
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("goodbye~ meow");
			Thread.Sleep(1500);
			Environment.Exit(0);
		}
		void ClearCommand () {
			Console.Clear();
		}
	}
}
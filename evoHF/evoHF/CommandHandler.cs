using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Globalization;
using System.ComponentModel.Design.Serialization;

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
				case CommandType.Decode:
					Decode(command);
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
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid parameters.");
				Console.ForegroundColor = ConsoleColor.White;
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
		void Decode(Command command)
		{
			if (command.text.Length < 3 || command.text[1].Length == 0 || command.text[2].Length == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid parameters.");
				Console.ForegroundColor = ConsoleColor.White;
				return;
			}

			string _decodedA = "";
			string _decodedB = "";

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
					string loadedFile = File.ReadAllText($"{command.text[2]}.txt");

					char[] fileHelper = loadedFile.ToCharArray();

					List<char> fileUniques = new(fileHelper.Distinct().ToList());
					fileUniques.Remove(' ');
					fileUniques.Remove('\\');

					if (fileUniques.Count>2)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid MORSE Code.");
						Console.ForegroundColor = ConsoleColor.White;
						return;
					}

					//string[] temp = loadedFile.Split(' ');

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"MORSE Signs recognised: [{fileUniques[0]};{fileUniques[1]}]");
					Console.WriteLine($"Attempting translation...");
					Console.ForegroundColor = ConsoleColor.White;

					string[] loadedSplit = loadedFile.Split(' ');
					string tempF = "";

					foreach (string item in loadedSplit)
					{
						tempF = item;
						tempF = tempF.Replace(fileUniques[0], '.');
						tempF = tempF.Replace(fileUniques[1], '-');
						_decodedA += $"{Program.MorseDict.FirstOrDefault(x => x.Value == tempF).Key}";
						tempF = item;
						tempF = tempF.Replace(fileUniques[0], '-');
						tempF = tempF.Replace(fileUniques[1], '.');
						_decodedB += $"{Program.MorseDict.FirstOrDefault(x => x.Value == tempF).Key}";
					}

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Translation A: ");
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(_decodedA);
					Console.WriteLine($"Saving decoded text to {command.text[2]}_A.txt.");
					File.WriteAllText($"{command.text[2]}_A.txt", _decodedA);


					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Translation B: ");
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(_decodedB);
					Console.WriteLine($"Saving decoded text to {command.text[2]}_B.txt.");
					File.WriteAllText($"{command.text[2]}_B.txt", _decodedB);


					Console.ForegroundColor = ConsoleColor.White;


					return;
				case "text":

					List<string> _lines = command.text.ToList();
					_lines.RemoveRange(0, 2);

					char[] textHelper = String.Join(" ", _lines.ToArray()).ToCharArray();

					List<char> textUniques = new(textHelper.Distinct().ToList());
					textUniques.Remove(' ');
					textUniques.Remove('\\');

					if (textUniques.Count > 2)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid MORSE Code.");
						Console.ForegroundColor = ConsoleColor.White;
						return;
					}

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"MORSE Signs recognised: [{textUniques[0]};{textUniques[1]}]");
					Console.WriteLine($"Attempting translation...");
					Console.ForegroundColor = ConsoleColor.White;
					
					string tempT = "";
					char? Tkey = null;

					foreach (string item in _lines)
					{
						tempT = item;
						tempT = tempT.Replace(textUniques[0], '.');
						tempT = tempT.Replace(textUniques[1], '-');

						try{
							Tkey = Program.MorseDict.First(x => x.Value == tempT).Key;
							_decodedA += $"{Tkey}";
						}
						catch (Exception e)
						{
							_decodedA = " ";
							break;
						}						

					}

					foreach (string item in _lines)
					{
						tempT = item;
						tempT = tempT.Replace(textUniques[0], '-');
						tempT = tempT.Replace(textUniques[1], '.');
						
						try
						{
							Tkey = Program.MorseDict.First(x => x.Value == tempT).Key;
							_decodedB += $"{Tkey}";
						}
						catch (Exception e)
						{
							Console.WriteLine($"\"{tempT}\"");
							_decodedB = " ";
							break;
						}
					}

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Translation A: ");
					if (_decodedA==" ")
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid.");
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.WriteLine(_decodedA);
						File.WriteAllText($"text_A.txt", _decodedA);
						Console.WriteLine($"Saving decoded text to text_A.txt.");
					}


					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Translation B: ");
					if (_decodedB == " ")
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid.");
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.WriteLine(_decodedB);
						File.WriteAllText($"text_B.txt", _decodedB);
						Console.WriteLine($"Saving decoded text to text_B.txt.");
					}



					Console.ForegroundColor = ConsoleColor.White;

					
					return;


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
			Console.WriteLine("<-clear->");
			Console.WriteLine("<-exit->");
			Console.ForegroundColor= ConsoleColor.White;
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
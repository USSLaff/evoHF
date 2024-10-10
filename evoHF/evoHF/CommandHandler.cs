using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace evoHF
{
	internal class CommandHandler
	{

		public void EvaluateCommand(Command command)
		{
			switch (command.type) {
				case CommandType.Help:
					
					break;

				case CommandType.Translator:
					TranslatorCommand(command);
					break;
                case CommandType.Sound:
					SoundCommand(command);
					break;

				case CommandType.Null:
				default:
                    Console.WriteLine($"Command not recognised: {command.text[0]}");
					break;
			}
		}

		void SoundCommand(Command command)
		{
			if (command.text.Length != 2)
			{
				Console.WriteLine("Invalid amount of parameters. [sound <state>]");
				return;
			}
			switch (command.text[1])
			{
				case "1":
					Console.WriteLine("Sounds enabled.");
					Program._sound = true;
					break;
				case "0":
					Console.WriteLine("Sounds disabled.");
					Program._sound = false;
					break;
				default:
					Console.WriteLine("Invalid parameters. [1=on,0=off]");
					break;
			}
		}
		void TranslatorCommand(Command command)
		{
			int commandParams = command.text.Length;
			if (commandParams != 2 && commandParams != 4) {
                Console.WriteLine("Invalid parameter count. <translator [get/set]>");
				return;
            }



			switch (command.text[1])
			{
				case "get":
                    Console.WriteLine(Program._translator);
                    break;
				case "set":

					if (command.text.Length != 4)
					{
                        Console.WriteLine("Invalid parameter count.\n<translator set [shortSign] [longSign]");
						return;
					}

					char shortS;
					char longS;

					if (!char.TryParse(command.text[2], out shortS))
					{
                        Console.WriteLine("Invalid short sign paramater. Must be a single character.");
						return;
					};

					if (!char.TryParse(command.text[3], out longS))
					{
						Console.WriteLine("Invalid long sign paramater. Must be a single character.");
						return;
					};

					if (shortS == longS)
					{
                        Console.WriteLine("Short and long signs cannot be the same.");
						return;
                    }
					Program._translator._shortSign = shortS;
					Program._translator._longSign = longS;

                    Console.WriteLine($"New translator parameters:\nShort sign : {shortS}\nLong sign  : {longS}");



                    break;
				default:
                    Console.WriteLine("Invalid parameters.");
                    break;
			}

			
		}
	}
}
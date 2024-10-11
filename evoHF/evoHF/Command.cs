using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evoHF
{



	internal class Command
	{
		public string[] text;
		public CommandType type;

		
		public Command(string message)
		{

			text = message.Split(' ');

			if (text.Length == 0) return;

			switch (text[0])
			{
				case Commands.CMD_HELP:
					type = CommandType.Help;
					return;
				case Commands.CMD_CONFIG:
					type = CommandType.Config;
					return;
				case Commands.CMD_EXIT:
					type = CommandType.Exit;
					return;
				case Commands.CMD_DECODE:
					type = CommandType.Decode;
					return;
				case Commands.CMD_ENCODE:
					type = CommandType.Encode;
					return;
				case Commands.CMD_SET_SOUND:
					type = CommandType.Sound;
					return;
				case Commands.CMD_TRANSLATOR:
					type = CommandType.Translator;
					return;
				case Commands.CMD_CLEAR:
					type = CommandType.Clear;
					return;
				default:
					type = CommandType.Null;
					return;

			}
		}

}
}

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
					type = CommandType.ReadConfig;
					return;
				case Commands.CMD_USER_SHOOT:
					type = MessageType.UserShoot;
					return;
				case Commands.CMD_USER_CREATELOBBY:
					type = MessageType.UserCreateLobby;
					return;
				case Commands.CMD_USER_JOINLOBBY:
					type = MessageType.UserJoinLobby;
					return;
				case Commands.CMD_USER_GETROOM:
					type = MessageType.UserGetRooms;
					return;
				case Commands.CMD_USER_SHIPDMGTAKE:
					type = MessageType.UserShipDMGTake;
					return;
				case Commands.CMD_USER_LEAVELOBBY:
					type = MessageType.UserLeaveLobby;
					return;
				default:
					type = MessageType.Null;
					return;

			}
		}

}
}

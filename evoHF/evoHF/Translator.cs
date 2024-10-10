using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evoHF
{
	public class Translator
	{
		public char _shortSign = '.';
		public char _longSign = '-';

		public char ReadSign()
		{
			char _read;


			if (!char.TryParse(Console.ReadLine(), out _read))
			{
				return ReadSign();
			};

			if (_shortSign == _read)
			{
				Console.WriteLine("Short and long signs cannot be the same!");
				return ReadSign();
			}

			return _read;
		}

		public string Decode(string input)
		{


			return "";
		}



		public string Encode(string input)
		{


			return "";
		}

		public override string ToString()
		{
			return $"Short sign: {_shortSign}\nLong sign : {_longSign}";
		}
	}
}

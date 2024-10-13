using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evoHF
{
	public class Translator
	{

		public char _shortSign { get; set; }
		public char _longSign { get; set; }

		public override string ToString()
		{
			return $"Short sign : {_shortSign}\nLong sign  : {_longSign}";
		}
	}
}

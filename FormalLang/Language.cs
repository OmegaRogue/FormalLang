using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace FormalLang
{
	public class Language
	{
		private static readonly Random Rng = new Random();

		public string Startsymbol;
		public char Trennzeichen;
		public string[] Terminalsymbole;
		public string[] Nichtterminalsymbole;
		public Dictionary<string, string[]> Regeln;
		
		public Language(string                       startsymbol, char trennzeichen, string[] terminalsymbole, string[] nichtterminalsymbole,
						Dictionary<string, string[]> regeln)
		{
			Startsymbol          = startsymbol;
			Trennzeichen         = trennzeichen;
			Terminalsymbole      = terminalsymbole;
			Nichtterminalsymbole = nichtterminalsymbole;
			Regeln               = regeln;
		}
		
		public string Replace(string input, bool output = false)
		{
			if (output) Console.WriteLine(input);
			while (true)
			{
				var stringarray = input.Split(Trennzeichen);
				var buffer      = "";
				var done        = false;
				var wasntdone   = false;
				foreach (var s in stringarray)
				{
					if (Terminalsymbole.Contains(s))
					{
						done   =  true;
						buffer += s + Trennzeichen;
					}

					if (Nichtterminalsymbole.Contains(s))
					{
						done      = false;
						wasntdone = true;
					}

					if (done) continue;
					try
					{
						var arrBuffer = Regeln[s];
						buffer += arrBuffer[Rng.Next(arrBuffer.Length)] + Trennzeichen;
					}
					catch (KeyNotFoundException)
					{
					}
				}

				if (output) Console.WriteLine(buffer);
				if (done && !wasntdone) return buffer;
				input = buffer;
			}
		}
		
		public static Language Load(string path)
		{
			var file = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Language>(file);
		}
		public static void Write(Language lang, string path)
		{
			var file = File.CreateText(path);
			file.Write(JsonConvert.SerializeObject(lang, Formatting.Indented));
			file.Flush();
			file.Close();
		}

		
		
	}
}
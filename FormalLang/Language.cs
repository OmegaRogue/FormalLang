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
		public char Seperator;
		public string[] Alphabet;
		public string[] Variables;
		public Dictionary<string, string[]> Rules;
		
		public Language(string                       startsymbol, char seperator, string[] alphabet, string[] variables,
						Dictionary<string, string[]> rules = null)
		{
			Startsymbol          = startsymbol;
			Seperator         = seperator;
			Alphabet = alphabet;
			Variables = variables;
			Rules = rules ?? new Dictionary<string, string[]>();
		}
		
		public string Replace(string input, bool output = false)
		{
			if (output) Console.WriteLine(input);
			while (true)
			{
				var stringarray = input.Split(Seperator);
				var buffer      = "";
				var done        = false;
				var wasntdone   = false;
				foreach (var s in stringarray)
				{
					if (Alphabet.Contains(s))
					{
						done   =  true;
						buffer += s + Seperator;
					}

					if (Variables.Contains(s))
					{
						done      = false;
						wasntdone = true;
					}

					if (done) continue;
					try
					{
						var arrBuffer = Rules[s];
						buffer += arrBuffer[Rng.Next(arrBuffer.Length)] + Seperator;
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

		public Language AddRule(string inSymbol, params string[] outSymbols)
		{
			Rules.Add(inSymbol, outSymbols);
			return this;
		}

		
		
	}
}
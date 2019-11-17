using System;
using FormalLang;

namespace LangTest
{
	class Program
	{
		private static Language _lang;
		static void Main(string[] args)
		{
			try
			{
				_lang = Language.Load(args[0]);
			}
			catch (Exception)
			{
				_lang = Language.Load("deutsch.json");
			}
			int  n;
			bool log;
			try
			{
				n = Convert.ToInt32(args[1]);
			}
			catch (Exception)
			{
				n = 1;
			}

			try
			{
				log = Convert.ToBoolean(args[2]);
			}
			catch (Exception)
			{
				log = false;
			}
            
			for (var i = 0; i < n; i++)
			{
				Console.WriteLine(_lang.Replace(_lang.Startsymbol, log));
			}
		}
	}
}
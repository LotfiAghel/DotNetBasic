using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Localization
{
	public static class Localize
	{
		public enum Language
		{
			English = 0, Persian = 1, German = 2, French = 3, Spanish = 4,
			Turkish = 5, Arabic = 6, Chinese = 7, Japanese = 8
		}

		public static Language Parse(string lang)
		{
			try
			{
				if (lang != null)
					return (Language)Enum.Parse(typeof(Language), lang);
			}
			catch (Exception e)
			{
				//Logging.Warn("Localize.Parse", e + "");
			}
			return Language.English;
		}

		public static string Get(Language language, string key, params object[] args)
		{
			string str;
			var currentPack = Phrases.GetPack(language);
			if (currentPack != null && currentPack.ContainsKey(key))
				str = currentPack[key];
			else
				str = key;
			return string.Format(str, args);
		}
		public static string Get2(Language language, string key, Dictionary<string,object> parms)
		{
			string str;
			var currentPack = Phrases.GetPack(language);
			if (currentPack != null && currentPack.ContainsKey(key))
				str = currentPack[key];
			else
				str = key;
			return key;
		}
		static Dictionary<string, string> isoMap = new Dictionary<string, string>()
		{
			{"IR", "Persian"}, 
			{"US", "English"}, 
			{"CA", "English"},
			{"DE", "German"},
			{"FR", "French"},
			{"CN", "Chinese"},
			{"HK", "Chinese"},
			{"CO", "Spanish"},
			{"EG", "Arabic"},
			{"GR", "Greek"},
			{"IN", "Hindi"},
			{"IQ", "Arabic"},
			{"IT", "Italian"},
			{"JP", "Japanese"},
			{"KP", "Korean"},
			{"KR", "Korean"},
			{"KW", "Arabic"},
			{"LB", "Arabic"},
			{"MX", "Spanish"},
			{"MA", "Arabic"},
			{"NL", "Dutch"},
			{"NZ", "English"},
			{"SA", "Arabic"},
			{"ES", "Spanish"},
			{"SE", "Swedish"},
			{"TW", "Chinese"},
			{"TR", "Turkish"},
			{"AE", "Arabic"},
			{"GB", "English"},
			{"BH", "Arabic"},
		};

		public static Language GetLanguage(string countryISO)
		{
			if (isoMap.ContainsKey(countryISO))
			{
				return Parse(isoMap[countryISO]);
			}
			return Language.English;
		}

		public static Dictionary<Language, Dictionary<string, string>> GetPacks()
		{
			return Phrases.phrasePacks;
		}
	}
}

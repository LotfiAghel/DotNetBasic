using System;
using System.Collections.Generic;

namespace Localization
{
    public static class Phrases
    {
		public static Dictionary<Localize.Language, Dictionary<string, string>> phrasePacks =
			new Dictionary<Localize.Language, Dictionary<string, string>>();

		static Phrases()
		{
			phrasePacks.Add(Localize.Language.Persian, new Dictionary<string, string>()
			{
				{"Server Error", "خطای سرور"},
				{"Invalid Parameters", "خطا در اراسال پارامترها"},
				{"Function Not Found", "تابع پیدا نشد"},
				{"Operation Failed", "خطایی پیش آمده"}
			});
		}

		public static void UpdatePhrase(Localize.Language language, string phrase, string localized)
		{
			Dictionary<string, string> pack;
			if (!phrasePacks.TryGetValue(language, out pack))
			{
				pack = new Dictionary<string, string>();
			}
			if (!pack.TryAdd(phrase, localized))
			{
				pack[phrase] = localized;
			}
		}

		public static Dictionary<string, string> GetPack(Localize.Language language)
		{
			if (phrasePacks.ContainsKey(language))
				return phrasePacks[language];
			return null;
		}
    }
}

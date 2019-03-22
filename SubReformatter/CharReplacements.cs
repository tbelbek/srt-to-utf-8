using System.Collections.Generic;

namespace SubReformatter
{
    public static class CharReplacements
    {
        public static List<KeyValuePair<string, string>> CharReplacementsList { get; set; }

        public static List<KeyValuePair<string, string>> GetList()
        {
            return CharReplacementsList;
        }

        static CharReplacements()
        {
            CharReplacementsList = new List<KeyValuePair<string, string>>();
            CharReplacementsList.Add(new KeyValuePair<string, string>("ð", "ğ"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("ý", "ı"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("þ", "ş"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ã–", "Ö"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ã¶", "ö"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ã¼", "ü"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ãœ", "Ü"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ã§", "ç"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ã‡", "Ç"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("ÅŸ", "ş"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ä±", "ı"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ä°", "İ"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("ÄŸ", "ğ"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Åž", "Ş"));
            CharReplacementsList.Add(new KeyValuePair<string, string>("Ã¢", "a"));
            
        }
    }
}

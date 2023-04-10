namespace LanguageAnalyzer
{
    public class LanguageText
    {
        public int[] Letters { get; set; }
        public string Language { get; set; }

        public LanguageText(string text, string language)
        {
            Letters = new int[26];
            foreach (var letter in text.ToLower())
                if (letter is >= 'a' and <= 'z')
                    Letters[letter-97]++;
            Language = language;
        }

        public override string ToString()
        {
            return Language + " - " + string.Join(",",Letters);
        }
    }
}
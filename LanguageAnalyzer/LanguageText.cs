using System;
using System.Linq;

namespace LanguageAnalyzer
{
    public class LanguageText
    {
        public double[] Letters { get; set; }
        public string Language { get; set; }

        public LanguageText(string text, string language)
        {
            Letters = new double[26];
            foreach (var letter in text.ToLower())
                if (letter is >= 'a' and <= 'z')
                    Letters[letter-97]++;
            Language = language;
        }
        
        public void Normalise()
        {
            var norm = Letters.Sum(d => d*d);
            norm = Math.Sqrt(norm);
            for (var i = 0; i < Letters.Length; i++)
                Letters[i] /= norm;
        }

        public override string ToString()
        {
            return Language + " - [" + Math.Round(Letters[0],3) + ".." + Math.Round(Letters.Last(),3) + "]";
        }
    }
}
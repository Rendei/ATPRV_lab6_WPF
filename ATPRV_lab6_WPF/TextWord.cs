using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPRV_lab6_WPF
{
    internal class TextWord
    {
        public string Name { get; set; }
        public List<WordCount> WordCountList { get; set; }

        public override string ToString()
        {
            string wordCountString = string.Empty;
            foreach (WordCount wordCount in WordCountList) { wordCountString += wordCount.ToString() + "\n";}

            return $"Текст: {Name}\n" +
                $"Вхождения слов: \n" +
                $"{wordCountString}";
        }
    }
}

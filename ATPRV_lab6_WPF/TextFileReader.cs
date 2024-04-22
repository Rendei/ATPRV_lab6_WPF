using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPRV_lab6_WPF
{
    internal class TextFileReader
    {
        public List<Text> Texts { get; set; }
        public List<TextWord> TextWords { get; set; }

        public TextFileReader()
        {
            Texts = LoadTexts();
            TextWords = new List<TextWord>();
        }

        private static List<Text> LoadTexts()
        {
            List<Text> texts = new List<Text>();
            DirectoryInfo directory = new DirectoryInfo("./texts");
            var files = directory.GetFiles();

            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    texts.Add(new Text
                    { 
                            Name = Path.GetFileNameWithoutExtension(file.Name),
                            Words = sr.ReadToEnd(),
                    });
                }

            }
            return texts;
        }

        public void FindWordsCount(Text text, List<string> filterWords)
        {
            string[] textWords = text.Words.Split('\n', '\r' ,' ', ',',
                '.', '?', '!',
                '»', '>', '}',
                '«', '<', '{',
                ';', ':', '(', ')',
                '-', '—'
                );

            textWords = textWords.Select(word => word.ToLower()).ToArray();
            var filteredWords = textWords.Where(textWord => filterWords.Contains(textWord))
                .GroupBy(group => group)
                .Select(group => new WordCount
                {
                    Word = group.Key,
                    Count = group.Count()
                }).ToList();

            TextWords.Add(new TextWord
            {
                Name = text.Name,
                WordCountList = filteredWords,
            });

        }
    }
}

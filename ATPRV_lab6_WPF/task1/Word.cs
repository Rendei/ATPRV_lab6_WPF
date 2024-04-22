using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPRV_lab6_WPF
{
    public class WordCount
    {
        public string Word {  get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return $"Слово: {Word}, вхождения: {Count}";
        }
    }
}

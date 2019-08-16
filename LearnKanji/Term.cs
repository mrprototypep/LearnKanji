using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LearnKanji
{
    public class Term
    {
        [JsonProperty]
        public string TermDefinition { get; private set; }
        [JsonProperty]
        public string HiraganaDefinition { get; private set; }
        [JsonProperty]
        public string AnswerDefinition { get; private set; }

        public Term(string term, string hiragana, string answer)
        {
            TermDefinition = term;
            HiraganaDefinition = hiragana;
            AnswerDefinition = answer;
        }
    }
}

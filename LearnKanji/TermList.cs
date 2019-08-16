using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using Newtonsoft.Json;

namespace LearnKanji
{
    class TermList : ListBoxItem
    {
        public string Path { get; private set; }
        public List<Term> Terms { get; private set; }

        public TermList(string path) : base() {
            Path = path;
            AddText(System.IO.Path.GetFileNameWithoutExtension(path));

            Terms = JsonConvert.DeserializeObject<List<Term>>(File.ReadAllText(path));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapPropertyEdit
{
    public class Tag
    {
        public string Name { get; set; }

        public List<string> SeartchKeys { get; set; } = new List<string>();


        public override string ToString()
        {
            return Name;
        }

        public bool hasSearchKeysThathStartsWithKey(string key) {
            return SeartchKeys.Where(x => x.StartsWith(key)).Count() > 0;
        }

    }
}

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

        public string English { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}

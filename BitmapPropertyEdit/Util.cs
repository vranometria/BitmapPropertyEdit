using System;
using System.Collections.Generic;
using System.Linq;
using Codeplex.Data;
using System.IO;

namespace BitmapPropertyEdit
{
    public static class Util
    {
        public static string ReadToEnd(string file) {
            using (StreamReader reader = new StreamReader(file)) {
                return reader.ReadToEnd();
            }
        }

        public static dynamic GetJson(string file) {
            return DynamicJson.Parse(ReadToEnd(file));
        }

        internal static void WriteJson(string file , dynamic obj)
        {
            string s = DynamicJson.Serialize(obj);

            using (StreamWriter writer = new StreamWriter(file)) {
                writer.WriteLine(s);
            }


        }
    }
}

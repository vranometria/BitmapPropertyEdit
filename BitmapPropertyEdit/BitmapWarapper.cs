using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;

namespace BitmapPropertyEdit
{
    public class BitmapWarapper : IDisposable
    {

        private Bitmap bitmap;

        private string file;

        public BitmapWarapper(string file)
        {
            this.file = file;
            bitmap = new Bitmap(file);
        }

        public string Comment { set { SetPropertyValue(value, 40095); } }

        public string Tag { set { SetPropertyValue(value, 40094); } }



        private void SetPropertyValue(string value, int id)
        {
            var pi = bitmap.GetPropertyItem(id);
            pi.Value = Encoding.Unicode.GetBytes(value);
            pi.Len = pi.Value.Length;
            bitmap.SetPropertyItem(pi);
        }

        public void Dispose()
        {
            bitmap.Dispose();
        }

        public void Save(string directory)
        {
            string name = Path.GetFileName(file);
            string output = Path.Combine(directory, name);
            bitmap.Save(output);
        }
    }
}

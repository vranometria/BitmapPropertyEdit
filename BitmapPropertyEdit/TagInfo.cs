using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BitmapPropertyEdit
{
    public class TagInfo
    {
        #region<sigleton>
        private TagInfo() { Load(); }
        private static TagInfo instance;
        public static TagInfo Instance { get { return instance == null ? (instance = new TagInfo()) : instance; } }
        #endregion<sigleton>


        private Hashtable groups = new Hashtable();

        public Hashtable Groups { get { return groups; } }

        private void Load()
        {
            dynamic json = Util.GetJson("tag.json");

            foreach (var groupJson in json.groups) {

                if (!groups.ContainsKey(groupJson.name))
                    groups.Add(groupJson.name, new List<string>());

                var group = groups[groupJson.name] as List<string>;

                foreach (var tagJson in groupJson.tags) {
                    group.Add(tagJson);
                }

                groups[groupJson.name] = group;
            }
        }

    }
}

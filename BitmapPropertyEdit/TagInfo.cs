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

            // group { name:str , tags[ {},{} ] }
            foreach (var groupJson in json.groups) {

                if (!groups.ContainsKey(groupJson.name))
                    groups.Add(groupJson.name, new List<Tag>());

                var group = groups[groupJson.name] as List<Tag>;

                //tags[ { name :"" , search-keys : ["","",""] } ]
                foreach (var tagJson in groupJson.tags) {
                    Tag tag = new Tag() { Name = tagJson.name };
                    foreach (var key in tagJson.search_keys ) {
                        tag.SeartchKeys.Add(key);
                    }
                    group.Add(tag);
                }

                groups[groupJson.name] = group;
            }
        }

    }
}

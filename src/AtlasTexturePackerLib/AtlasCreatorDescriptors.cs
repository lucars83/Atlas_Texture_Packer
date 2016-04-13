using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace AtlasTexturePacker.Library
{
    partial class AtlasCreator
    {
        public enum AtlasFormat
        {
            LIBGDX
        }

        public static void SaveAtlasFile(Dictionary<string, Atlas> atlases, string path, AtlasFormat format)
        {
            switch (format)
            {
                case AtlasFormat.LIBGDX: SaveLibgdxFile(atlases, path); break;
            }
        }

        #region Libgdx

        protected static void SaveLibgdxFile(Dictionary<string, Atlas> atlases, string path)
        {
            KeyValuePair<string, Atlas> first = atlases.FirstOrDefault();
            string fullPath = string.Format("{0}\\{1}", path, first.Key.Replace(".png", ".atlas"));

            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                foreach (KeyValuePair<string, Atlas> atlas in atlases)
                {
                    sw.WriteLine("");
                    // Image name
                    sw.WriteLine(atlas.Key);
                    // size
                    sw.WriteLine(string.Format("size: {0},{1}", atlas.Value.root.rc.Width, atlas.Value.root.rc.Height));
                    // format
                    sw.WriteLine("format: RGBA8888");
                    // filter
                    sw.WriteLine("filter: Nearest,Nearest");
                    // repeat
                    sw.WriteLine("repeat: none");

                    SaveLibgdxFile(sw, atlas.Value.root);
                }
            }

        }

        protected static void SaveLibgdxFile(StreamWriter sw, AtlasNode node)
        {
            // This node write contents only if it is a leaf node
            if (node.name != "Unknown")
            {
                int index = GetIndex(ref node.name);
                // image name
                sw.WriteLine(node.name);
                // rotate
                sw.WriteLine("  rotate: false");
                // xy
                sw.WriteLine("  xy: {0}, {1}", node.rc.X, node.rc.Y);
                // size
                sw.WriteLine("  size: {0}, {1}", node.rc.Width, node.rc.Height);
                // orig
                sw.WriteLine("  orig: {0}, {1}", node.rc.Width, node.rc.Height);
                // offset
                sw.WriteLine("  offset: {0}, {1}", 0, 0);
                // index
                sw.WriteLine("  index: {0}", index);
            }


            if (node.child != null)
            {
                if (node.child[0] != null)
                    SaveLibgdxFile(sw, node.child[0]);
                if (node.child[1] != null)
                    SaveLibgdxFile(sw, node.child[1]);
            }

        }

        #endregion

        public static int GetIndex(ref string name)
        {
            string[] parts = name.Split('_');

            if(parts.Length <= 1)
                return -1;

            int index = -1;

            int.TryParse(parts[parts.Length - 1], out index);

            string[] newParts = new string[parts.Length - 1];

            Array.Copy(parts, newParts, parts.Length - 1);

            name = string.Join("_", newParts);
            
            return index;
        }
    }
}
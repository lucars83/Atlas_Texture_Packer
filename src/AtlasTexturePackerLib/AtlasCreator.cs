//#define DEBUG_ATLASES
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AtlasTexturePacker.Library
{

    public partial class AtlasCreator
    {
        public static int AtlasSize = 1024;
        public static string ImageRegex = "[\\.png|\\.jpg|\\.jpeg]$";

        public partial class AtlasNode
        {
            public AtlasNode[] child = null;
            public Rect rc = new Rect(0, 0, 0, 0);
            public BitmapExtended imageRef = null;
            public bool hasBitmapExtended = false;
            public int sortIndex = 0;
            public string name = "Unknown";

            private static readonly int TEXTURE_PADDING = 0;
            private static readonly bool BLEED = false;

            // The insert function traverses the tree looking for a place to insert the texture. 
            // It returns the node of the atlas the texture can go into or null to say it can't fit. 
            // Note we really don't have to store the rectangle for each node.
            // All we need is a split direction and coordinate like in a kd-tree, but it's more convenient with rects.
            public AtlasNode Insert(BitmapExtended image, int index)
            {
                if (image == null) // Obviously an error!
                    return null;

                if (child != null)
                {// If this node is not a leaf, try inserting into first child.
                    AtlasNode newNode = child[0].Insert(image, index);
                    if (newNode != null)
                        return newNode;

                    // No more room in first child, insert into second child!
                    return child[1].Insert(image, index);
                }
                else
                {
                    // If there is already a lightmap in this node, early out
                    if (hasBitmapExtended)
                        return null;

                    // If this node is too small for the image, return
                    if (!BitmapExtendedFits(image, rc))
                        return null;

                    // If the image is perfect, accept!
                    if (PerfectFit(image, rc))
                    {
                        hasBitmapExtended = true;
                        imageRef = image;
                        name = imageRef.Name;
                        sortIndex = index;
                        return this;
                    }

                    // If we made it this far, this node must be split.
                    child = new AtlasNode[2];
                    child[0] = new AtlasNode();
                    child[1] = new AtlasNode();

                    // Decide which way to split image
                    float deltaW = rc.Width - image.Width;
                    float deltaH = rc.Height - image.Height;

                    if (deltaW > deltaH)
                    {
                        child[0].rc = new Rect(rc.xMin, rc.yMin, image.Width, rc.Height);
                        child[1].rc = new Rect(rc.xMin + image.Width + TEXTURE_PADDING, rc.yMin, rc.Width - (image.Width + TEXTURE_PADDING), rc.Height);
                    }
                    else
                    {
                        child[0].rc = new Rect(rc.xMin, rc.yMin, rc.Width, image.Height);
                        child[1].rc = new Rect(rc.xMin, rc.yMin + image.Height + TEXTURE_PADDING, rc.Width, rc.Height - (image.Height + TEXTURE_PADDING));
                    }

                    // Lets try inserting into first child, eh?
                    return child[0].Insert(image, index);
                }
            }

            public bool Contains(string search)
            {
                if (name == search)
                    return true;

                if (child != null)
                {
                    if (child[0].Contains(search))
                        return true;
                    if (child[1].Contains(search))
                        return true;
                }

                return false;
            }

            static bool BitmapExtendedFits(BitmapExtended image, Rect rect)
            {
                return rect.Width >= image.Width && rect.Height >= image.Height;
            }

            static bool PerfectFit(BitmapExtended image, Rect rect)
            {
                return rect.Width == image.Width && rect.Height == image.Height;
            }

            public void GetNames(ref List<string> result)
            {
                if (hasBitmapExtended)
                {
                    result.Add(name);
                }
                if (child != null)
                {
                    if (child[0] != null)
                    {
                        child[0].GetNames(ref result);
                    }
                    if (child[1] != null)
                    {
                        child[1].GetNames(ref result);
                    }
                }
            }

            public void GetBounds(ref List<AtlasNode> result)
            {
                if (hasBitmapExtended)
                {
                    result.Add(this);
                }
                if (child != null)
                {
                    if (child[0] != null)
                    {
                        child[0].GetBounds(ref result);
                    }
                    if (child[1] != null)
                    {
                        child[1].GetBounds(ref result);
                    }
                }
            }

            public void Clear()
            {
                if (child != null)
                {
                    if (child[0] != null)
                    {
                        child[0].Clear();
                    }
                    if (child[1] != null)
                    {
                        child[1].Clear();
                    }
                }
                if (imageRef != null)
                    imageRef = null;
            }

            public void Build(BitmapExtended target)
            {
                if (child != null)
                {
                    if (child[0] != null)
                    {
                        child[0].Build(target);
                    }
                    if (child[1] != null)
                    {
                        child[1].Build(target);
                    }
                }
                if (imageRef != null)
                {
                    
                    Color[] data = imageRef.GetPixels();
                    for (int x = 0; x < imageRef.Width; ++x)
                    {
                        for (int y = 0; y < imageRef.Height; ++y)
                        {
                            target.SetPixel(x + (int)rc.X, y + (int)rc.Y, data[x + y * imageRef.Width]);
                        }
                    }
                    // Artificial texture bleeding!
                    if (TEXTURE_PADDING > 0 && BLEED)
                    {
                        for (int y = 0; y < imageRef.Height; ++y)
                        {
                            int x = imageRef.Width - 1;
                            target.SetPixel(x + (int)rc.X + TEXTURE_PADDING, y + (int)rc.Y, data[x + y * imageRef.Width]);
                        }
                        for (int x = 0; x < imageRef.Width; ++x)
                        {
                            int y = imageRef.Height - 1;
                            target.SetPixel(x + (int)rc.X, y + (int)rc.Y + TEXTURE_PADDING, data[x + y * imageRef.Width]);
                        }
                    }
                }
            }
        }

        public class AtlasDescriptor
        {
            public string name;
            public Rect uvRect;
            public int width;
            public int height;
        }

        public class Atlas
        {
            public BitmapExtended texture;
            public AtlasNode root;
            public AtlasDescriptor[] uvRects;
        }

        public static void SaveAtlas(Atlas atlas, string directory, string name)
        {
            if (atlas == null || atlas.texture == null)
                return;

            string path = directory + "\\" + name;

            atlas.texture.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static Atlas[] CreateAtlas(string name, BitmapExtended[] textures, Atlas startWith = null)
        {
            // Rotate images
            for (int i = 0; i < textures.Length; ++i)
                textures[i].RotateFlip(RotateFlipType.Rotate90FlipNone);

            List<BitmapExtended> toProcess = new List<BitmapExtended>();
            toProcess.AddRange(textures);
            int index = toProcess.Count - 1;
            toProcess.Reverse(); // Because we index backwards

            List<Atlas> result = new List<Atlas>();

            int insertIndex = 0;
            if (startWith != null)
            {
                insertIndex = startWith.root.sortIndex;
            }

            while (index >= 0)
            {
                Atlas _atlas = startWith;
                if (_atlas == null)
                {
                    _atlas = new Atlas();
                    //_atlas.texture = new Texture2D(AtlasSize, AtlasSize, TextureFormat.RGBA32, false);
                    _atlas.texture = new BitmapExtended(AtlasSize, AtlasSize);
                    _atlas.root = new AtlasNode();
                    _atlas.root.rc = new Rect(0, 0, AtlasSize, AtlasSize);
                }
                startWith = null;

                while (index >= 0 && (_atlas.root.Contains(toProcess[index].Name) || _atlas.root.Insert(toProcess[index], insertIndex++) != null))
                {
                    index -= 1;
                }
                result.Add(_atlas);
                _atlas.root.sortIndex = insertIndex;
                insertIndex = 0;
                _atlas = null;
            }

            foreach (Atlas atlas in result)
            {
                atlas.root.Trim(ref atlas.texture);
                atlas.root.Build(atlas.texture);
                List<AtlasNode> nodes = new List<AtlasNode>();
                atlas.root.GetBounds(ref nodes);
                nodes.Sort(delegate(AtlasNode x, AtlasNode y)
                {
                    if (x.sortIndex == y.sortIndex) return 0;
                    if (y.sortIndex > x.sortIndex) return -1;
                    return 1;
                });

                List<Rect> rects = new List<Rect>();
                foreach (AtlasNode node in nodes)
                {
                    Rect normalized = new Rect(node.rc.xMin / atlas.root.rc.Width, node.rc.yMin / atlas.root.rc.Height, node.rc.Width / atlas.root.rc.Width, node.rc.Height / atlas.root.rc.Height);
                    // bunp everything over by half a pixel to avoid floating errors
                    normalized.X += 0.5f / atlas.root.rc.Width;
                    normalized.Width -= 1.0f / atlas.root.rc.Width;
                    normalized.X += 0.5f / atlas.root.rc.Height;
                    normalized.Height -= 1.0f / atlas.root.rc.Height;
                    rects.Add(normalized);
                }

                atlas.uvRects = new AtlasDescriptor[rects.Count];
                for (int i = 0; i < rects.Count; i++)
                {
                    atlas.uvRects[i] = new AtlasDescriptor();
                    atlas.uvRects[i].width = (int)nodes[i].rc.Width;
                    atlas.uvRects[i].height = (int)nodes[i].rc.Height;
                    atlas.uvRects[i].name = nodes[i].name;
                    atlas.uvRects[i].uvRect = rects[i];
                }

                atlas.root.Clear();
            }

            return result.ToArray();
        }
        
        	

        public static void QuickCreate(string inputDir, string outputDir, int maxSize = 1024, bool recursive = true, AtlasFormat format = AtlasFormat.NONE)
        {
            Console.WriteLine("Loading images");
        	string[] images = Directory.GetFiles( inputDir ).Where(x => Regex.IsMatch(x.ToLower(), ImageRegex)).ToArray();
        	
        	string atlasName = Path.GetFullPath(inputDir).Substring(inputDir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        	
        	BitmapExtended[] textures = images.Select(x => new BitmapExtended(x)).ToArray();
        	
        	if(recursive)
        	{
        		string[] subDirs = Directory.GetDirectories(inputDir);
                for (int i = 0; i < subDirs.Length; ++i)
                    textures = textures.Concat(LoadImagesR(subDirs[i], inputDir)).ToArray();
        	}

            AtlasCreator.AtlasSize = maxSize;

            Console.WriteLine("Creating Atlas");
        	AtlasCreator.Atlas[] atlases = AtlasCreator.CreateAtlas(atlasName, textures);
            Dictionary<string, Atlas> atlasesWithNames = new Dictionary<string, Atlas>();

            Console.WriteLine("Saving Atlas");
            for(int i = 0; i < atlases.Length; ++i)
            {
                // build name
                string sheetName = i == 0 ? string.Format("{0}.png", atlasName) :
                    string.Format("{0}{1}.png", atlasName, i);
                atlasesWithNames.Add(sheetName, atlases[i]);

                // create atlas
                AtlasCreator.SaveAtlas(atlases[i], outputDir, sheetName);

                // create descriptor
                SaveAtlasFile(atlasesWithNames, outputDir, format);
            }
        }
        
        private static BitmapExtended[] LoadImagesR(string inputDir, string rootDir)
        {
        	string[] images = Directory.GetFiles( inputDir ).Where(x => Regex.IsMatch(x.ToLower(), ImageRegex)).ToArray();

            string subDirName = inputDir.Substring(rootDir.Length + 1);
        	
        	BitmapExtended[] textures = images.Select(x => new BitmapExtended(x, (subDirName + "/" + Path.GetFileNameWithoutExtension(x)).Replace("\\", "/"))).ToArray();
        	
        	string[] subDirs = Directory.GetDirectories(inputDir);
        		for(int i = 0; i < subDirs.Length; ++i)
        			textures = textures.Concat(LoadImagesR(subDirs[i], inputDir)).ToArray();
        			
        	return textures;
        }
    }
}
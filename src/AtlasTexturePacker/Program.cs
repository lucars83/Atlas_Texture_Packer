using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasTexturePacker.Library;
using System.Text.RegularExpressions;
using System.Drawing;


namespace AtlasTexturePacker.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string[] images = Directory.GetFiles( Directory.GetCurrentDirectory() ).Where(x => Regex.IsMatch(x.ToLower(), "\\.png$")).ToArray();

            BitmapExtended[] textures = images.Select(x => new BitmapExtended(x)).ToArray();

            //AtlasCreator.Atlas[] atlases = AtlasCreator.CreateAtlas("PlayerGuard", textures);

            for(int i = 0; i < atlases.Length; ++i)
            {
                //AtlasCreator.SaveAtlas(atlases[i], "Name" + i.ToString());
            }
            */
            AtlasArguments arguments = new AtlasArguments();
            
            if (CommandLine.Parser.Default.ParseArguments(args, arguments))
            {
                int i = 0;
            }
        }
    }
}

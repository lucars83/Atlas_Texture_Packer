using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace AtlasTexturePacker.CLI
{
    public class AtlasArguments
    {
        [Option('i', "inputPath", Required = true, HelpText = "The path to the images to pack")]
        public string InputPath { get; set; }

        [Option('o', "outputPath", Required = true, HelpText = "The path to export the packed texture to")]
        public string OutputPath { get; set; }

        [Option('r', "recursive", HelpText = "If included, the utility will recursively create texture packs for each sub folder", DefaultValue = false)]
        public bool Recursive { get; set; }

        [Option('s', "size", Required = false, HelpText = "The maximum size of the generated texture pack", DefaultValue = 1024)]
        public int Size { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("Atlas Texture Packer", "v 1.0.0"),
                Copyright = new CopyrightInfo("Lucas Lundy", 2016),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("MIT");
            help.AddPreOptionsLine("Usage: app -i \\\\Input\\Path -o \\\\Output\\Path");
            help.AddOptions(this);
            return help;
        }
    }
}

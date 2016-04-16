﻿using System;
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
            AtlasArguments arguments = new AtlasArguments();
            
            if (CommandLine.Parser.Default.ParseArguments(args, arguments))
            {
                AtlasCreator.AtlasFormat format = AtlasCreator.AtlasFormat.NONE;

                if(!Enum.TryParse<AtlasCreator.AtlasFormat>(arguments.format, true, out format))
                {
                    Console.WriteLine("Invalid Atlas format. See help -h for valid formats");
                    return;
                }

                AtlasCreator.QuickCreate(arguments.InputPath, arguments.OutputPath, arguments.Size, arguments.Recursive, format);
            }
        }
    }
}

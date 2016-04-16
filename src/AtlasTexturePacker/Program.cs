using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasTexturePacker.Library;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Diagnostics;


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

                if (!IsPowerOfTwo((ulong)arguments.Size))
                {
                    Console.WriteLine("Size is not a power of two");
                    return;
                }

                AtlasCreator.QuickCreate(arguments.InputPath, arguments.OutputPath, arguments.Size, arguments.Recursive, format);

                PromptForEnter();
            }
        }

        static bool IsPowerOfTwo(ulong x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        [Conditional("DEBUG")]
        static void PromptForEnter()
        {
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }
    }
}

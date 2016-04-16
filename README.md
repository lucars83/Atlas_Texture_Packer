## About
This is a library/command line utility for creating texture atlases. The library is also available through Nuget. This utility will pack all .png .jpg .jpeg files in a given folder into a texture atlas.

## Command line arguments

### InputPath / i & OutputPath / o

Inputpath is the full or relative to the images to pack into the texture atlas. The name of the last folder will also be the name of the resulting image. 

OutputPath is the full or relative path to save the resulting texture atlas to.

These are the only required arguments.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output"
* AtlasTexturePsckerCLI --inputPath "C:\Path\To\Images" --outputPath "C:\Path\To\Output"

Will result in the images from "C:\Path\To\Images" being packed into a texture "C:\Path\To\Output\Images.png"

### Recursive / r

Including a recursive flag will result in images in sub folders of the input folder being included in the texture atlas.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output" -r
* AtlasTexturePsckerCLI --inputPath "C:\Path\To\Images" --outputPath "C:\Path\To\Output" -recursive

With an image at "C:\Path\To\Images\sub1\sub2\walk1.png"  
Will create a texture atlas that includes an image with the name "sub1\sub2\walk1"

### Size / s

Specifies the maximum size of the output atlas. Default is 1024. Must be a power of two. Any unused areas will be trimmed, while preserving a power of two size.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output" -s 512
* AtlasTexturePsckerCLI --inputPath "C:\Path\To\Images" --outputPath "C:\Path\To\Output" --size 1024

### Format / f

Specifies the format of the atlas description file. Must be one of the following

* None
* Libgdx

Default value is None.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output" -f None
* AtlasTexturePsckerCLI --inputPath "C:\Path\To\Images" --outputPath "C:\Path\To\Output" --Format Libgdx

## Building With Grunt

### Prereqs

* Nodejs must be installed on your machine
* The grunt-cli must be installed "npm install -g grunt-cli"

### Steps

* Navigate to the Atlas_Texture_Packer
* Do a "npm install"
* Then "grunt rel"

You should now have a build directory with the cli and the library. As well as two nuget packages.

## Using the library

Install the library via nuget or by referencing the dll.

### QuickCreate

Add a using statement for the _AtlasTexturePacker.Library_ and call the function

AtlasCreator.QuickCreate(string inputDir, string outputDir, int maxSize = 1024, bool recursive = true, AtlasFormat format = AtlasFormat.NONE)

These arguments are the same as the command line utility

* inputDir _The path to the images to pack_
* outputDir _The location to save the resulting texture atlas to_
* maxSize _The maximum size of the atlas images_
* recursive _Whether or not to include images in sub directories_
* format _The format to save the description atlas in_

### Atlas Creator

You can also take a look a the QuickCreate function itself if you would like to use the AtlasCreator directly

	// Get a list of images to add to add to the atlas
    string[] images = Directory.GetFiles( inputDir ).Where(x => Regex.IsMatch(x.ToLower(), ImageRegex)).ToArray();
        	
	// Create a name for the atlas
    string atlasName = Path.GetFullPath(inputDir).Substring(inputDir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        	
	// Create a BitmapExtended for each image
	// BitmapExtend is a wrapper around the Bitmap class to add some functionality
	// that is present in Unity texture class but not in the Bitmap class
    BitmapExtended[] textures = images.Select(x => new BitmapExtended(x)).ToArray();

	// Simply creating the atlas here
    AtlasCreator.Atlas[] atlases = AtlasCreator.CreateAtlas(atlasName, textures);

    Dictionary<string, Atlas> atlasesWithNames = new Dictionary<string, Atlas>();

    for(int i = 0; i < atlases.Length; ++i)
    {
	    // Create a file name for each atlas image file and save it 
		// We need it for the SaveAtlasFile so we can associate a description
		// to an image file 
    	string sheetName = i == 0 ? string.Format("{0}.png", atlasName) :
	    	string.Format("{0}{1}.png", atlasName, i);
        atlasesWithNames.Add(sheetName, atlases[i]);

        // create atlas image
        AtlasCreator.SaveAtlas(atlases[i], outputDir, sheetName);

        // create description file
        SaveAtlasFile(atlasesWithNames, outputDir, format);
    }

## Todo

* Find a way to create atlases for Unity

## bugs

* None known


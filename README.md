## About
This is a library/command line utility for creating texture atlases. The library is also available through Nuget. This utility will pack all .png .jpg .jpeg files in a given folder into a texture atlas

## Command line arguments

### InputPath / i & OutputPath / o
Inputpath is the full or relative to the images to pack into the texture atlas. The name of the last folder will also be the name pf the resulting image. 

OutputPath is the full or relative path to save the resulting texture to.

These are the only required arguments.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output"
* AtlasTexturePsckerCLI -inputPath "C:\Path\To\Images" -outputPath "C:\Path\To\Output"

Will result in the images from "C:\Path\To\Images" being packed into a texture "C:\Path\To\Output\Images.png"

### Recursive / r
Including a recursive flag will result in images in sub folders of the input folder being included in the texture atlas.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output" -r
* AtlasTexturePsckerCLI -inputPath "C:\Path\To\Images" -outputPath "C:\Path\To\Output" -recursive

With an image at "C:\Path\To\Images\sub1\sub2\walk1.png"  
Will create a texture atlas that includes an image with the name "sub1\sub2\walk1"
### Size / s

Specifies the maximum size of the output atlas. Default is 1024. Must be a power of two. Any unused areas will be trimmed, while preserving a power of two size.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output" -s 512
* AtlasTexturePsckerCLI -inputPath "C:\Path\To\Images" -outputPath "C:\Path\To\Output" -size 1024

### Format / f

Specifies the format of the atlas description file. Must be one of the following, None, Libgdx. Default value is None.

* AtlasTexturePackerCLI -i "C:\Path\To\Images" -o "C:\Path\To\Output" -f None
* AtlasTexturePsckerCLI -inputPath "C:\Path\To\Images" -outputPath "C:\Path\To\Output" -Format Libgdx

## Building With Grunt

### Prereqs

* Nodejs must be installed on your machine
* The grunt-cli must be installed "npm install -g grunt-cli"

### Steps

* Navigate to the Atlas_Texture_Packer
* do a "npm install"
* then "grunt rel"

You should now have a build directory with the cli and the library. As well as two nuget packages.

## Using the library

Install the library via nuget or by referencing the dll.

### QuickCreate

### Atlas Creator

## Todo

* Documentation

## bugs

* N\A


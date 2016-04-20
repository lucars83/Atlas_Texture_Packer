using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace AtlasTexturePacker.Library
{
    public class BitmapExtended
    {
        Bitmap _bitmap;

        public string Name { get; set; }

        public int Width
        {
            get
            {
                return _bitmap.Width;
            }
        }

        public int Height
        {
            get
            {
                return _bitmap.Height;
            }
        }

        public BitmapExtended(string fileName)
        {
            _bitmap = new Bitmap(fileName);
            Name = Path.GetFileNameWithoutExtension(fileName);
        }
        
        public BitmapExtended(string fileName, string imageName)
        {
        	_bitmap = new Bitmap(fileName);
        	Name = imageName;
        }

        public BitmapExtended(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
        }

        public PixelMap GetPixels()
        {
            PixelMap map = new PixelMap(_bitmap.Width, _bitmap.Height);

            for(int x = 0; x < _bitmap.Width; ++x) //(int x = _bitmap.Width - 1; x >= 0; --x)
            {
                for (int y = 0; y < _bitmap.Height; ++y)
                {
                    map.SetPixel(x, y, _bitmap.GetPixel(x, y));
                }
            }

            return map;
        }
        /*
        public void RotateFlip(RotateFlipType rotation)
        {
            _bitmap.RotateFlip(rotation);
        }
        */
        public void SetPixel(int x, int y, Color color)
        {
            _bitmap.SetPixel(x, y, color);
        }

        public void Save(string path, System.Drawing.Imaging.ImageFormat format)
        {
            _bitmap.Save(path, format);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AtlasTexturePacker.Library
{
    public class PixelMap
    {
        Color[,] _Pixels = null;

        public PixelMap(int width, int height)
        {
            _Pixels = new Color[width, height];
        }

        public int Width
        {
            get
            {
                return _Pixels.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return _Pixels.GetLength(1);
            }
        }

        public Color GetPixel(int x, int y)
        {
            if (x > _Pixels.GetLength(0) || x < 0)
                throw new ArgumentOutOfRangeException("x must be >= 0 and < width");

            if (y > _Pixels.GetLength(1) || y < 0)
                throw new ArgumentOutOfRangeException("y must be >= 0 and < height");

            return _Pixels[x, y];
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (x > _Pixels.GetLength(0) || x < 0)
                throw new ArgumentOutOfRangeException("x must be >= 0 and < width");

            if (y > _Pixels.GetLength(1) || y < 0)
                throw new ArgumentOutOfRangeException("y must be >= 0 and < height");

            _Pixels[x, y] = color;
        }
    }
}

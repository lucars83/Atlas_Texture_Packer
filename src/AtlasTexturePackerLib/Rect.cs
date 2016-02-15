using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasTexturePacker.Library
{
    public class Rect
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public float xMin
        {
            get
            {
                return X;
            }
        }

        public float xMax
        {
            get
            {
                return X + Width;
            }
        }

        public float yMin
        {
            get
            {
                return Y;
            }
        }

        public float yMax
        {
            get
            {
                return Y + Height;
            }
        }

        public Rect(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

    }
}

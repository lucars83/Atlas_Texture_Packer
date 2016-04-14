using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasTexturePacker.Library
{
    public partial class AtlasCreator
    {
        public partial class AtlasNode
        {
            public void Trim(ref BitmapExtended target)
            {
                // Get biggest Y
                float biggestY = GetBiggestYPoint();

                // Get biggest X
                float biggestX = GetBiggestXPoint();

                // Get next largest power of two
                int atlasWidth = biggestX < AtlasCreator.AtlasSize ? GetNextPowerOfTwo(biggestX) : AtlasCreator.AtlasSize;
                int atlasHeight = biggestY < AtlasCreator.AtlasSize ? GetNextPowerOfTwo(biggestY) : AtlasCreator.AtlasSize;

                TrimRecs(atlasWidth, atlasHeight);

                target = new BitmapExtended(atlasWidth, atlasHeight);
            }

            protected float GetBiggestXPoint()
            {
                float biggestX = 0;

                if(name != "Unknown")
                    biggestX = rc.xMax;
                
                if(child != null)
                {
                    if(child[0] != null)
                    {
                        float ch1X = child[0].GetBiggestXPoint();
                        if (ch1X > biggestX)
                            biggestX = ch1X;
                    }
                    if(child[1] != null)
                    {
                        float ch2X = child[1].GetBiggestXPoint();
                        if (ch2X > biggestX)
                            biggestX = ch2X;
                    }
                }

                return biggestX;
            }

            protected float GetBiggestYPoint()
            {
                float biggestY = 0;
                if(name != "Unknown")
                    biggestY = rc.yMax;

                if (child != null)
                {
                    if (child[0] != null)
                    {
                        float ch1Y = child[0].GetBiggestYPoint();
                        if (ch1Y > biggestY)
                            biggestY = ch1Y;
                    }
                    if (child[1] != null)
                    {
                        float ch2Y = child[1].GetBiggestYPoint();
                        if (ch2Y > biggestY)
                            biggestY = ch2Y;
                    }
                }

                return biggestY;
            }

            protected int GetNextPowerOfTwo(float f)
            {
                int powerOfTwo = 2;

                while(powerOfTwo < f)
                {
                    powerOfTwo *= 2;
                }

                return powerOfTwo;
            }

            protected void TrimRecs(float newWidth, float newHeight)
            {
                if(rc != null)
                {
                    if (rc.Width > newWidth)
                        rc.Width = newWidth;
                    if (rc.Height > newHeight)
                        rc.Height = newHeight;
                }

                if(child != null)
                {
                    if(child[0] != null)
                        child[0].TrimRecs(newWidth, newHeight);
                    if (child[1] != null)
                        child[1].TrimRecs(newWidth, newHeight);
                    
                }
            }
        }
    }
}

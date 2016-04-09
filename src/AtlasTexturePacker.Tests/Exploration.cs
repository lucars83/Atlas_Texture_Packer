using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtlasTexturePacker;

namespace AtlasTexturePacker.Tests
{
    [TestClass]
    public class Exploration
    {
        [TestMethod]
        public void SimpleRun()
        {
            AtlasTexturePacker.Library.AtlasCreator.QuickCreate(@"C:\Users\lucar_000\Desktop\AtlasTests\input", "C:\\Users\\lucar_000\\Desktop\\AtlasTests\\output");
        }
    }
}

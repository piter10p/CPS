using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Tests
{
    [TestClass()]
    public class ContentPackReaderTests
    {
        [TestMethod()]
        public void ReadContentPacksBasicTest()
        {
            try
            {
                List<ContentPack> contentPacksList;
                ContentPackReader contentPackReader = new ContentPackReader();
                contentPacksList = contentPackReader.ReadContentPacks("Basic Content Pack Test");

                //Reader result

                Assert.IsFalse(contentPackReader.WarningsExists);
                Assert.AreEqual(contentPacksList.Count, 1);

                //Check content pack data

                ContentPack contentPack = contentPacksList[0];

                Assert.AreEqual("name", contentPack.Name);
                Assert.AreEqual("name.domain.com", contentPack.Id);
                Assert.AreEqual("v 1.0", contentPack.Version);
                Assert.AreEqual("cpsv 1.0", contentPack.CPSVersion);
                Assert.AreEqual("author", contentPack.Author);

                Assert.AreEqual(contentPack.GetElementsOfType("text").Count, 1);

                //Check element Data

                ContentPackElement element = contentPack.GetElementWithId("elementId");

                Assert.AreEqual("elementName", element.Name);
                Assert.AreEqual("elementId", element.Id);
                Assert.AreEqual("text", element.Type);

                //Check resource file
                
                System.IO.Stream stream = element.OpenStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(stream);
                string text = streamReader.ReadLine();
                Assert.AreEqual("This is some content.", text);
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void ReadContentPackBadPackInformationFileTest()
        {
            try
            {
                List<ContentPack> contentPacksList;
                ContentPackReader contentPackReader = new ContentPackReader();
                contentPacksList = contentPackReader.ReadContentPacks("Bad Pack Info File Test");

                //Reader result

                Assert.IsTrue(contentPackReader.WarningsExists);
                Assert.AreEqual(contentPacksList.Count, 0);

                Warning warning = contentPackReader.NextWarning;
                Assert.AreEqual("source.cpack", warning.ContentPackFileName);
                Assert.IsFalse(contentPackReader.WarningsExists);
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
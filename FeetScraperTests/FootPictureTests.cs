using FeetScraper.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using static FeetScraper.Logic.Constants;
using static FeetScraperTests.TestFunctions.HelperFunctions;
using FeetScraper;
using System.IO;

namespace FeetScraperTests
{
    [TestFixture]
    public class FootPictureTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void DownloadFootpictureTests()
        {
            MockHttpMessageHandler mockhttp = new();

            MemoryStream ms = new();
            StreamWriter writer = new(ms);

            ms.Read(GetEmbeddedPicture("testpicture.jpg"), 0, GetEmbeddedPicture("testpicture.jpg").Length);

            mockhttp.When($"{FEET_PICTURE_URL}Jeri_Ryan-Feet-14584789.jpg").Respond("image/jpeg", ms);

            FootPicture fp = new()
            {
                Name = "Jeri Ryan",
                Height = 2547,
                Width = 3000,
                Id = 14584789,
                FootTags = new List<FootTag>() { new('T'), new('C') }
            };

            byte[] picture = null;

            Assert.DoesNotThrowAsync(async () => picture = await fp.DownloadFootPictureAsync(new(mockhttp)));

            ms.Dispose();
            writer.Dispose();
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}

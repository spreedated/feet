using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using FeetScraper.Logic;
using RichardSzalay.MockHttp;
using static UnitTests.TestFunctions.HelperFunctions;
using HtmlAgilityPack;
using FeetScraper.Models;

namespace UnitTests
{
    [TestFixture]
    public class PageTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ProcessFailedTests()
        {
            MockHttpMessageHandler mockhttp = new();
            mockhttp.When($"{Constants.FEET_URL}foo").Respond("text/plain", "");
            FeetPage s = new("foo")
            {
                httpClient = mockhttp.ToHttpClient()
            };

            PageResponse res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.RetrieveAsync());
                Assert.That(res.Feet, Is.Empty);
            });

            mockhttp = new();
            mockhttp.When($"{Constants.FEET_URL}foo").Respond("text/plain", "<html><p>foobar</p></html>");
            s = new("foo")
            {
                httpClient = mockhttp.ToHttpClient()
            };

            res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.RetrieveAsync());
                Assert.That(res.Feet, Is.Empty);
            });
        }

        [Test]
        public void ProcessSuccessTests()
        {
            MockHttpMessageHandler mockhttp = new();
            mockhttp.When($"{Constants.FEET_URL}foo").Respond("text/html", GetEmbeddedHtml("KnownFeetResult.html"));
            FeetPage s = new("foo")
            {
                httpClient = mockhttp.ToHttpClient()
            };

            PageResponse res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.RetrieveAsync());
                Assert.That(res.Feet.Count(x => x.FootTags.Any(y => y.Id == 'T')), Is.EqualTo(14));
                Assert.That(res.ShoeSize, Is.EqualTo(9));
                Assert.That(res.Feet.Count(), Is.EqualTo(558));
                Assert.That(res.Birthday, Is.EqualTo(new DateTime(1968, 2, 22)));
                Assert.That(res.Birthplace, Is.EqualTo("United States"));
            });
        }

        [Test]
        public void RetrieveFootPicturesTests()
        {
            Assert.That(FeetPage.RetrieveFootPictures(null, "foobar"), Is.Empty);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetPage.RetrieveFootPictures(doc.DocumentNode, "foobar"), Is.Empty);

            doc.LoadHtml("");
            Assert.That(FeetPage.RetrieveFootPictures(doc.DocumentNode, "foobar"), Is.Empty);

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html"));

            IEnumerable<FootPicture> footPictures = FeetPage.RetrieveFootPictures(doc.DocumentNode, "foobar");

            Assert.That(footPictures.Count(), Is.EqualTo(558));
            Assert.That(footPictures.Any(x => x.Name == null || x.Name != "foobar"), Is.False);
        }

        [Test]
        public void RetrieveFeetpicCountTests()
        {
            Assert.That(FeetPage.RetrieveFeetPicsCount(null), Is.EqualTo(0));

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetPage.RetrieveFeetPicsCount(doc.DocumentNode), Is.EqualTo(0));

            doc.LoadHtml("");
            Assert.That(FeetPage.RetrieveFeetPicsCount(doc.DocumentNode), Is.EqualTo(0));

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html"));
            Assert.That(FeetPage.RetrieveFeetPicsCount(doc.DocumentNode), Is.EqualTo(558));
        }

        [Test]
        public void RetrieveShoesizeTests()
        {
            Assert.That(FeetPage.RetrieveShoesize(null), Is.Null);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetPage.RetrieveShoesize(doc.DocumentNode), Is.Null);

            doc.LoadHtml("");
            Assert.That(FeetPage.RetrieveShoesize(doc.DocumentNode), Is.Null);

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html"));
            Assert.That(FeetPage.RetrieveShoesize(doc.DocumentNode), Is.Not.Null);
            Assert.That(FeetPage.RetrieveShoesize(doc.DocumentNode), Is.EqualTo(9f));

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html").Replace("9 US", "14.5 US"));
            Assert.That(FeetPage.RetrieveShoesize(doc.DocumentNode), Is.Not.Null);
            Assert.That(FeetPage.RetrieveShoesize(doc.DocumentNode), Is.EqualTo(14.5f));
        }

        [Test]
        public void RetrieveFeetRatingStatsTests()
        {
            Assert.That(FeetPage.RetrieveFeetRatingStats(null), Is.Null);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetPage.RetrieveFeetRatingStats(doc.DocumentNode), Is.Null);

            doc.LoadHtml("");
            Assert.That(FeetPage.RetrieveFeetRatingStats(doc.DocumentNode), Is.Null);

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html"));
            Assert.That(FeetPage.RetrieveFeetRatingStats(doc.DocumentNode), Is.Not.Null);
            Assert.That(FeetPage.RetrieveFeetRatingStats(doc.DocumentNode).Total, Is.EqualTo(1413));
        }

        [Test]
        public void RetrieveBirthplaceTests()
        {
            Assert.That(FeetPage.RetrieveBirthplace(null), Is.Null);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetPage.RetrieveBirthplace(doc.DocumentNode), Is.Null);

            doc.LoadHtml("");
            Assert.That(FeetPage.RetrieveBirthplace(doc.DocumentNode), Is.Null);

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html"));
            Assert.That(FeetPage.RetrieveBirthplace(doc.DocumentNode), Is.Not.Null);
            Assert.That(FeetPage.RetrieveBirthplace(doc.DocumentNode), Is.EqualTo("United States"));
        }

        [Test]
        public void RetrieveBirthdayTests()
        {
            Assert.That(FeetPage.RetrieveBirthday(null), Is.Null);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetPage.RetrieveBirthday(doc.DocumentNode), Is.Null);

            doc.LoadHtml("");
            Assert.That(FeetPage.RetrieveBirthday(doc.DocumentNode), Is.Null);

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html"));
            Assert.That(FeetPage.RetrieveBirthday(doc.DocumentNode), Is.Not.Null);
            Assert.That(FeetPage.RetrieveBirthday(doc.DocumentNode), Is.EqualTo(new DateTime(1968,2,22)));
        }

        [Test]
        public void RetrieveNameTests()
        {
            Assert.That(FeetPage.RetrieveName(null), Is.Null);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetPage.RetrieveName(doc.DocumentNode), Is.Null);

            doc.LoadHtml("");
            Assert.That(FeetPage.RetrieveName(doc.DocumentNode), Is.Null);

            doc.LoadHtml(GetEmbeddedHtml("KnownFeetResult.html"));
            Assert.That(FeetPage.RetrieveName(doc.DocumentNode), Is.Not.Null);
            Assert.That(FeetPage.RetrieveName(doc.DocumentNode), Is.EqualTo("Jeri Ryan"));
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}

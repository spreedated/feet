using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using FeetScraper.Logic;
using static FeetScraper.Logic.Constants;
using RichardSzalay.MockHttp;
using static UnitTests.TestFunctions.HelperFunctions;
using HtmlAgilityPack;
using FeetScraper.Models;
using System.Net;

namespace UnitTests
{
    [TestFixture]
    public class SearchTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void SearchFailedTests()
        {
            MockHttpMessageHandler mockhttp = new();
            mockhttp.When($"{Constants.FEET_URL}search/foo").Respond("text/html", "");
            FeetSearch s = new()
            {
                httpClient = mockhttp.ToHttpClient()
            };

            IEnumerable<SearchResponse> res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
                Assert.That(res.Count(), Is.EqualTo(0));
            });

            mockhttp.When($"{Constants.FEET_URL}search/foo").Respond("text/plain", "<html>nothing</html>");
            s = new()
            {
                httpClient = mockhttp.ToHttpClient()
            };

            res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
                Assert.That(res.Count(), Is.EqualTo(0));
            });

            mockhttp.When($"{Constants.FEET_URL}search/foo").Respond(HttpStatusCode.BadGateway);
            s = new()
            {
                httpClient = mockhttp.ToHttpClient()
            };

            res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
                Assert.That(res.Count(), Is.EqualTo(0));
            });

            mockhttp.When($"{Constants.FEET_URL}search/foo").Respond(HttpStatusCode.BadRequest);
            s = new()
            {
                httpClient = mockhttp.ToHttpClient()
            };

            IEnumerable<SearchResponse> res0 = null;

            s.Completed += (o,e) => res0 = e.SearchResponses;

            res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
                Assert.That(res, Is.Empty);
                Assert.That(res.Count(), Is.EqualTo(0));
                Assert.That(res0, Is.Empty);
                Assert.That(res0.Count(), Is.EqualTo(0));
            });
        }

        [Test]
        public void SearchSuccessTests()
        {
            MockHttpMessageHandler mockhttp = new();
            mockhttp.When($"{Constants.FEET_URL}search/foo").Respond("text/html", GetEmbeddedHtml("SearchResult.html"));
            FeetSearch s = new()
            {
                httpClient = mockhttp.ToHttpClient()
            };

            IEnumerable<SearchResponse> res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
                Assert.That(res.Count(), Is.EqualTo(12));
                Assert.That(res.Any(x => x.Piccount == 0), Is.True);
                Assert.That(res.Count(x => x.Piccount == 0), Is.EqualTo(1));
                Assert.That(res.Count(x => x.Piccount != 0), Is.EqualTo(11));
            });
        }

        [Test]
        public void RetrieveRatingTests()
        {
            Assert.That(FeetSearch.RetrieveRating(null), Is.Null);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetSearch.RetrieveRating(doc.DocumentNode), Is.Null);

            doc.LoadHtml("");
            Assert.That(FeetSearch.RetrieveRating(doc.DocumentNode), Is.Null);

            //beautiful feet (108 - 97)
            doc.LoadHtml("<html><div class=\"boxtitle\"><div><div><div style=\"width:104px; height:20px; background-image:url(/images/stars1.gif); background-position:top left; background-repeat:no-repeat; float:left\"></div></div></div></div></html>");
            FeetRating res = FeetSearch.RetrieveRating(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.Width, Is.EqualTo(104));
                Assert.That(res.Percent, Is.GreaterThan(96d));
                Assert.That(res.PercentString.StartsWith("96"), Is.True);
                Assert.That(res.PercentString.EndsWith("%"), Is.True);
                Assert.That(res.FeetSiteRating, Is.EqualTo(FeetRating.FeetSiteRatings.Beautiful));
            });

            //nice feet (96 - 75)
            doc.LoadHtml("<html><div class=\"boxtitle\"><div><div><div style=\"width:78px; height:20px; background-image:url(/images/stars1.gif); background-position:top left; background-repeat:no-repeat; float:left\"></div></div></div></div></html>");
            res = FeetSearch.RetrieveRating(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.Width, Is.EqualTo(78));
                Assert.That(res.Percent, Is.GreaterThan(72d));
                Assert.That(res.PercentString.StartsWith("72"), Is.True);
                Assert.That(res.PercentString.EndsWith("%"), Is.True);
                Assert.That(res.FeetSiteRating, Is.EqualTo(FeetRating.FeetSiteRatings.Nice));
            });

            //ok feet (74 - 54)
            doc.LoadHtml("<html><div class=\"boxtitle\"><div><div><div style=\"width:54px; height:20px; background-image:url(/images/stars1.gif); background-position:top left; background-repeat:no-repeat; float:left\"></div></div></div></div></html>");
            res = FeetSearch.RetrieveRating(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.Width, Is.EqualTo(54));
                Assert.That(res.Percent, Is.EqualTo(50d));
                Assert.That(res.PercentString.StartsWith("50"), Is.True);
                Assert.That(res.PercentString.EndsWith("%"), Is.True);
                Assert.That(res.FeetSiteRating, Is.EqualTo(FeetRating.FeetSiteRatings.Ok));
            });

            //bad feet (53 - 32)
            doc.LoadHtml("<html><div class=\"boxtitle\"><div><div><div style=\"width:46px; height:20px; background-image:url(/images/stars1.gif); background-position:top left; background-repeat:no-repeat; float:left\"></div></div></div></div></html>");
            res = FeetSearch.RetrieveRating(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.Width, Is.EqualTo(46));
                Assert.That(res.Percent, Is.GreaterThan(42d));
                Assert.That(res.PercentString.StartsWith("42"), Is.True);
                Assert.That(res.PercentString.EndsWith("%"), Is.True);
                Assert.That(res.FeetSiteRating, Is.EqualTo(FeetRating.FeetSiteRatings.Bad));
            });

            //ugly feet (31 - 1)
            doc.LoadHtml("<html><div class=\"boxtitle\"><div><div><div style=\"width:24px; height:20px; background-image:url(/images/stars1.gif); background-position:top left; background-repeat:no-repeat; float:left\"></div></div></div></div></html>");
            res = FeetSearch.RetrieveRating(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.Width, Is.EqualTo(24));
                Assert.That(res.Percent, Is.GreaterThan(22d));
                Assert.That(res.PercentString.StartsWith("22"), Is.True);
                Assert.That(res.PercentString.EndsWith("%"), Is.True);
                Assert.That(res.FeetSiteRating, Is.EqualTo(FeetRating.FeetSiteRatings.Ugly));
            });

            //unrated
            doc.LoadHtml("<html><div class=\"boxtitle\"><div><div><img style=\"float:left\" src=\"/images/stars0.gif\"/></div></div></div></html>");
            res = FeetSearch.RetrieveRating(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.Width, Is.EqualTo(0));
                Assert.That(res.Percent, Is.EqualTo(0d));
                Assert.That(res.PercentString.StartsWith("0"), Is.True);
                Assert.That(res.PercentString.EndsWith("%"), Is.True);
                Assert.That(res.FeetSiteRating, Is.EqualTo(FeetRating.FeetSiteRatings.Unrated));
            });

            //gorgeous
            doc.LoadHtml("<html><div class=\"boxtitle\"><div><div><img style=\"float:left\" src=\"/images/stars2.gif\"/></div></div></div></html>");
            res = FeetSearch.RetrieveRating(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.Width, Is.EqualTo(FEET_GORGEOUS_RATING_WIDTH));
                Assert.That(res.PercentString.EndsWith("%"), Is.True);
                Assert.That(res.FeetSiteRating, Is.EqualTo(FeetRating.FeetSiteRatings.Gorgeous));
            });
        }

        [Test]
        public void RetrieveNameTests()
        {
            Assert.That(FeetSearch.RetrieveName(null), Is.Null);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetSearch.RetrieveName(doc.DocumentNode), Is.Null);

            doc.LoadHtml("");
            Assert.That(FeetSearch.RetrieveName(doc.DocumentNode), Is.Null);

            doc.LoadHtml("<html><div class=\"boxtitle\"><div><a href=\"/foobar\"><span style=\"color:white\">Foo</span> Bar</a><small>(360 pics)</small></div></div></html>");
            Assert.That(FeetSearch.RetrieveName(doc.DocumentNode), Is.Not.Null);
            Assert.That(FeetSearch.RetrieveName(doc.DocumentNode), Is.EqualTo("Foo Bar"));
        }

        [Test]
        public void RetrieveThumbnailsTests()
        {
            Assert.That(FeetSearch.RetrieveThumbnails(null), Is.Empty);

            HtmlDocument doc = new();

            doc.LoadHtml("<html>foobar</html>");
            Assert.That(FeetSearch.RetrieveThumbnails(doc.DocumentNode), Is.Empty);

            doc.LoadHtml("");
            Assert.That(FeetSearch.RetrieveThumbnails(doc.DocumentNode), Is.Empty);

            doc.LoadHtml("<html><div class=\"boxcont\"><a href=\"/foober\"><div style=\"background-image:url('https://thumbs.wikifeet.com/123456.jpg')\"></div><div style=\"background-image:url('https://thumbs.wikifeet.com/789123.jpg')\"></div><div style=\"background-image:url('https://thumbs.wikifeet.com/1547895.jpg')\"></div></a></div></html>");
            IEnumerable<string> res = FeetSearch.RetrieveThumbnails(doc.DocumentNode);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Empty);
                Assert.That(res.Count(), Is.EqualTo(3));
                Assert.That(res.Any(x => x.Contains("123456.jpg")), Is.True);
                Assert.That(res.Any(x => x.Contains("789123.jpg")), Is.True);
                Assert.That(res.Any(x => x.Contains("1547895.jpg")), Is.True);
            });
        }

        [Test]
        public void RetrievePiccountTests()
        {
            Assert.That(FeetSearch.RetrievePiccount(null), Is.EqualTo(0));

            HtmlDocument doc = new();
            doc.LoadHtml("<html>foobar</html>");

            Assert.That(FeetSearch.RetrievePiccount(doc.DocumentNode), Is.EqualTo(0));

            doc.LoadHtml("");
            Assert.That(FeetSearch.RetrievePiccount(doc.DocumentNode), Is.EqualTo(0));

            doc.LoadHtml("<html><div class=\"boxtitle\"><div><small>(360 pics)</small></div></div></html>");
            Assert.That(FeetSearch.RetrievePiccount(doc.DocumentNode), Is.EqualTo(360));

            doc.LoadHtml("<html><div class=\"boxtitle\"><div><small>360 pics</small></div></div></html>");
            Assert.That(FeetSearch.RetrievePiccount(doc.DocumentNode), Is.EqualTo(360));

            doc.LoadHtml("<html><div class=\"boxtitle\"><div><small>360</small></div></div></html>");
            Assert.That(FeetSearch.RetrievePiccount(doc.DocumentNode), Is.EqualTo(360));
        }

        [Test]
        public void RetrieveUpdatedDateTests()
        {
            DateTime dtDefault = new();

            Assert.That(FeetSearch.RetrieveUpdatedDate(null), Is.EqualTo(dtDefault));

            HtmlDocument doc = new();
            doc.LoadHtml("<html>foobar</html>");

            Assert.That(FeetSearch.RetrieveUpdatedDate(doc.DocumentNode), Is.EqualTo(dtDefault));

            doc.LoadHtml("");
            Assert.That(FeetSearch.RetrieveUpdatedDate(doc.DocumentNode), Is.EqualTo(dtDefault));

            doc.LoadHtml("<html><div class=\"boxcont\"><div>Updated: 2012-03-03</div></div></html>");
            Assert.That(FeetSearch.RetrieveUpdatedDate(doc.DocumentNode).Date, Is.EqualTo(new DateTime(2012,3,3).Date));

            doc.LoadHtml("<html><div class=\"boxcont\"><div>2012-03-03</div></div></html>");
            Assert.That(FeetSearch.RetrieveUpdatedDate(doc.DocumentNode).Date, Is.EqualTo(new DateTime(2012, 3, 3).Date));

            doc.LoadHtml("<html><div class=\"boxcont\"><div>2012-12-15</div></div></html>");
            Assert.That(FeetSearch.RetrieveUpdatedDate(doc.DocumentNode).Date, Is.EqualTo(new DateTime(2012, 12, 15).Date));
        }

        [Test]
        public void RetrievePagelinkTests()
        {
            Assert.That(FeetSearch.RetrievePagelink(null), Is.EqualTo(null));

            HtmlDocument doc = new();
            doc.LoadHtml("<html>foobar</html>");

            Assert.That(FeetSearch.RetrievePagelink(doc.DocumentNode), Is.EqualTo(null));

            doc.LoadHtml("");
            Assert.That(FeetSearch.RetrievePagelink(doc.DocumentNode), Is.EqualTo(null));

            doc.LoadHtml("<html><div class=\"boxtitle\"><div>something</div><div><a href=\"/foobar\"></a></div></div></html>");
            Assert.That(FeetSearch.RetrievePagelink(doc.DocumentNode), Is.EqualTo($"{FEET_URL}foobar"));

            doc.LoadHtml("<html><div class=\"boxtitle\"><div><a href=\"/foobar\"></a></div></div></html>");
            Assert.That(FeetSearch.RetrievePagelink(doc.DocumentNode), Is.EqualTo($"{FEET_URL}foobar"));
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}

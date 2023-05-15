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
    public class PageTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        //[Test]
        //public void SearchFailedTests()
        //{
        //    MockHttpMessageHandler mockhttp = new();
        //    mockhttp.When($"{Constants.FEET_URL}search/foo").Respond("text/html", "");
        //    FeetSearch s = new()
        //    {
        //        httpClient = mockhttp.ToHttpClient()
        //    };

        //    IEnumerable<SearchResponse> res = null;

        //    Assert.Multiple(() =>
        //    {
        //        Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
        //        Assert.That(res.Count(), Is.EqualTo(0));
        //    });

        //    mockhttp.When($"{Constants.FEET_URL}search/foo").Respond("text/plain", "<html>nothing</html>");
        //    s = new()
        //    {
        //        httpClient = mockhttp.ToHttpClient()
        //    };

        //    res = null;

        //    Assert.Multiple(() =>
        //    {
        //        Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
        //        Assert.That(res.Count(), Is.EqualTo(0));
        //    });

        //    mockhttp.When($"{Constants.FEET_URL}search/foo").Respond(HttpStatusCode.BadGateway);
        //    s = new()
        //    {
        //        httpClient = mockhttp.ToHttpClient()
        //    };

        //    res = null;

        //    Assert.Multiple(() =>
        //    {
        //        Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
        //        Assert.That(res.Count(), Is.EqualTo(0));
        //    });

        //    mockhttp.When($"{Constants.FEET_URL}search/foo").Respond(HttpStatusCode.BadRequest);
        //    s = new()
        //    {
        //        httpClient = mockhttp.ToHttpClient()
        //    };

        //    IEnumerable<SearchResponse> res0 = null;

        //    s.Completed += (o,e) => res0 = e.SearchResponses;

        //    res = null;

        //    Assert.Multiple(() =>
        //    {
        //        Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
        //        Assert.That(res, Is.Empty);
        //        Assert.That(res.Count(), Is.EqualTo(0));
        //        Assert.That(res0, Is.Empty);
        //        Assert.That(res0.Count(), Is.EqualTo(0));
        //    });
        //}

        //[Test]
        //public void SearchSuccessTests()
        //{
        //    MockHttpMessageHandler mockhttp = new();
        //    mockhttp.When($"{Constants.FEET_URL}search/foo").Respond("text/html", GetEmbeddedHtml("SearchResult.html"));
        //    FeetSearch s = new()
        //    {
        //        httpClient = mockhttp.ToHttpClient()
        //    };

        //    IEnumerable<SearchResponse> res = null;

        //    Assert.Multiple(() =>
        //    {
        //        Assert.DoesNotThrowAsync(async () => res = await s.SearchAsync("foo"));
        //        Assert.That(res.Count(), Is.EqualTo(12));
        //        Assert.That(res.Any(x => x.Piccount == 0), Is.True);
        //        Assert.That(res.Count(x => x.Piccount == 0), Is.EqualTo(1));
        //        Assert.That(res.Count(x => x.Piccount != 0), Is.EqualTo(11));
        //    });
        //}

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

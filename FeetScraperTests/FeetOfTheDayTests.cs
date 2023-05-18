using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using FeetScraper.Logic;
using RichardSzalay.MockHttp;
using static FeetScraperTests.TestFunctions.HelperFunctions;
using HtmlAgilityPack;
using FeetScraper.Models;

namespace FeetScraperTests
{
    [TestFixture]
    public class FeetOfTheDayTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ProcessSuccessTests()
        {
            MockHttpMessageHandler mockhttp = new();
            mockhttp.When($"{Constants.FEET_URL}").Respond("text/html", GetEmbeddedHtml("Homepage.html"));
            FeetScraper.Logic.FeetOfTheDay s = new()
            {
                httpClient = mockhttp.ToHttpClient()
            };

            FeetScraper.Models.FeetOfTheDay res = null;

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(async () => res = await s.RetrieveAsync());
                Assert.That(res.Name, Is.EqualTo("Seema Khan"));
                Assert.That(res.Link, Is.EqualTo("https://www.wikifeet.com/Seema_Khan"));
            });

        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}

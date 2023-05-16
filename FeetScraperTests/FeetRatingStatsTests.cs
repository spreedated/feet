using FeetScraper.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeetScraperTests
{
    [TestFixture]
    public class FeetRatingStatsTests
    {
        private int b;
        private int n;
        private int o;
        private int ba;
        private int u;

        [SetUp]
        public void SetUp()
        {
            Random rnd = new(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));
            this.b = rnd.Next(32, 900);
            this.n = rnd.Next(32, 900);
            this.o = rnd.Next(32, 900);
            this.ba = rnd.Next(32, 900);
            this.u = rnd.Next(32, 900);
        }

        [Test]
        public void TotalCalculationTests()
        {
            FeetRatingStats f = new();

            Assert.That(f.Total, Is.EqualTo(0));

            f.Beautiful = this.b;
            Assert.That(f.Total, Is.EqualTo(this.b));

            f.Nice = this.n;
            Assert.That(f.Total, Is.EqualTo(this.b + this.n));

            f.Okay = this.o;
            Assert.That(f.Total, Is.EqualTo(this.b + this.n + this.o));

            f.Bad = this.ba;
            Assert.That(f.Total, Is.EqualTo(this.b + this.n + this.o + this.ba));

            f.Ugly = this.u;
            Assert.That(f.Total, Is.EqualTo(this.b + this.n + this.o + this.ba + this.u));
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}

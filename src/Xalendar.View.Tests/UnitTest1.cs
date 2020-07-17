using NUnit.Framework;

namespace Xalendar.View.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var temp = new Temp();
            
            Assert.IsTrue(temp.Check());
        }
    }
}

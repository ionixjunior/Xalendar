using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xalendar.Extensions;

namespace Xalendar.Tests.Extensions
{
    [TestFixture]
    public class IEnumerableExtensionTests
    {
        private IEnumerable<int> _list;
        
        [Test]
        public void ShouldBeTrueWhenListIsNull()
        {
            var result = _list.IsNullOrEmpty();
            
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldBeTrueWhenListIsEmpty()
        {
            _list = Enumerable.Empty<int>();

            var result = _list.IsNullOrEmpty();
            
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldBeFalseWhenListHasItems()
        {
            _list = Enumerable.Range(1, 10);

            var result = _list.IsNullOrEmpty();
            
            Assert.IsFalse(result);
        }
    }
}

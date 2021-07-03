using System;
using System.Reflection;
using Xunit;

namespace ProjNetSupplement.Test.Tests
{
    public class WktCacheTest
    {
        [Theory]
        [InlineData(4301, true, "Tokyo")]
        [InlineData(4326, true, "WGS_1984")]
        [InlineData(900913, true, "Google")]
        [InlineData(192939111, false, "")]
        public void TryGet(int srid, bool expected, string contains)
        {
            var wktCache = new WktCache();
            var actual = wktCache.TryGet(srid, out var wkt);
            
            Assert.Equal(expected, actual);
            if (actual)
            {
                Assert.Contains(contains, wkt, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
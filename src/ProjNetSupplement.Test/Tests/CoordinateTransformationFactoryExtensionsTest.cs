using ProjNet;
using ProjNet.CoordinateSystems.Transformations;
using Xunit;

namespace ProjNetSupplement.Test.Tests
{
    public class CoordinateTransformationFactoryExtensionsTest
    {
        [Theory]
        [InlineData(Srid.Tokyo, Srid.Wgs84, 6, 139.767337, 35.681231, 139.764103, 35.6844699)]
        [InlineData(Srid.Tokyo, Srid.Jgd2000, 6, 139.767337, 35.681231, 139.764103, 35.6844699)]
        [InlineData(Srid.Tokyo, Srid.Jgd2011, 6, 139.767337, 35.681231, 139.764103, 35.6844699)]
        // [InlineData(Srid.Tokyo, Srid.Google, 2, 139.767337, 35.681231, 15558468.78, 4257291.32)]
        public void Test(Srid source, Srid target, int precision, double x, double y, double expectedX, double expectedY)
        {
            var factory = new CoordinateTransformationFactory();
            var transformation = factory.CreateFromSrid(source, target);
            var (actualX, actualY) = transformation.MathTransform.Transform(x, y);
            
            Assert.Equal(expectedX, actualX, precision);
            Assert.Equal(expectedY, actualY, precision);
        }
    }
}
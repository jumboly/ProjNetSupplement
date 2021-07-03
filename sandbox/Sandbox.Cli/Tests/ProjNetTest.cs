using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.IO.CoordinateSystems;

namespace Sandbox.Cli.Tests
{
    public class ProjNetTest : ConsoleAppBase
    {
        private readonly ILogger<ProjNetTest> _logger;

        public ProjNetTest(ILogger<ProjNetTest> logger)
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("{0}", nameof(ProjNetTest));

            var wkt = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]";
            var coordinateSystem = CoordinateSystemWktReader.Parse(wkt) as GeographicCoordinateSystem;
            _logger.LogInformation("{0}", new
            {
                coordinateSystem.WKT
            });

            var factory = new CoordinateTransformationFactory();
            var coordinateTransformation = factory.CreateFromCoordinateSystems(coordinateSystem, coordinateSystem);

            var (lng, lat) = coordinateTransformation.MathTransform.Transform(135, 34);
            _logger.LogInformation("{0}", new{lng, lat});
        }
    }
}
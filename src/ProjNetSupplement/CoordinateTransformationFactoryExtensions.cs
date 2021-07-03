using System;
using System.Collections.Concurrent;
using ProjNet.IO.CoordinateSystems;
using ProjNetSupplement;

// ReSharper disable once CheckNamespace
namespace ProjNet.CoordinateSystems.Transformations
{
    public static class CoordinateTransformationFactoryExtensions
    {
        private static readonly ConcurrentDictionary<int, CoordinateSystem> CoordinateSystems = new();
        
        public static ICoordinateTransformation CreateFromSrid(this CoordinateTransformationFactory factory, Srid source, Srid target, WktCache wktCache = null)
        {
            return CreateFromSrid(factory, (int) source, (int) target);
        }

        public static ICoordinateTransformation CreateFromSrid(this CoordinateTransformationFactory factory, int source, int target, WktCache wktCache = null)
        {
            wktCache ??= WktCache.Default;
            
            var sourceCoordinateSystem = GetCoordinateSystem(source, wktCache);
            var targetCoordinateSystem = GetCoordinateSystem(target, wktCache);
            
            return factory.CreateFromCoordinateSystems(sourceCoordinateSystem, targetCoordinateSystem);
        }

        private static CoordinateSystem GetCoordinateSystem(int srid, WktCache wktCache)
        {
            return CoordinateSystems.GetOrAdd(srid, key =>
            {
                if (!wktCache.TryGet(key, out var wkt))
                {
                    throw new ArgumentException($"不明なSrid:{key}", nameof(srid));
                }

                var coordinateSystem = CoordinateSystemWktReader.Parse(wkt) as CoordinateSystem;
                if (coordinateSystem == null)
                {
                    throw new ArgumentException($"不正なWkt:{wkt}", nameof(srid));
                }

                return coordinateSystem;
            });
        }
    }
}
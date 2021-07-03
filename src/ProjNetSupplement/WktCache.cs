using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace ProjNetSupplement
{
    public class WktCache
    {
        private static readonly Lazy<WktCache> _default = new ();
        public static WktCache Default => _default.Value; 
        
        private readonly ConcurrentDictionary<int, string> _cache = new();

        public bool TryGet(int srid, out string wkt)
        {
            var ret = _cache.TryGetValue(srid, out wkt);
            if (ret)
            {
                return true;
            }

            try
            {
                wkt = _cache.GetOrAdd(srid, key =>
                {
                    var type = GetType();
                    
                    var name = $"{type.Namespace}.wkt.{srid}.wkt";
                    
                    using var stream = type.Assembly.GetManifestResourceStream(name);
                    if (stream == null)
                    {
                        throw new Exception("not found");
                    }

                    using var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
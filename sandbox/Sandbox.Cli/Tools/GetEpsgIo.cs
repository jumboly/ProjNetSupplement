using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using ProjNet.CoordinateSystems;
using ProjNet.IO.CoordinateSystems;

namespace Sandbox.Cli.Tools
{
    public class GetEpsgIo : ConsoleAppBase
    {
        private readonly ILogger<GetEpsgIo> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public GetEpsgIo(ILogger<GetEpsgIo> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Run(string folder, string[] srid)
        {
            _logger.LogInformation("args={0}", new {folder, srid=string.Join(",", srid)});

            var fullPath = Path.GetFullPath(folder);
            if (!Directory.Exists(fullPath))
            {
                _logger.LogInformation("フォルダ作成:{0}", fullPath);
                Directory.CreateDirectory(fullPath);
            }

            using var http = _httpClientFactory.CreateClient();

            await using var csproj = new StreamWriter(Path.Combine(Path.Combine(fullPath, "wkt.csproj")));
            await csproj.WriteLineAsync("<ItemGroup>");

            foreach (var id in srid)
            {
                var ids = id.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (ids.Length == 2)
                {
                    // from-to を展開
                    
                    var from = Convert.ToInt32(ids.First());
                    var to = Convert.ToInt32(ids.Last());

                    ids = Enumerable.Range(from, to - from + 1)
                        .Select(it => it.ToString())
                        .ToArray();
                }
                
                foreach (var i in ids)
                {
                    var url = $"https://epsg.io/{i}.wkt";
                    var wkt = await http.GetStringAsync(url);

                    IInfo info;
                    try
                    {
                        info = CoordinateSystemWktReader.Parse(wkt);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"不正なwkt:{i}:{wkt.Substring(0, Math.Min(30, wkt.Length))}...");
                    }
                    _logger.LogInformation("{0}", new {srid = i, info.Name, info.Authority});

                    var path = Path.Combine(fullPath, $"{i}.wkt");
                    
                    _logger.LogInformation("出力:{0}", path);
                    await File.WriteAllTextAsync(path, wkt, Encoding.ASCII);
                    
                    // csproj に貼り付ける断片
                    await csproj.WriteLineAsync($"  <None Remove=\"{folder}\\{i}.wkt\" />");
                    await csproj.WriteLineAsync($"  <EmbeddedResource Include=\"{folder}\\{i}.wkt\" />");
                }
            }

            await csproj.WriteLineAsync("</ItemGroup>");
        }
    }
}
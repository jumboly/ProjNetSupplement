using System;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sandbox.Cli.Tests;

namespace Sandbox.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
#if DEBUG
            args = new[]
            {
                // "ProjNetTest.Run",
                // "ResourceTest.Run",

                "GetEpsgIo.Run",
                    "-folder", @".\wkt", 
                    "-srid", "3857,900913" // 球面メルカトル
                             + ",4326,32651-32656"   // WGS84, UTM
                             + ",4301,3092-3096,30161-30179" // Tokyo, UTM, 平面直角座標
                             + ",4612,3097-3101,2443-2461" // JGD2000, UTM, 平面直角座標
                             + ",6668,6688-6692,6669-6687" // JGD2011, UTM, 平面直角座標
            };
#endif
            await Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ReplaceToSimpleConsole();
                })
                .ConfigureServices(services =>
                {
                    services.AddHttpClient();
                })
                .RunConsoleAppFrameworkAsync(args);
        }
    }
}
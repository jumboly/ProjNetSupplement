using System.IO;
using System.Reflection;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging;

namespace Sandbox.Cli.Tests
{
    public class ResourceTest : ConsoleAppBase
    {
        private readonly ILogger<ResourceTest> _logger;

        public ResourceTest(ILogger<ResourceTest> logger)
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation(nameof(ResourceTest));

            var asm = Assembly.GetExecutingAssembly();
            foreach (var name in asm.GetManifestResourceNames())
            {
                _logger.LogInformation("resource={0}", name);

                var txt = new StreamReader(asm.GetManifestResourceStream(name)).ReadToEnd();
                _logger.LogInformation("txt={0}", txt);
            }
        }
    }
}
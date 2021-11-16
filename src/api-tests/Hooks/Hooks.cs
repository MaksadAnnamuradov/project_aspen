using Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Tests.Steps;

namespace Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private static IHost host;
        public static int ExposedPort { get; private set; }
        private static ILogger<Hooks> logger = TestLogger.Create<Hooks>();

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            logger.LogInformation("Starting up web server on random port...");
            host = Program.CreateHostBuilder(new[] {"--urls", "http://127.0.0.1:0"}).Build();
            host.Start();
            logger.LogInformation("Started web server.");
            foreach(var address in Api.Startup.HostedAddresses)
            {
                var parts = address.Split(':');
                ExposedPort = int.Parse(parts[2]);
                logger.LogInformation("Web server running on port " + ExposedPort);
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            logger.LogInformation("Tearing down web server.");
            host.StopAsync().Wait();
        }
    }
}

using Api.DtoModels;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Tests.Steps
{
    public class AspenApi
    {
        private readonly RestClient client;
        private readonly ILogger<AspenApi> logger;

        public AspenApi()
        {
            logger = TestLogger.Create<AspenApi>();
            var url = "http://127.0.0.1:" + Hooks.Hooks.ExposedPort;
            logger.LogInformation($"Creating rest client for {url}");
            client = new RestClient(url);
            client.ThrowOnAnyError = true;
        }

        public async Task<DtoPerson> CreatePersonAsync(DtoPerson person)
        {
            logger.LogInformation($"POST /api/person with: {person}");
            var request = new RestRequest("api/person/").AddJsonBody(person);
            var response = await client.PostAsync<DtoPerson>(request);
            logger.LogInformation($"Response: {response?.ToString() ?? "<NULL>"}");
            return response;
        }
    }

    public static class TestLogger
    {
        public static ILogger<T> Create<T>()
        {
            var logger = new NUnitLogger<T>();
            return logger;
        }

        public class NUnitLogger<T> : ILogger<T>, IDisposable
        {
            private readonly Action<string> output = Console.WriteLine;

            public void Dispose()
            {
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter) => output(formatter(state, exception));

            public bool IsEnabled(LogLevel logLevel) => true;

            public IDisposable BeginScope<TState>(TState state) => this;
        }

    }
}
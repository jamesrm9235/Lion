using Lion.Abstractions;
using Lion.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;

namespace Lion.Server.Endpoints
{
    [NonParallelizable]
    [TestFixture]
    public partial class BundlesControllerTests
    {
        private WebApplicationFactory<Startup> factory;
        private HttpClient client;
        private FakeBundleRepository repo;

        [OneTimeSetUp]
        public void Initialize()
        {
            repo = new FakeBundleRepository();

            factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IBundleRepository>(_ => repo);
                });
            });

            client = factory.CreateClient();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            factory?.Dispose();
            client?.Dispose();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            repo.Reset(seed: 50);
        }
    }
}

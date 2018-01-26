using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using PalTracker;

namespace PalTrackerTests
{
    public static class IntegrationTestServer
    {
        public static TestServer Start() =>
            new WebApplicationFactory<Program>().Server;
    }
}
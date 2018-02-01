using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Xunit;

namespace PalTrackerTests
{
    [Collection("Integration")]
    public class ManagementIntegrationTest
    {
        private readonly HttpClient _testClient;

        public ManagementIntegrationTest()
        {
            Environment.SetEnvironmentVariable("MYSQL__CLIENT__CONNECTIONSTRING", DbTestSupport.TestDbConnectionString);
            DbTestSupport.ExecuteSql("TRUNCATE TABLE time_entries");
            _testClient = IntegrationTestServer.Start().CreateClient();
        }

        [Fact]
        public async void HasHealth()
        {
            var response = await _testClient.GetAsync("/actuator/health").ConfigureAwait(false);
            var responseBody = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("UP", responseBody["status"]);
            Assert.Equal("UP", responseBody["diskSpace"]["status"]);
            Assert.Equal("UP", responseBody["timeEntry"]["status"]);
        }

        [Fact]
        public async void HasInfo()
        {
            var response = await _testClient.GetAsync("/actuator/info").ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

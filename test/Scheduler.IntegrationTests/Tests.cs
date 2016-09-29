using System;
using System.Net.Http;
using System.Threading.Tasks;
using ConsoleApplication;
using Love.Net.Scheduler.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Xunit.Abstractions;

namespace Tests {
    public class Tests {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output) {
            this._output = output;
            // Arrange
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();
            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }
        /// <summary>
        /// <see cref="JobsController.Get"/>
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAsync() {
            var responseMessage = await _client.GetAsync($"jobs/1/10");
            var message = await responseMessage.Content.ReadAsStringAsync();
            _output.WriteLine(message);
            Assert.True(false);
        }

        [Fact]
        public async Task Post() {
            var responseMessage = await _client.PostAsJsonAsync($"jobs", new
            CreateDto {
                CallbackUrl = "http://www.baidu.com",
                Cron = "1 2 2 2 2"
            });
            var message = await responseMessage.Content.ReadAsStringAsync();
            _output.WriteLine(message);
            Assert.True(false);
        }
    }
}

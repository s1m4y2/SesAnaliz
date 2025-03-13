using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SesAnalizTests
{
    public class AudioUploadTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task UploadAudio_Returns_Success()
        {
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(new byte[100]), "file", "test.wav");

            var response = await _client.PostAsync("/api/analysis/process-audio", content);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}

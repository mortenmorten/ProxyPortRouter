namespace ProxyPortRouter.Core.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using NSubstitute;

    using NUnit.Framework;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [TestFixture]
    public class TestRestBackend
    {
        [Test]
        public async Task GetListenAddressAsync_HttpCallSuccess_ReturnsListenAddress()
        {
            var handler = Substitute.ForPartsOf<FakeHttpMessageHandler>();
            handler.Send(
                    Arg.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get && r.RequestUri.LocalPath == "/api/entry/listen")).Returns(
                    new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("127.0.0.1") });
            var client = new RestBackend(handler);
            Assert.That(await client.GetListenAddressAsync().ConfigureAwait(false), Is.EqualTo("127.0.0.1"));
        }

        [Test]
        public async Task GetCurrentEntryAsync_HttpCallSuccess_ReturnsCurrentEntry()
        {
            var handler = Substitute.ForPartsOf<FakeHttpMessageHandler>();
            handler
                .Send(
                    Arg.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get && r.RequestUri.LocalPath == "/api/entry")).Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(
                                JsonSerializer<CommandEntry>.Serialize(
                                    new CommandEntry() { Name = "test" }),
                                Encoding.UTF8,
                                "application/json")
                        });
            var client = new RestBackend(handler);
            var result = await client.GetCurrentAsync().ConfigureAwait(false);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("test"));
        }
    }
}

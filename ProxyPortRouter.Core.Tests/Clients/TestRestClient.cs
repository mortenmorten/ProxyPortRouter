namespace ProxyPortRouter.Core.Tests.Clients
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using NSubstitute;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [TestFixture]
    public class TestRestClient
    {
        [Test]
        public async Task SetCurrentEntryAsync_ValidEntry_SendsMessageWithJsonBody()
        {
            var hasSentMessage = false;
            var handler = Substitute.ForPartsOf<FakeHttpMessageHandler>();
            handler
                .Send(
                    Arg.Is<HttpRequestMessage>(
                        r => AssertHttpCall(r, HttpMethod.Put, "/api/entry", Is.Not.Null))).Returns(
                    new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(
                                JsonSerializer<CommandEntry>.Serialize(
                                    new CommandEntry()
                                        {
                                            Name = "theEntryName"
                                        }),
                                Encoding.UTF8,
                                "application/json")
                        }).AndDoes(ci => hasSentMessage = true);

            var client = new RestClient(new HttpClient(handler) { BaseAddress = new Uri("http://localhost") });
            await client.SetCurrentEntryAsync("theEntryName").ConfigureAwait(false);

            Assert.That(hasSentMessage, Is.True);
        }

        private static bool AssertHttpCall(HttpRequestMessage request, HttpMethod expectedMethod, string expectedLocalPath, IResolveConstraint expectedContent)
        {
            Assert.That(request.Method, Is.EqualTo(expectedMethod));
            Assert.That(request.RequestUri.LocalPath, Is.EqualTo(expectedLocalPath));
            Assert.That(request.Content, expectedContent);

            return true;
        }
    }
}

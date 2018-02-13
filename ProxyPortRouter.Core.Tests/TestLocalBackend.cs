namespace ProxyPortRouter.Core.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using NSubstitute;

    using NUnit.Framework;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [TestFixture]
    public class TestLocalBackend
    {
        private readonly ISettings settings = Substitute.For<ISettings>();
        private readonly IPortProxyControllerAsync proxyController = Substitute.For<IPortProxyControllerAsync>();
        private readonly ISlaveClientAsync slaveClient = Substitute.For<ISlaveClientAsync>();

        [Test]
        public async Task GetListenAddressAsync_ValidConfigInConstructor_ReturnsAddressFromConfig()
        {
            settings.ListenAddress.Returns("settingsAddress");
            IBackendAsync backend = new LocalBackend(settings, null, null);
            Assert.That(await backend.GetListenAddressAsync().ConfigureAwait(false), Is.EqualTo("settingsAddress"));
        }

        [Test]
        public async Task GetEntriesAsync_ValidProxyController_ReturnsEntriesFromController()
        {
            var entries = new List<CommandEntry> { new CommandEntry() { Name = "1" }, new CommandEntry() { Name = "2" } };
            proxyController.GetEntriesAsync().Returns(entries);
            IBackendAsync backend = new LocalBackend(null, proxyController, null);
            Assert.That(await backend.GetEntriesAsync().ConfigureAwait(false), Is.SameAs(entries));
        }

        [Test]
        public async Task GetCurrentAsync_ValidProxyController_ReturnsResultFromController()
        {
            var current = new CommandEntry();
            proxyController.GetCurrentEntryAsync().Returns(current);
            IBackendAsync backend = new LocalBackend(null, proxyController, null);
            Assert.That(await backend.GetCurrentAsync().ConfigureAwait(false), Is.SameAs(current));
        }

        [Test]
        public async Task SetCurrentAsync_ValidProxyController_CallsProxyController()
        {
            IBackendAsync backend = new LocalBackend(null, proxyController, null);
            await backend.SetCurrentAsync("name").ConfigureAwait(false);
            await proxyController.Received().SetCurrentEntryAsync(Arg.Is("name")).ConfigureAwait(false);
        }

        [Test]
        public async Task SetCurrentAsync_ValidProxyControllerValidSlaveClient_CallsSlaveClient()
        {
            IBackendAsync backend = new LocalBackend(null, proxyController, slaveClient);
            await backend.SetCurrentAsync("name").ConfigureAwait(false);
            await slaveClient.Received().SetCurrentEntryAsync(Arg.Is("name")).ConfigureAwait(false);
        }

        [Test]
        public async Task SetCurrentAsync_CurrentChanges_RaisesCurrentChangedEvent()
        {
            var wasCalled = false;
            IBackendAsync backend = new LocalBackend(null, proxyController, slaveClient);
            proxyController.GetCurrentEntryAsync().Returns(
                new CommandEntry() { Name = "oldName" },
                new CommandEntry() { Name = "newName" });
            backend.CurrentChanged += (o, e) => wasCalled = true;
            await backend.SetCurrentAsync("newName").ConfigureAwait(false);
            Assert.That(wasCalled, Is.True);
        }
    }
}

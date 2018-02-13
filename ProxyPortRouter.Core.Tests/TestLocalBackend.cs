namespace ProxyPortRouter.Core.Tests
{
    using System.Collections.Generic;

    using NSubstitute;

    using NUnit.Framework;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [TestFixture]
    public class TestLocalBackend
    {
        private readonly ISettings settings = Substitute.For<ISettings>();
        private readonly IPortProxyController proxyController = Substitute.For<IPortProxyController>();
        private readonly ISlaveClient slaveClient = Substitute.For<ISlaveClient>();

        [Test]
        public void GetListenAddress_ValidConfigInConstructor_ReturnsAddressFromConfig()
        {
            settings.ListenAddress.Returns("settingsAddress");
            IBackend backend = new LocalBackend(settings, null, null);
            Assert.That(backend.GetListenAddress(), Is.EqualTo("settingsAddress"));
        }

        [Test]
        public void GetEntries_ValidProxyController_ReturnsEntriesFromController()
        {
            var entries = new List<CommandEntry> { new CommandEntry() { Name = "1" }, new CommandEntry() { Name = "2" } };
            proxyController.GetEntries().Returns(entries);
            IBackend backend = new LocalBackend(null, proxyController, null);
            Assert.That(backend.GetEntries(), Is.SameAs(entries));
        }

        [Test]
        public void GetCurrent_ValidProxyController_ReturnsResultFromController()
        {
            var current = new CommandEntry();
            proxyController.GetCurrentEntry().Returns(current);
            IBackend backend = new LocalBackend(null, proxyController, null);
            Assert.That(backend.GetCurrent(), Is.SameAs(current));
        }

        [Test]
        public void SetCurrent_ValidProxyController_CallsProxyController()
        {
            IBackend backend = new LocalBackend(null, proxyController, null);
            backend.SetCurrent("name");
            proxyController.Received().SetCurrentEntry(Arg.Is("name"));
        }

        [Test]
        public void SetCurrent_ValidProxyControllerValidSlaveClient_CallsSlaveClient()
        {
            IBackend backend = new LocalBackend(null, proxyController, slaveClient);
            backend.SetCurrent("name");
            slaveClient.Received().SetCurrentEntryAsync(Arg.Is("name"));
        }

        [Test]
        public void SetCurrent_CurrentChanges_RaisesCurrentChangedEvent()
        {
            var wasCalled = false;
            IBackend backend = new LocalBackend(null, proxyController, slaveClient);
            proxyController.GetCurrentEntry().Returns(
                new CommandEntry() { Name = "oldName" },
                new CommandEntry() { Name = "newName" });
            backend.CurrentChanged += (o, e) => wasCalled = true;
            backend.SetCurrent("newName");
            Assert.That(wasCalled, Is.True);
        }
    }
}

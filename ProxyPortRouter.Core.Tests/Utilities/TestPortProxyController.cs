namespace ProxyPortRouter.Core.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using NSubstitute;

    using NUnit.Framework;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [TestFixture]
    public class TestPortProxyController
    {
        private ISettings settings;
        private IProcessRunnerAsync processRunner;
        private IPortProxyControllerAsync controller;

        [SetUp]
        public void Setup()
        {
            settings = Substitute.For<ISettings>();
            processRunner = Substitute.For<IProcessRunnerAsync>();
            controller = new PortProxyController(settings, processRunner);
        }

        [Test]
        public void Constructor_ValidProcessRunner_DoesNotUpdateCurrentEntry()
        {
            processRunner.DidNotReceive().RunAsync(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetShowCommandArguments()));
        }

        [Test]
        public async Task GetEntriesAsync_ValidSettings_ReturnsEntriesFromConfig()
        {
            var entries = new List<CommandEntry> { new CommandEntry() { Name = "1" }, new CommandEntry() { Name = "2" } };
            settings.Entries.Returns(entries);
            Assert.That(await controller.GetEntriesAsync().ConfigureAwait(false), Is.EquivalentTo(entries));
        }

        [Test]
        public async Task GetCurrentEntryAsync_NetshReturnsEmptyResult_ReturnsNotSetResult()
        {
            var currentEntry = await controller.GetCurrentEntryAsync().ConfigureAwait(false);
            Assert.That(currentEntry, Is.Not.Null);
            Assert.That(currentEntry.Name, Is.EqualTo("<not set>"));
        }

        [Test]
        public async Task GetCurrentEntryAsync_NetshReturnsUnknownResult_ReturnsUnknownResult()
        {
            processRunner.RunAsync(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetShowCommandArguments())).Returns("127.0.0.1 80 192.168.42.42 80");
            var tempController = new PortProxyController(settings, processRunner);
            await tempController.RefreshCurrentConnectAddressAsync().ConfigureAwait(false);
            var currentEntry = await tempController.GetCurrentEntryAsync().ConfigureAwait(false);
            Assert.That(currentEntry, Is.Not.Null);
            Assert.That(currentEntry.Name, Is.EqualTo("<unknown>"));
        }

        [Test]
        public async Task GetCurrentEntryAsync_NetshReturnsKnownResult_ReturnsEntry()
        {
            settings.Entries.Returns(
                new List<CommandEntry> { new CommandEntry { Name = "MyEntry", Address = "192.168.42.42" } });
            processRunner.RunAsync(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetShowCommandArguments())).Returns("127.0.0.1 80 192.168.42.42 80");
            IPortProxyControllerAsync tempController = new PortProxyController(settings, processRunner);
            await tempController.RefreshCurrentConnectAddressAsync().ConfigureAwait(false);
            var currentEntry = await tempController.GetCurrentEntryAsync().ConfigureAwait(false);
            Assert.That(currentEntry, Is.SameAs(settings.Entries[0]));
        }

        [Test]
        public async Task GetCurrentEntryAsync_NetshReturnsKnownResultWithPort8080_ReturnsEntry()
        {
            settings.Entries.Returns(
                new List<CommandEntry> { new CommandEntry { Name = "MyEntry", Address = "192.168.42.42:8080" } });
            processRunner.RunAsync(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetShowCommandArguments())).Returns("127.0.0.1 80 192.168.42.42 8080");
            IPortProxyControllerAsync tempController = new PortProxyController(settings, processRunner);
            await tempController.RefreshCurrentConnectAddressAsync().ConfigureAwait(false);
            var currentEntry = await tempController.GetCurrentEntryAsync().ConfigureAwait(false);
            Assert.That(currentEntry, Is.SameAs(settings.Entries[0]));
        }

        [Test]
        public void SetCurrentEntryAsync_EntryIsNotInConfig_ThrowsException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => controller.SetCurrentEntryAsync("test"));
        }

        [Test]
        public async Task SetCurrentEntryAsync_EntryIsInConfig_RunsProcessSettingNewAddress()
        {
            const string ListenAddress = "127.0.0.1";
            const string MyEntryAddress = "192.168.42.42";

            settings.ListenAddress.Returns(ListenAddress);
            settings.Entries.Returns(
                new List<CommandEntry> { new CommandEntry { Name = "MyEntry", Address = MyEntryAddress } });
            await controller.SetCurrentEntryAsync("MyEntry").ConfigureAwait(false);
            await processRunner.Received().RunAsync(
                    Arg.Is(NetshCommandFactory.Executable),
                    Arg.Is(NetshCommandFactory.GetAddCommandArguments(ListenAddress, MyEntryAddress)))
                .ConfigureAwait(false);
        }
    }
}

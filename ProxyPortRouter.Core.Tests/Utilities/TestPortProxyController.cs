namespace ProxyPortRouter.Core.Tests.Utilities
{
    using System;
    using System.Collections.Generic;

    using NSubstitute;

    using NUnit.Framework;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [TestFixture]
    public class TestPortProxyController
    {
        private ISettings settings;
        private IProcessRunner processRunner;
        private PortProxyController controller;

        [SetUp]
        public void Setup()
        {
            settings = Substitute.For<ISettings>();
            processRunner = Substitute.For<IProcessRunner>();
            controller = new PortProxyController(settings, processRunner);
        }

        [Test]
        public void Constructor_ValidProcessRunner_UpdatesCurrentEntry()
        {
            processRunner.Received().Run(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetShowCommandArguments()));
        }

        [Test]
        public void GetEntries_ValidSettings_ReturnsEntriesFromConfig()
        {
            var entries = new List<CommandEntry> { new CommandEntry() { Name = "1" }, new CommandEntry() { Name = "2" } };
            settings.Entries.Returns(entries);
            Assert.That(controller.GetEntries(), Is.EquivalentTo(entries));
        }

        [Test]
        public void GetCurrentEntry_NetshReturnsEmptyResult_ReturnsNotSetResult()
        {
            var currentEntry = controller.GetCurrentEntry();
            Assert.That(currentEntry, Is.Not.Null);
            Assert.That(currentEntry.Name, Is.EqualTo("<not set>"));
        }

        [Test]
        public void GetCurrentEntry_NetshReturnsUnknownResult_ReturnsUnknownResult()
        {
            processRunner.Run(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetShowCommandArguments())).Returns("127.0.0.1 80 192.168.42.42 80");
            var tempController = new PortProxyController(settings, processRunner);
            var currentEntry = tempController.GetCurrentEntry();
            Assert.That(currentEntry, Is.Not.Null);
            Assert.That(currentEntry.Name, Is.EqualTo("<unknown>"));
        }

        [Test]
        public void GetCurrentEntry_NetshReturnsKnownResult_ReturnsEntry()
        {
            settings.Entries.Returns(
                new List<CommandEntry> { new CommandEntry { Name = "MyEntry", Address = "192.168.42.42" } });
            processRunner.Run(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetShowCommandArguments())).Returns("127.0.0.1 80 192.168.42.42 80");
            var tempController = new PortProxyController(settings, processRunner);
            var currentEntry = tempController.GetCurrentEntry();
            Assert.That(currentEntry, Is.SameAs(settings.Entries[0]));
        }

        [Test]
        public void SetCurrentEntry_EntryIsNotInConfig_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => controller.SetCurrentEntry("test"));
        }

        [Test]
        public void SetCurrentEntry_EntryIsInConfig_RunsProcessSettingNewAddress()
        {
            const string ListenAddress = "127.0.0.1";
            const string MyEntryAddress = "192.168.42.42";

            settings.ListenAddress.Returns(ListenAddress);
            settings.Entries.Returns(
                new List<CommandEntry> { new CommandEntry { Name = "MyEntry", Address = MyEntryAddress } });
            controller.SetCurrentEntry("MyEntry");
            processRunner.Received().Run(
                Arg.Is(NetshCommandFactory.Executable),
                Arg.Is(NetshCommandFactory.GetAddCommandArguments(ListenAddress, MyEntryAddress)));
        }
    }
}

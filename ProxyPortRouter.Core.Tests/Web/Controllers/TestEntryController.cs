namespace ProxyPortRouter.Core.Tests.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using NSubstitute;

    using NUnit.Framework;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Web.Controllers;

    [TestFixture]
    public class TestEntryController
    {
        private readonly IBackendAsync backend = Substitute.For<IBackendAsync>();

        [Test]
        public async Task GetEntriesAsync_ValidBackend_ReturnsEntriesFromBackend()
        {
            var entries = new List<CommandEntry> { new CommandEntry() { Name = "1" }, new CommandEntry() { Name = "2" } };
            backend.GetEntriesAsync().Returns(entries);
            var entryController = new EntryController(backend);
            var contentResult = await entryController.GetEntriesAsync().ConfigureAwait(false);
            var valueResult = (contentResult.Result as OkObjectResult).Value as IEnumerable<CommandEntry>;
            Assert.That(valueResult, Is.Not.Null);
            Assert.That(valueResult, Is.EquivalentTo(entries));
        }

        [Test]
        public async Task GetCurrentAsync_ValidBackend_ReturnsCurrentFromBackend()
        {
            backend.GetCurrentAsync().Returns(new CommandEntry() { Name = "backendCurrent" });
            var entryController = new EntryController(backend);
            var contentResult = await entryController.GetCurrentAsync().ConfigureAwait(false);
            var valueResult = (contentResult.Result as OkObjectResult).Value as CommandEntry;
            Assert.That(valueResult, Is.Not.Null);
            Assert.That(valueResult.Name, Is.EqualTo("backendCurrent"));
        }

        [Test]
        public async Task GetCurrentAsync_ValidBackendCurrentIsNull_ReturnsOkAndEmptyContent()
        {
            backend.GetCurrentAsync().Returns((CommandEntry)null);
            var entryController = new EntryController(backend);
            var contentResult = await entryController.GetCurrentAsync().ConfigureAwait(false);
            Assert.That(contentResult, Is.Not.Null);
        }

        [Test]
        public async Task PutCurrentAsync_ValidBackend_CallsSetCurrentOnBackend()
        {
            backend.GetCurrentAsync().Returns(new CommandEntry() { Name = "backendCurrent" });
            var entryController = new EntryController(backend);
            var contentResult =
                await entryController.PutCurrentAsync(new NameEntry("name")).ConfigureAwait(false);
            var valueResult = (contentResult.Result as OkObjectResult).Value as CommandEntry;
            Assert.That(valueResult, Is.Not.Null);
            Assert.That(valueResult.Name, Is.EqualTo("backendCurrent"));
            await backend.Received().SetCurrentAsync(Arg.Is("name")).ConfigureAwait(false);
        }

        [Test]
        public async Task PutCurrentAsync_ValidBackendSetCurrentNotFound_ReturnsNotFoundResult()
        {
            backend.When(b => b.SetCurrentAsync("name")).Throw<InvalidOperationException>();
            var entryController = new EntryController(backend);
            var notFoundResult =
                await entryController.PutCurrentAsync(new NameEntry("name")).ConfigureAwait(false);
            Assert.That(notFoundResult.Result as NotFoundResult, Is.Not.Null);
        }
    }
}

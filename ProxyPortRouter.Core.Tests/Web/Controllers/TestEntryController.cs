namespace ProxyPortRouter.Core.Tests.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http.Results;

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
            var contentResult =
                await entryController.GetEntriesAsync().ConfigureAwait(false) as
                    OkNegotiatedContentResult<IEnumerable<CommandEntry>>;
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.EquivalentTo(entries));
        }

        [Test]
        public async Task GetCurrentAsync_ValidBackend_ReturnsCurrentFromBackend()
        {
            backend.GetCurrentAsync().Returns(new CommandEntry() { Name = "backendCurrent" });
            var entryController = new EntryController(backend);
            var contentResult = await entryController.GetCurrentAsync().ConfigureAwait(false) as OkNegotiatedContentResult<CommandEntry>;
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo("backendCurrent"));
        }

        [Test]
        public async Task GetCurrentAsync_ValidBackendCurrentIsNull_ReturnsOkAndEmptyContent()
        {
            backend.GetCurrentAsync().Returns((CommandEntry)null);
            var entryController = new EntryController(backend);
            var contentResult = await entryController.GetCurrentAsync().ConfigureAwait(false) as OkResult;
            Assert.That(contentResult, Is.Not.Null);
        }

        [Test]
        public async Task PutCurrentAsync_ValidBackend_CallsSetCurrentOnBackend()
        {
            backend.GetCurrentAsync().Returns(new CommandEntry() { Name = "backendCurrent" });
            var entryController = new EntryController(backend);
            var contentResult =
                await entryController.PutCurrentAsync(new NameEntry("name")).ConfigureAwait(false) as
                    OkNegotiatedContentResult<CommandEntry>;
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo("backendCurrent"));
            await backend.Received().SetCurrentAsync(Arg.Is("name")).ConfigureAwait(false);
        }

        [Test]
        public async Task PutCurrentAsync_ValidBackendSetCurrentNotFound_ReturnsNotFoundResult()
        {
            backend.When(b => b.SetCurrentAsync("name")).Throw<InvalidOperationException>();
            var entryController = new EntryController(backend);
            var notFoundResult =
                await entryController.PutCurrentAsync(new NameEntry("name")).ConfigureAwait(false) as NotFoundResult;
            Assert.That(notFoundResult, Is.Not.Null);
        }
    }
}

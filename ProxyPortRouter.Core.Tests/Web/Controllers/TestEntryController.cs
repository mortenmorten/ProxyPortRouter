namespace ProxyPortRouter.Core.Tests.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Results;

    using NSubstitute;

    using NUnit.Framework;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Web.Controllers;

    [TestFixture]
    public class TestEntryController
    {
        private readonly IBackend backend = Substitute.For<IBackend>();

        [Test]
        public void GetEntries_ValidBackend_ReturnsEntriesFromBackend()
        {
            var entries = new List<CommandEntry> { new CommandEntry() { Name = "1" }, new CommandEntry() { Name = "2" } };
            backend.GetEntries().Returns(entries);
            var entryController = new EntryController(backend);
            var contentResult = entryController.GetEntries() as OkNegotiatedContentResult<IEnumerable<CommandEntry>>;
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.EquivalentTo(entries));
        }

        [Test]
        public void GetCurrent_ValidBackend_ReturnsCurrentFromBackend()
        {
            backend.GetCurrent().Returns(new CommandEntry() { Name = "backendCurrent" });
            var entryController = new EntryController(backend);
            var contentResult = entryController.GetCurrent() as OkNegotiatedContentResult<CommandEntry>;
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo("backendCurrent"));
        }

        [Test]
        public void GetCurrent_ValidBackendCurrentIsNull_ReturnsOkAndEmptyContent()
        {
            backend.GetCurrent().Returns((CommandEntry)null);
            var entryController = new EntryController(backend);
            var contentResult = entryController.GetCurrent() as OkResult;
            Assert.That(contentResult, Is.Not.Null);
        }

        [Test]
        public void PutCurrent_ValidBackend_CallsSetCurrentOnBackend()
        {
            backend.GetCurrent().Returns(new CommandEntry() { Name = "backendCurrent" });
            var entryController = new EntryController(backend);
            var contentResult = entryController.PutCurrent(new NameEntry("name")) as OkNegotiatedContentResult<CommandEntry>;
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo("backendCurrent"));
            backend.Received().SetCurrent(Arg.Is("name"));
        }

        [Test]
        public void PutCurrent_ValidBackendSetCurrentNotFound_ReturnsNotFoundResult()
        {
            backend.When(b => b.SetCurrent("name")).Throw<InvalidOperationException>();
            var entryController = new EntryController(backend);
            var notFoundResult = entryController.PutCurrent(new NameEntry("name")) as NotFoundResult;
            Assert.That(notFoundResult, Is.Not.Null);
        }
    }
}

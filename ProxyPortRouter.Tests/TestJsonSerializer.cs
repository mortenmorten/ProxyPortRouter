using NUnit.Framework;
using ProxyPortRouter.Config;
using ProxyPortRouter.Utilities;

namespace ProxyPortRouter.Tests
{
    [TestFixture]
    public class TestJsonSerializer
    {
        [Test]
        public void DeserializeFromString_CommandEntry_NonNullObject()
        {
            var test = JsonSerializer<CommandEntry>.DeserializeFromString("{ \"name\": \"test\", \"command\": \"cmd\" }");
            Assert.That(test, Is.Not.Null);
            Assert.That(test.Name, Is.EqualTo("test"));
        }

        [Test]
        public void DeserializeFromString_CommandEntries_NonNullObject()
        {
            var test = JsonSerializer<Settings>.DeserializeFromString("{ \"entries\": [ { \"name\": \"test1\", \"command\": \"cmd1\" }, { \"name\": \"test2\", \"command\": \"cmd2\" } ] }");
            Assert.That(test, Is.Not.Null);
            Assert.That(test.Entries, Is.Not.Empty);
            Assert.That(test.Entries[0].Name, Is.EqualTo("test1"));
        }

        [Test]
        public void DeserializeFromString_CommandEntriesFile_NonNullObject()
        {
            var test = JsonSerializer<Settings>.Deserialize(
                System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "entries.json"));
            Assert.That(test, Is.Not.Null);
            Assert.That(test.Entries, Is.Not.Empty);
            Assert.That(test.Entries[0].Name, Is.EqualTo("None"));
        }
    }
}

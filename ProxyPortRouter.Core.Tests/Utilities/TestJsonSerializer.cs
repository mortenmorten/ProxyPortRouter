﻿namespace ProxyPortRouter.Core.Tests.Utilities
{
    using NUnit.Framework;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

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
                System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "entries.json"));
            Assert.That(test, Is.Not.Null);
            Assert.That(test.Entries, Is.Not.Empty);
            Assert.That(test.Entries[0].Name, Is.EqualTo("None"));
        }

        [Test]
        public void Serialize_NameEntry_ReturnsJsonString()
        {
            Assert.That(JsonSerializer<NameEntry>.Serialize(new NameEntry("test")), Is.EqualTo("{\"name\":\"test\"}"));
        }
    }
}

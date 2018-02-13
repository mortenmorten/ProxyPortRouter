namespace ProxyPortRouter.Core.Tests.Utilities
{
    using NUnit.Framework;

    using ProxyPortRouter.Core.Utilities;

    [TestFixture]
    public class TestNetshCommandFactory
    {
        [Test]
        public void GetShowCommandArguments_ReturnsCommandArguments()
        {
            Assert.That(
                NetshCommandFactory.GetShowCommandArguments(),
                Is.EqualTo("interface portproxy show v4tov4"));
        }

        [Test]
        public void GetAddCommandArguments_ValidParameter_ReturnsCommandArguments()
        {
            Assert.That(
                NetshCommandFactory.GetAddCommandArguments("127.0.0.1", "192.168.42.42"),
                Is.EqualTo("interface portproxy add v4tov4 listenport=80 listenaddress=127.0.0.1 connectport=80 connectaddress=192.168.42.42 protocol=tcp"));
        }

        [Test]
        public void GetDeleteCommandArguments_ValidParameter_ReturnsCommandArguments()
        {
            Assert.That(
                NetshCommandFactory.GetDeleteCommandArguments("127.0.0.1"),
                Is.EqualTo("interface portproxy delete v4tov4 listenport=80 listenaddress=127.0.0.1 protocol=tcp"));
        }
    }
}

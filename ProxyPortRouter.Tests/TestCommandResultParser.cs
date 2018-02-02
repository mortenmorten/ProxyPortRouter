using NUnit.Framework;

namespace ProxyPortRouter.Tests
{
    [TestFixture]
    public class TestCommandResultParser
    {
        private readonly CommandResultParser commandResultParser = new CommandResultParser {ListenAddress = "127.0.0.11"};

        [Test]
        public void GetCurrentProxyAddress_ValidResponse_ReturnsProxyAddress()
        {
            const string commandResult = "\r\nListen on ipv4:             Connect to ipv4:\r\n\r\nAddress         Port        Address         Port\r\n--------------- ----------  --------------- ----------\r\n127.0.0.11      80          192.168.1.42       80\r\n";
            Assert.That(commandResultParser.GetCurrentProxyAddress(commandResult), Is.EqualTo("192.168.1.42"));
        }

        [Test]
        public void GetCurrentProxyAddress_EmptyResponse_ReturnsBlankAddress()
        {
            const string commandResult = "\n";
            Assert.That(commandResultParser.GetCurrentProxyAddress(commandResult), Is.Empty);
        }

    }
}

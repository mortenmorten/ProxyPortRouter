using System.Text.RegularExpressions;

namespace ProxyPortRouter
{
    public class CommandResultParser
    {
        public string ListenAddress { get; set; }

        public string Port { get; set; } = "80";

        public string GetCurrentProxyAddress(string commandResult)
        {
            var match = Regex.Match(commandResult, $"{ListenAddress}\\s+{Port}\\s+(\\S*)\\s+{Port}");
            if (!match.Success || match.Groups.Count < 2) return string.Empty;
            return match.Groups[1].Value;
        }
    }
}

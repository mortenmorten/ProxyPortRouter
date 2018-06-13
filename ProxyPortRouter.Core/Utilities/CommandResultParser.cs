namespace ProxyPortRouter.Core.Utilities
{
    using System.Text.RegularExpressions;

    public class CommandResultParser
    {
        public string ListenAddress { get; set; }

        public string Port { get; set; } = "80";

        public string GetCurrentProxyAddress(string commandResult)
        {
            var match = Regex.Match(commandResult, $"{this.ListenAddress}\\s+{this.Port}\\s+(\\S*)\\s+(\\d+)");
            if (!match.Success || match.Groups.Count < 2)
            {
                return string.Empty;
            }

            return $"{match.Groups[1].Value}:{(match.Groups.Count > 2 ? match.Groups[2].Value : "80")}";
        }
    }
}

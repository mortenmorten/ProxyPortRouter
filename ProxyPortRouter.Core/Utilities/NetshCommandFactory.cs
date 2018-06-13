namespace ProxyPortRouter.Core.Utilities
{
    public static class NetshCommandFactory
    {
        public const string Executable = "netsh.exe";
        private const string CommandParameter = "interface portproxy";
        private const string AddEntryParameter =
            "add v4tov4 listenport=80 listenaddress={0} connectport={2} connectaddress={1} protocol=tcp";

        private const string DeleteEntryParameter =
            "delete v4tov4 listenport=80 listenaddress={0} protocol=tcp";

        private const string ShowEntriesParameter = "show v4tov4";

        public static string GetAddCommandArguments(string listenAddress, string connectAddress, int connectPort = 80)
        {
            return string.Join(" ", CommandParameter, string.Format(AddEntryParameter, listenAddress, connectAddress, connectPort));
        }

        public static string GetDeleteCommandArguments(string listenAddress)
        {
            return string.Join(" ", CommandParameter, string.Format(DeleteEntryParameter, listenAddress));
        }

        public static string GetShowCommandArguments()
        {
            return string.Join(" ", CommandParameter, ShowEntriesParameter);
        }
    }
}

﻿namespace ProxyPortRouter
{
    public static class NetshCommandFactory
    {
        public const string Executable = "netsh.exe";
        private const string CommandParameter = "interface portproxy";
        private const string AddEntryParameter =
            "add v4tov4 listenport=80 listenaddress={0} connectport=80 connectaddress={1} protocol=tcp";

        private const string DeleteEntryParameter =
            "delete v4tov4 listenport=80 listenaddress={0} protocol=tcp";

        private const string ShowEntriesParameter = "show v4tov4";

        public static string GetAddCommandArguments(string listenAddress, string connectAddress)
        {
            return string.Join(" ", CommandParameter, string.Format(AddEntryParameter, listenAddress, connectAddress));
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

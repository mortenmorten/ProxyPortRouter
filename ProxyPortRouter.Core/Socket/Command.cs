namespace ProxyPortRouter.Core.Socket
{
    using System.Text.RegularExpressions;

    public class Command
    {
        private readonly string input;

        public Command(string input)
        {
            this.input = input;
            var match = Regex.Match(input, @"(\w+)\s\/(\w+)\??(.*)");
            Method = GetValue(match, 1, string.Empty);
            Path = GetValue(match, 2, string.Empty);
            QueryParameters = GetValue(match, 3, string.Empty);
        }

        public string Method { get; }

        public string Path { get; }

        public string QueryParameters { get; }

        public void ThrowIfDefaultValues()
        {
            if (string.IsNullOrEmpty(Method))
            {
                throw new CommandException("Missing Method");
            }

            if (string.IsNullOrEmpty(Path))
            {
                throw new CommandException("Missing Path");
            }
        }

        public override string ToString()
        {
            return input;
        }

        private static string GetValue(Match match, int group, string defaultValue)
        {
            return match == null || !match.Success || match.Groups.Count <= group
                ? defaultValue
                : match.Groups[group].Value;
        }
    }
}
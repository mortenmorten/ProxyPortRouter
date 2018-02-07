namespace ProxyPortRouter.Config
{
    using System.Collections.Generic;
    using CommandLine;

    public class Options : IOptions
    {
        [Option('s', "slave", HelpText = "Address to slave")]
        public string SlaveAddress { get; set; }

        public static Options Create(IEnumerable<string> args)
        {
            Options rtrn = null;
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                                      .WithParsed(parsed => rtrn = parsed);
            return rtrn ?? new Options();
        }
    }
}

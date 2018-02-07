namespace ProxyPortRouterService
{
    using System.Reflection;
    using System.ServiceProcess;

    using log4net;

    public partial class Service : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Proxy Port Router is starting");
        }

        protected override void OnStop()
        {
            Log.Info("Proxy Port Router is stopping");
        }
    }
}

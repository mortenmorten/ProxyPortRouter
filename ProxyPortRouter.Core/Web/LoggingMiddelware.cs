namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Microsoft.Owin;

    using Serilog;
    using Serilog.Context;

    [UsedImplicitly]
    public class LoggingMiddelware : OwinMiddleware
    {
        public LoggingMiddelware(OwinMiddleware next)
            : base(next)
        {
        }

        // ReSharper disable once AsyncConverter.AsyncMethodNamingHighlighting
        public override async Task Invoke(IOwinContext context)
        {
            Log.Information(
                "Method: {Method} Path: {Path}",
                context.Request.Method,
                context.Request.Path);

            using (LogContext.PushProperty("RequestId", Guid.NewGuid()))
            {
                await Next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}

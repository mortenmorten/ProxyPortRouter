namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.IO;
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

            using (var captureResponseBody = new CaptureResponseBody(context))
            {
                using (LogContext.PushProperty("RequestId", Guid.NewGuid()))
                {
                    await Next.Invoke(context).ConfigureAwait(false);
                }

                Log.Verbose("Response body: {Body}", await captureResponseBody.GetBodyAsync().ConfigureAwait(false));
            }
        }

        private class CaptureResponseBody : IDisposable
        {
            /*
            // Response body is a write-only network stream by default for Katana hosts.
            // You will need to replace context.Response.Body with a MemoryStream,
            // read the stream, log the content and then copy the memory stream content
            // back into the original network stream
            */

            private readonly Stream stream;
            private readonly MemoryStream buffer;

            public CaptureResponseBody(IOwinContext context)
            {
                stream = context.Response.Body;
                buffer = new MemoryStream();
                context.Response.Body = buffer;
            }

            public Task<string> GetBodyAsync()
            {
                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);
                return reader.ReadToEndAsync();
            }

            public async void Dispose()
            {
                await GetBodyAsync().ConfigureAwait(false);

                // You need to do this so that the response we buffered
                // is flushed out to the client application.
                buffer.Seek(0, SeekOrigin.Begin);
                await buffer.CopyToAsync(stream).ConfigureAwait(false);
            }
        }
    }
}

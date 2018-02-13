namespace ProxyPortRouter.Core.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using ProxyPortRouter.Core.Config;

    [RoutePrefix("api/entry")]
    public class EntryController : ApiController
    {
        private readonly IBackendAsync backend;

        public EntryController(IBackendAsync backend)
        {
            this.backend = backend;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentAsync()
        {
            var entry = await backend.GetCurrentAsync().ConfigureAwait(false);
            if (entry == null)
            {
                return Ok();
            }

            return Ok(entry);
        }

        [Route("")]
        [HttpPut]
        public async Task<IHttpActionResult> PutCurrentAsync([FromBody] NameEntry entry)
        {
            try
            {
                await backend.SetCurrentAsync(entry?.Name).ConfigureAwait(false);
                return await GetCurrentAsync().ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Route("list")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEntriesAsync()
        {
            return Ok(await backend.GetEntriesAsync().ConfigureAwait(false));
        }

        [Route("listen")]
        [HttpGet]
        public async Task<IHttpActionResult> GetListenAddressAsync()
        {
            return Ok(await backend.GetListenAddressAsync().ConfigureAwait(true));
        }
    }
}

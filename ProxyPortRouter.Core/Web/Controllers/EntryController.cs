namespace ProxyPortRouter.Core.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using ProxyPortRouter.Core.Config;

    using Serilog;

    [ApiController]
    [Route("api/entry")]
    public class EntryController : ControllerBase
    {
        private readonly IBackendAsync backend;

        public EntryController(IBackendAsync backend)
        {
            this.backend = backend;
        }

        [HttpGet]
        public async Task<ActionResult<CommandEntry>> GetCurrentAsync()
        {
            var entry = await backend.GetCurrentAsync().ConfigureAwait(false);
            if (entry == null)
            {
                return Ok();
            }

            return Ok(entry);
        }

        [HttpPut]
        public async Task<ActionResult<CommandEntry>> PutCurrentAsync([FromBody] NameEntry entry)
        {
            Log.Debug("Entry: {@Entry}", entry);

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

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<CommandEntry>>> GetEntriesAsync()
        {
            return Ok(await backend.GetEntriesAsync().ConfigureAwait(false));
        }

        [HttpGet("listen")]
        public async Task<ActionResult> GetListenAddressAsync()
        {
            return Ok(await backend.GetListenAddressAsync().ConfigureAwait(true));
        }
    }
}

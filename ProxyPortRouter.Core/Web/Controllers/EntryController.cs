namespace ProxyPortRouter.Core.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using ProxyPortRouter.Core.Config;

    [RoutePrefix("api/entry")]
    public class EntryController : ApiController
    {
        private readonly IBackend backend;

        public EntryController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetCurrent()
        {
            var entry = backend.GetCurrent();
            if (entry == null)
            {
                return Ok();
            }

            return Ok(entry);
        }

        [Route("")]
        [HttpPut]
        public IHttpActionResult PutCurrent([FromBody] NameEntry entry)
        {
            try
            {
                backend.SetCurrent(entry?.Name);
                return GetCurrent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Route("list")]
        [HttpGet]
        public IHttpActionResult GetEntries()
        {
            return Ok(backend.GetEntries());
        }

        [Route("listen")]
        [HttpGet]
        public IHttpActionResult GetListenAddress()
        {
            return Ok(backend.GetListenAddress());
        }
    }
}

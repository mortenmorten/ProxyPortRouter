namespace ProxyPortRouter.Core.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [RoutePrefix("api/entry")]
    public class EntryController : ApiController
    {
        private readonly IPortProxyController proxyController;

        public EntryController(IPortProxyController proxyController)
        {
            this.proxyController = proxyController;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetCurrent()
        {
            var entry = proxyController.GetCurrentEntry();
            if (entry == null)
            {
                return Ok();
            }

            return Ok(entry);
        }

        [Route("")]
        [HttpPut]
        public IHttpActionResult PutCurrent(string name)
        {
            try
            {
                this.proxyController.SetCurrentEntry(name);
                return this.GetCurrent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Route("list")]
        [HttpGet]
        public IEnumerable<CommandEntry> GetEntries()
        {
            return this.proxyController.GetEntries();
        }
    }
}

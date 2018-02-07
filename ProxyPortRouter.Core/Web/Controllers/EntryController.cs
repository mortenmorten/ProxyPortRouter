namespace ProxyPortRouter.Core.Web.Controllers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    [Route("api/[controller]")]
    public class EntryController : Controller
    {
        private readonly IPortProxyController proxyController;

        public EntryController(IPortProxyController proxyController)
        {
            this.proxyController = proxyController;
        }

        [HttpGet]
        public IActionResult GetCurrent()
        {
            var entry = this.proxyController.GetCurrentEntry();
            if (entry == null)
            {
                return this.NoContent();
            }

            return new OkObjectResult(entry);
        }

        [HttpPut]
        public IActionResult PutCurrent(string name)
        {
            try
            {
                this.proxyController.SetCurrentEntry(name);
                return this.GetCurrent();
            }
            catch (InvalidOperationException exception)
            {
                return this.NotFound(exception.Message);
            }
        }

        [HttpGet("list")]
        public IEnumerable<CommandEntry> GetEntries()
        {
            return this.proxyController.GetEntries();
        }
    }
}

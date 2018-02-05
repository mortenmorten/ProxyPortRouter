using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ProxyPortRouter.Web.Controllers
{
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
            var entry = proxyController.GetCurrentEntry();
            if (entry == null) return NoContent();
            return new OkObjectResult(entry);
        }

        [HttpPut]
        public IActionResult PutCurrent(string name)
        {
            try
            {
                proxyController.SetCurrentEntry(name);
                return Ok();
            }
            catch (InvalidOperationException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpGet("list")]
        public IEnumerable<CommandEntry> GetEntries()
        {
            return proxyController.GetEntries();
        }
    }
}

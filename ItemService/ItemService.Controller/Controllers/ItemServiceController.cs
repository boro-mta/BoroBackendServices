using ItemService.API.Interfaces;
using ItemService.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItemService.Controller.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemServiceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IItemServiceBackend _backend;

        public ItemServiceController(ILoggerFactory loggerFactory,
            IItemServiceBackend backend)
        {
            _logger = loggerFactory.CreateLogger("ItemServiceService");
            _backend = backend;
        }

        [HttpGet("IsAlive")]
        public bool IsAlive() => true;

        [HttpGet()]
        public ActionResult<ItemServiceModel> GetItemService(int id)
        {
            _logger.LogInformation("GetItemService was called with id: [{id}]", id);

            var template = _backend.GetItemService(id);

            _logger.LogInformation("GetItemService - Finished with: [{@template}]", template);

            return Ok(template);
        }

        [HttpPost()]
        public ActionResult<bool> PostItemService([FromBody] ItemServiceModel template)
        {
            _logger.LogInformation("PostItemService was called with [{template}]", template);

            var result = _backend.SetItemService(template);

            _logger.LogInformation("PostItemService - Finished with result: [{result}]", result);

            return Ok(result);
        }
    }
}
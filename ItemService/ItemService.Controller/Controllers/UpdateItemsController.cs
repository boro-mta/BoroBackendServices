using Boro.Validations;
using ItemService.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItemService.Controller.Controllers;

[Route("Items/{itemId}/Update")]
[ApiController]
public partial class UpdateItemsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IItemServiceBackend _backend;

    public UpdateItemsController(ILoggerFactory loggerFactory,
        IItemServiceBackend backend)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _backend = backend;
    }

    [HttpPost("Description")]
    [ValidatesGuid("itemId")]
    public ActionResult UpdateDescription(string itemId, [FromBody] string description)
    {
        _logger.LogInformation("UpdateDescription was called with id: [{itemId}]", itemId);
        var guid = Guid.Parse(itemId);
        //var item = _backend.GetItem(guid);

        _logger.LogInformation("UpdateDescription - Finished with: [{guid}]", guid);

        return Ok();
    }
}

using Boro.Common.Authentication;
using Boro.Common.Exceptions;
using Boro.Validations;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItemService.Controller.Controllers;

[Route("Items/{itemId}/Update")]
[ApiController]
[Authorize(Policy = AuthPolicies.ItemOwner)]
[ValidatesGuid("itemId")]
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

    [HttpPost("Location")]
    public ActionResult UpdateItemLocation(string itemId, double latitude, double longitude)
    {
        try
        {
            _logger.LogInformation("UpdateItemLocation was called with id: [{itemId}] and new location: {@latitude} {@longitude}",
                itemId, latitude, longitude);

            var guid = Guid.Parse(itemId);

            _backend.UpdateItemLocation(guid, latitude, longitude).Wait();

            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id: {itemId} was not found");
        }
    }

    [HttpPost()]
    public ActionResult UpdateItemInfo(string itemId, [FromBody] UpdateItemInfoInput updateInput)
    {
        try
        {
            _logger.LogInformation("UpdateItemInfo was called with id: [{itemId}] and new information: {@info}",
                itemId, updateInput);

            var guid = Guid.Parse(itemId);
            _backend.UpdateItemInfo(guid, updateInput).Wait();

            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id: {itemId} was not found");
        }
    }

}

using ItemService.API.Exceptions;
using ItemService.API.Interfaces;
using ItemService.API.Models;
using ItemService.Controller.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace ItemService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IItemServiceBackend _backend;

    public ItemsController(ILoggerFactory loggerFactory,
        IItemServiceBackend backend)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _backend = backend;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemModel), statusCode: 200)]
    [ProducesResponseType(typeof(string), statusCode: 404)]
    [ProducesResponseType(typeof(string), statusCode: 400)]
    public ActionResult<ItemModel> GetItem(string id)
    {
        _logger.LogInformation("GetItem was called with id: [{id}]", id);
        if (Guid.TryParse(id, out var guid))
        {
            var item = _backend.GetItem(guid);

            _logger.LogInformation("GetItem - Finished with: [{@item}]", item);

            return item is null ? NotFound($"Item with id: {id} was not found") : Ok(item);
        }
        else
        {
            _logger.LogError("GetItem - [{id}] is not a valid Guid", id);
            return BadRequest($"[{id}] is not a valid Guid");
        }
    }

    [HttpGet()]
    [ProducesResponseType(typeof(List<ItemModel>), statusCode: 200)]
    [ProducesResponseType(typeof(string), statusCode: 400)]
    [ProducesResponseType(typeof(string), statusCode: 404)]
    public ActionResult<List<ItemModel>> GetItems([FromBody][MinLength(1)] List<string> ids)
    {
        _logger.LogInformation("GetItems was called with ids: [{@ids}]", ids);
        if (ids.AreValidGuids(out var guids))
        {
            var items = _backend.GetItems(guids);
            _logger.LogInformation("GetItems - Finished with: [{@items}]", items);

            return items.Any() ? Ok(items) : NotFound("No item was found with the requested ids");
        }
        else
        {
            _logger.LogError("GetItems - One or more invalid guids received");
            return BadRequest("One or more input id is not a valid Guid");
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(Guid), statusCode: 200)]
    [ProducesResponseType(typeof(string), statusCode: 400)]
    public ActionResult<Guid> PostItem([FromBody] ItemInput item)
    {
        _logger.LogInformation("PostItem was called with [{@item}]", item);

        if (!item.IsValidInput(out var errors))
        {
            _logger.LogError("PostItem - invalid input received with following issues: [{@errors}]", errors);
            return BadRequest(
                $"""
                Bad Request. Invalid input due to following issues:
                {string.Join("\n", errors.Select(e => $"- {e}"))}
                """);
        }

        var guid = _backend.AddItem(item);

        _logger.LogInformation("PostItem Finished with: [{guid}]", guid);

        return Ok(guid);
    }

    [HttpPost("{id}/Update")]
    [ProducesResponseType(statusCode: 200)]
    [ProducesResponseType(typeof(string), statusCode: 400)]
    [ProducesResponseType(typeof(string), statusCode: 404)]
    [ProducesResponseType(typeof(string), statusCode: 409)]
    public ActionResult UpdateItem(string id, [FromBody] ItemInput item)
    {
        try
        {
            _logger.LogInformation("UpdateItem was called with: [{@item}]", item);

            if (!Guid.TryParse(id, out var guid))
            {
                _logger.LogError("UpdateItem - [{id}] is not a valid Guid", id);
                return BadRequest($"[{id}] is not a valid Guid");
            }
            if (!item.IsValidInput(out var errors))
            {
                _logger.LogError("UpdateItem - invalid input received with following issues: [{@errors}]", errors);
                return BadRequest(
                    $"""
                Bad Request. Invalid input due to following issues:
                {string.Join("\n", errors.Select(e => $"- {e}"))}
                """);
            }

            _backend.UpdateItem(guid, item);

            _logger.LogInformation("UpdateItem - Item with [{@id}] was updated", id);

            return Ok();
        }
        catch (DoesNotExistException e)
        {
            _logger.LogError(e, "UpdateItem - ERROR");
            return NotFound($"Item with id: {id} was not found");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "UpdateItem - Unknown error occurred.");
            return Conflict("Request could not be made");
        }
    }
}
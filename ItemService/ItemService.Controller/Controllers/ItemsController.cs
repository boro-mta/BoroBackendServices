using Boro.Validations;
using ItemService.API.Exceptions;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using ItemService.Controller.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace ItemService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
public partial class ItemsController : ControllerBase
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
    [ValidatesGuid("id")]
    public ActionResult<ItemModel> GetItem(string id)
    {
        _logger.LogInformation("GetItem was called with id: [{id}]", id);
        var guid = Guid.Parse(id);
        var item = _backend.GetItem(guid);

        _logger.LogInformation("GetItem - Finished with: [{id}]", item?.Id);

        return item is null ? NotFound($"Item with id: {id} was not found") : item;
    }

    [HttpGet()]
    [ValidatesGuid("ids")]
    public ActionResult<List<ItemModel>> GetItems([FromBody][MinLength(1)] List<string> ids)
    {
        _logger.LogInformation("GetItems was called with ids: [{@ids}]", ids);
        var guids = ids.Select(Guid.Parse);
        var items = _backend.GetItems(guids);
        _logger.LogInformation("GetItems - Finished with: [{@ids}]", items.Select(i => i.Id));

        return items.Any() ? items : NotFound("No image was found with the requested ids");
    }

    [HttpPost("Add")]
    public ActionResult<Guid> AddItem([FromBody] ItemInput item)
    {
        _logger.LogInformation("PostItem was called with [{title}, {description}, {ownerId}]", item.Title, item.Description, item.OwnerId);

        if (!item.IsValidInput(out var errors))
        {
            _logger.LogError("PostItem - invalid input received with following issues: [{@errors}]", errors);
            return BadRequest(
                $"""
                Bad Request. Invalid input due to following issues:
                {string.Join(Environment.NewLine, errors.Select(e => $"- {e}"))}
                """);
        }

        var guid = _backend.AddItem(item);

        _logger.LogInformation("PostItem Finished with: [{guid}]", guid);

        return guid;
    }

    [HttpPost("Update/{id}")]
    [ValidatesGuid("id")]
    public ActionResult UpdateItem(string id, [FromBody] UpdateItemInput item)
    {
        try
        {
            var guid = Guid.Parse(id);
            _logger.LogInformation("UpdateItem was called with: [{@image}]", item);

            
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
            _logger.LogCritical(e, "UpdateItem - an error occurred.");
            return Conflict("Request could not be made");
        }
    }

    [HttpPost("Update/{itemId}/AddImage")]
    [ValidatesGuid("itemId")]
    public ActionResult<Guid> AddImage(string itemId, [FromBody] ItemImageInput image)
    {
        try
        {
            var guid = Guid.Parse(itemId);
            _logger.LogInformation("AddImage was called with: [{@metadata}, {@imageLength}, {@isCover}]", 
                image.Base64ImageMetaData, image.Base64ImageData.Length, image.IsCover);
            
            if (!image.IsValidInput(out var errors))
            {
                _logger.LogError("AddImage - invalid input received with following issues: [{@errors}]", errors);
                return BadRequest(
                    $"""
                    Bad Request. Invalid input due to following issues:
                    {string.Join("\n", errors.Select(e => $"- {e}"))}
                    """);
            }

            _backend.AddImage(guid, image);

            _logger.LogInformation("AddImage - Item with [{@id}] was updated", itemId);

            return Ok();
        }
        catch (DoesNotExistException e)
        {
            _logger.LogError(e, "AddImage - ERROR");
            return NotFound($"Item with id: {itemId} was not found");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "AddImage - an error occurred.");
            return Conflict("Request could not be made");
        }
    }
}
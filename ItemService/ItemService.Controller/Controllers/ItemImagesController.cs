using Boro.Common.Authentication;
using Boro.Common.Exceptions;
using Boro.Validations;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItemService.Controller.Controllers;

[Route("Items/{itemId}/Images")]
[ApiController]
[Authorize]
[ValidatesGuid("itemId")]
public partial class ItemImagesController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IImagesBackend _backend;

    public ItemImagesController(ILoggerFactory loggerFactory,
        IImagesBackend backend)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _backend = backend;
    }

    [HttpPost("Add")]
    [Authorize(Policy = AuthPolicies.ItemOwner)]
    public ActionResult<Guid> AddImage(string itemId, [FromBody] ItemImageInput image)
    {
        try
        {
            var guid = Guid.Parse(itemId);
            _logger.LogInformation("AddImage was called with: [{@metadata}, {@imageLength}]",
                image.Base64ImageMetaData, image.Base64ImageData.Length);

            var imageId = _backend.AddImageAsync(guid, image).Result;

            _logger.LogInformation("AddImage - Item with [{@id}] was updated with image [{imageId}]", itemId, imageId);

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

    [HttpGet()]
    public ActionResult<List<ItemImage>> GetItemImages(string itemId)
    {
        try
        {
            _logger.LogInformation("GetItemImages was called with item id: [{itemId}]", itemId);
            var guid = Guid.Parse(itemId);

            var images = _backend.GetAllItemImagesAsync(guid).Result;

            _logger.LogInformation("GetItemImages - Item id: [{@id}]. Retrieved [{count}] images.", 
                itemId, images.Count);

            return Ok();
        }
        catch (DoesNotExistException e)
        {
            _logger.LogError(e, "GetItemImages - {@itemId} - ERROR", itemId);
            return NotFound($"No images were found for item with id: {itemId}");
        }
    }
}



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
    private readonly IItemServiceBackend _itemBackend;
    private readonly IImagesBackend _imagesBackend;

    public UpdateItemsController(ILoggerFactory loggerFactory,
        IItemServiceBackend backend,
        IImagesBackend imagesBackend)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _itemBackend = backend;
        _imagesBackend = imagesBackend;
    }

    [HttpPost("Images/AddImages")]
    [Authorize(Policy = AuthPolicies.ItemOwner)]
    public async Task<ActionResult<List<Guid>>> AddImages(string itemId, [FromBody] ItemImageInput[] images)
    {
        try
        {
            var guid = Guid.Parse(itemId);
            _logger.LogInformation("AddImages was called with: [{count}] images",
                images.Length);

            var imageIds = await _imagesBackend.AddImagesAsync(guid, images);

            _logger.LogInformation("AddImages - Item with [{@id}] was updated with images [{@imageIds}]", itemId, imageIds);

            return Ok(imageIds);
        }
        catch (DoesNotExistException e)
        {
            _logger.LogError(e, "AddImages - ERROR");
            return NotFound($"Item with id: {itemId} was not found");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "AddImages - an error occurred.");
            return Conflict("Request could not be made");
        }
    }

    [HttpPost("Images/AddImage")]
    public async Task<ActionResult<Guid>> AddImage(string itemId, [FromBody] ItemImageInput image)
    {
        try
        {
            var guid = Guid.Parse(itemId);
            _logger.LogInformation("AddImage was called with: [{@metadata}, {@imageLength}]",
                image.Base64ImageMetaData, image.Base64ImageData.Length);

            var imageId = await _imagesBackend.AddImageAsync(guid, image);

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

    [HttpPost("Location")]
    public async Task<ActionResult<string>> UpdateItemLocation(string itemId, double latitude, double longitude)
    {
        try
        {
            _logger.LogInformation("UpdateItemLocation was called with id: [{itemId}] and new location: {@latitude} {@longitude}",
                itemId, latitude, longitude);

            var guid = Guid.Parse(itemId);

            await _itemBackend.UpdateItemLocation(guid, latitude, longitude);

            return Ok($"item: [{itemId}]. Location updated to lat: [{latitude}], lon: [{longitude}]");
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id: {itemId} was not found");
        }
    }

    [HttpPost()]
    public async Task<ActionResult<string>> UpdateItemInfo(string itemId, [FromBody] UpdateItemInfoInput updateInput)
    {
        try
        {
            _logger.LogInformation("UpdateItemInfo was called with id: [{itemId}] and new information: {@info}",
                itemId, updateInput);

            var guid = Guid.Parse(itemId);
            await _itemBackend.UpdateItemInfo(guid, updateInput);

            return Ok($"item [{itemId}] info updated. [{updateInput.ImagesToRemove?.Count ?? 0}] images deleted.");
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id: {itemId} was not found");
        }
    }

}

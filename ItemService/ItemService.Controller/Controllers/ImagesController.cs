using Boro.Common.Authentication;
using Boro.Common.Exceptions;
using Boro.Validations;
using ItemService.API.Interfaces;
using ItemService.API.Models.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItemService.Controller.Controllers;

[Route("Items/Images")]
[ApiController]
[Authorize]
public partial class ImagesController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IImagesBackend _backend;

    public ImagesController(ILoggerFactory loggerFactory,
        IImagesBackend backend)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _backend = backend;
    }

    [HttpGet("{imageId}")]
    [ValidatesGuid("imageId")]
    public ActionResult<ItemImage> GetImage(string imageId)
    {
        try
        {
            var guid = Guid.Parse(imageId);
            _logger.LogInformation("GetImage was called with: [{imageId}]", imageId);

            var image = _backend.GetImageAsync(guid).Result;

            _logger.LogInformation("GetImage - received image with: [{@length}], [{@metaData}]",
                image.Base64ImageData.Length,
                image.Base64ImageMetaData);

            return Ok(image);
        }
        catch (DoesNotExistException e)
        {
            _logger.LogError(e, "GetImage - ERROR");
            return NotFound($"Image with id: {imageId} was not found");
        }
    }

    [HttpDelete("{imageId}")]
    [ValidatesGuid("imageId")]
    [Authorize(Policy = AuthPolicies.ImageOwner)]
    public ActionResult DeleteImage(string imageId)
    {
        try
        {
            _logger.LogInformation("DeleteImage was called with item id: [{itemId}]", imageId);
            var guid = Guid.Parse(imageId);

            _backend.DeleteImageAsync(guid).Wait();

            return Ok();
        }
        catch (DoesNotExistException e)
        {
            _logger.LogError(e, "DeleteImage - ERROR");
            return NotFound($"Image with id: {imageId} was not found");
        }
    }
}



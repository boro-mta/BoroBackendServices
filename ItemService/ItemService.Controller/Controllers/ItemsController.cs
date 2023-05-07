using Boro.Validations;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ItemService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
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

    [HttpGet("{itemId}")]
    [ValidatesGuid("itemId")]
    public ActionResult<ItemModel> GetItem(string itemId)
    {
        _logger.LogInformation("GetItem was called with id: [{id}]", itemId);
        var guid = Guid.Parse(itemId);
        var item = _backend.GetItemAsync(guid).Result;

        _logger.LogInformation("GetItem - Finished with: [{id}]", item?.Id);
        
        return item is null ? NotFound($"Item with id: {itemId} was not found") : item;
    }

    [HttpGet("OfUser/{userId}")]
    [ValidatesGuid("userId")]
    public ActionResult<List<MinimalItemInfo>> GetAllUserItems(string userId)
    {
        _logger.LogInformation("GetAllUserItems was called with userId: [{userId}]", userId);

        var guid = Guid.Parse(userId);
        var items = _backend.GetAllUserItemsAsync(guid).Result;

        _logger.LogInformation("GetAllUserItems - returning [{count}] items with the following ids: {@items}", items.Count, items.Select(i => i.Id));

        return items.IsNullOrEmpty() ? NotFound($"user [{userId}] has no items") : Ok(items);
    }

    [HttpGet("ByRadius")]
    public ActionResult<List<ItemLocationDetails>> GetAllItemsInRadius(double latitude, double longitude, double radiusInMeters)
    {
        _logger.LogInformation("GetAllItemsInRadius was called with latitude: [{lat}], longitude: [{lon}], radius: [{r}meters]", 
            latitude, longitude, radiusInMeters);

        var items = _backend.GetAllItemsInRadiusAsync(latitude, longitude, radiusInMeters).Result;

        _logger.LogInformation("GetAllItemsInRadius - returning [{count}] items with the following ids: {@items}", items.Count, items.Select(i => i.Id));

        return items.IsNullOrEmpty() ? NotFound($"no items found [{radiusInMeters}] meters around [latitude:{latitude}, longitud:{longitude}]") : Ok(items);
    }

    [HttpPost("Add")]
    public ActionResult<Guid> AddItem([FromBody] ItemInput item)
    {
        _logger.LogInformation("AddItem was called with [{title}, {description}, {ownerId}]", item.Title, item.Description, item.OwnerId);

        var guid = _backend.AddItemAsync(item).Result;

        _logger.LogInformation("AddItem Finished. New item was created with id: [{guid}]", guid);

        return Ok(guid);
    }
}
﻿using Boro.Common.Authentication;
using Boro.Common.Exceptions;
using Boro.Validations;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

    [HttpGet("{itemId}")]
    [ValidatesGuid("itemId")]
    public async Task<ActionResult<ItemModel>> GetItem(string itemId)
    {
        _logger.LogInformation("GetItem was called with id: [{id}]", itemId);
        var guid = Guid.Parse(itemId);
        var item = await _backend.GetItemAsync(guid);

        _logger.LogInformation("GetItem - Finished with: [{id}]", item?.Id);
        
        return item is null ? NotFound($"Item with id: {itemId} was not found") : item;
    }

    [HttpGet("OfUser/{userId}")]
    [ValidatesGuid("userId")]
    public async Task<ActionResult<List<MinimalItemInfo>>> GetAllUserItems(string userId)
    {
        try
        {
            _logger.LogInformation("GetAllUserItems was called with userId: [{userId}]", userId);

            var guid = Guid.Parse(userId);
            var items = await _backend.GetAllUserItemsAsync(guid);

            _logger.LogInformation("GetAllUserItems - returning [{count}] items with the following ids: {@items}", items.Count, items.Select(i => i.Id));

            return Ok(items);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"no user with id: [{userId}]");
        }
    }

    [HttpGet("ByRadius")]
    public async Task<ActionResult<List<ItemLocationDetails>>> GetAllItemsInRadius(double latitude, double longitude, double radiusInMeters)
    {
        _logger.LogInformation("GetAllItemsInRadius was called with latitude: [{lat}], longitude: [{lon}], radius: [{r}meters]", 
            latitude, longitude, radiusInMeters);

        var items = await _backend.GetAllItemsInRadiusAsync(latitude, longitude, radiusInMeters);

        _logger.LogInformation("GetAllItemsInRadius - returning [{count}] items with the following ids: {@items}", items.Count, items.Select(i => i.Id));

        return Ok(items);
    }

    [HttpPost("Add")]
    [Authorize]
    public async Task<ActionResult<Guid>> AddItem([FromBody] ItemInput item)
    {
        var userId = User.UserId();
        _logger.LogInformation("AddItem was called with [{title}, {description}, {ownerId}]", item.Title, item.Description, userId);

        var guid = await _backend.AddItemAsync(item, userId);

        _logger.LogInformation("AddItem Finished. New item was created with id: [{guid}]", guid);

        return Ok(guid);
    }
}
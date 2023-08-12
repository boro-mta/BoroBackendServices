using ItemService.API.Interfaces;
using ItemService.API.Models.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItemService.Controller.Controllers;

[Route("Items/Search")]
[ApiController]
public partial class ItemsSearchController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IItemsSearchBackend _searchBackend;

    public ItemsSearchController(ILoggerFactory loggerFactory, IItemsSearchBackend searchBackend)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _searchBackend = searchBackend;
    }

    [HttpGet("ByTitle")]
    public async Task<ActionResult<List<SearchResult>>> ByTitle(string partialTitle, double latitude, double longitude, int radiusInMeters = 5000, int limit = 100)
    {
        _logger.LogInformation("ByTitle - invoked with: [{@title}], [{@latitude}, {@longitude}], [{@radiusInMeters}], [{@limit}]",
            partialTitle, latitude, longitude, radiusInMeters, limit);

        if (radiusInMeters < 1)
        {
            radiusInMeters = 5000;
        }
        if (limit < 1)
        {
            limit = 100;
        }

        var results = await _searchBackend.SearchByTitle(partialTitle, latitude, longitude, radiusInMeters, limit);

        _logger.LogInformation("ByTitle - returning [{count}] results",
            results.Count);

        return Ok(results);
    }
}

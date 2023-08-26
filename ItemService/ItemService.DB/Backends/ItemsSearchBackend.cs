using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
using ItemService.API.Interfaces;
using ItemService.API.Models.Output;
using ItemService.DB.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ItemService.DB.Backends;

public class ItemsSearchBackend : IItemsSearchBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;

    public ItemsSearchBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _dbContext = dbContext;
    }

    public async Task<List<SearchResult>> SearchByTitle(string partialTitle, double latitude, double longitude, int radiusInMeters = 5000, int limit = 100)
    {
        _logger.LogInformation("SearchByTitle - Attempting to fetch search results for item with title: '{title}', in respect to location: [{lat},{lon}], in radius: [{radius}], limiting to [{limit}] results ",
            partialTitle, latitude, longitude, radiusInMeters, limit);

        var items = _dbContext.Items.FilterByRadius(latitude, longitude, radiusInMeters)
                                    .Where(item => item.Title.Contains(partialTitle, StringComparison.InvariantCultureIgnoreCase))
                                    .Take(limit)
                                    .ToList();

        _logger.LogInformation("SerachByTitle - fetched [{count}] results. Attempting to fetch images.", items.Count);

        var itemIds = items.Select(item => item.ItemId)
                           .ToList();

        var singleImages = await _dbContext.ItemImages.Where(image => itemIds.Contains(image.ItemId))
                                                      .GroupBy(image => image.ItemId)
                                                      .Select(group => group.First())
                                                      .ToDictionaryAsync(image => image.ItemId, image => image);

        _logger.LogInformation("SearchByTitle - fetched [{count}] images.", singleImages.Count);

        var searchResults = items.Select(item => (item, image: singleImages.GetValueOrDefault(item.ItemId)))
                                 .Select(pair => pair.item.ToSearchResult(pair.image))
                                 .ToList();

        return searchResults;
    }
}
using Boro.EntityFramework.DbContexts.BoroMainDb;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using ItemService.DB.Extensions;
using ItemService.DB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ItemService.DB.Backends;

public class ItemServiceBackend : IItemServiceBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;
    private readonly GeoCalculator _geoCalculator;

    public ItemServiceBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext,
        GeoCalculator geoCalculator)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _dbContext = dbContext;
        _geoCalculator = geoCalculator;
    }

    public async Task<ItemModel?> GetItemAsync(Guid id)
    {
        _logger.LogInformation("GetItem - Getting item with {id} from Items table", id);

        var item = await _dbContext.Items
                        .Include(item => item.Images)
                        .SingleOrDefaultAsync(item => item.Id.Equals(id));

        if (item is null) 
        {
            _logger.LogError("GetItem - no item with id: [{id}] was found", id);
            return null;
        }

        _logger.LogInformation("GetItem - returning [{id}, {title}]", item.Id, item.Title);

        return item.ToItemServiceModel();
    }

    public async Task<List<ItemModel>> GetItemsAsync(IEnumerable<Guid> ids)
    {
        _logger.LogInformation("GetItems - Getting items with ids from {@ids} from Items table", ids);

        var itemsQ = _dbContext.Items
                        .Include(item => item.Images)
                        .Where(item => ids.Contains(item.Id))
                        .Select(item => item.ToItemServiceModel());

        if (!itemsQ.Any())
        {
            _logger.LogError("GetItems - no items with id from: [{@ids}] were found", ids);
            return Enumerable.Empty<ItemModel>().ToList();
        }

        return await Task.FromResult(itemsQ.ToList());
    }

    public async Task<Guid> AddItemAsync(ItemInput item)
    {
        _logger.LogInformation("AddItem with [{title}, {ownerID}]", item.Title, item.OwnerId);

        var id = Guid.NewGuid();

        var entry = item.ToTableEntry(id);
        _logger.LogInformation("AddItem - Inserting [{id}]", entry.Id);

        await _dbContext.Items.AddAsync(entry);

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("AddItem - Successfully added item with [{id}]", entry.Id);
        return entry.Id;
    }

    public async Task<List<MinimalItemInfo>> GetAllUserItemsAsync(Guid userId)
    {
        var userItems = _dbContext.Items
            .Where(item => item.OwnerId.Equals(userId))
            .Select(item => item.ToMinimalItemInfo())
            .ToList();

        _logger.LogInformation("GetAllUserItems - [{count}] item were found for user [{userId}]",
            userItems.Count, userId);

        return await Task.FromResult(userItems.ToList());
    }

    public async Task<List<ItemLocationDetails>> GetAllItemsInRadiusAsync(double latitude, double longitude, double radiusInMeters)
    {
        var itemsInRadius = _dbContext.Items
            .Where(item => radiusInMeters >= _geoCalculator.Distance(latitude, longitude, item.Latitude, item.Longitude))
            .Select(item => item.ToItemLocationDetails())
            .ToList();

        _logger.LogInformation("GetAllItemsInRadius - [{count}] item were found in radius of [{radius}] meters from [{lat} - {long}]",
            itemsInRadius.Count, radiusInMeters, latitude, longitude);

        return await Task.FromResult(itemsInRadius);
    }
}
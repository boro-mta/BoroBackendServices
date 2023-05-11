using Boro.Common.Exceptions;
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

        var items = await _dbContext.Items
                        .Include(item => item.Images)
                        .Where(item => ids.Contains(item.Id))
                        .Select(item => item.ToItemServiceModel())
                        .ToListAsync();

        if (!items.Any())
        {
            _logger.LogError("GetItems - no items with id from: [{@ids}] were found", ids);
            return Enumerable.Empty<ItemModel>().ToList();
        }

        return items;
    }

    public async Task<Guid> AddItemAsync(ItemInput item, Guid userId)
    {
        _logger.LogInformation("AddItem with [{title}, {ownerID}]", item.Title, userId);

        var itemId = Guid.NewGuid();

        var entry = item.ToTableEntry(itemId, userId);
        _logger.LogInformation("AddItem - Inserting [{id}]", entry.Id);

        await _dbContext.Items.AddAsync(entry);

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("AddItem - Successfully added item with [{id}]", entry.Id);
        return entry.Id;
    }

    public async Task<List<MinimalItemInfo>> GetAllUserItemsAsync(Guid userId)
    {
        var userItems = await _dbContext.Items
            .Where(item => item.OwnerId.Equals(userId))
            .Select(item => item.ToMinimalItemInfo())
            .ToListAsync();

        _logger.LogInformation("GetAllUserItems - [{count}] item were found for user [{userId}]",
            userItems.Count, userId);

        return userItems;
    }

    public async Task<List<ItemLocationDetails>> GetAllItemsInRadiusAsync(double latitude, double longitude, double radiusInMeters)
    {
        var itemsInRadius = await _dbContext.Items
            .Include(item => item.Images)
            .Where(item => radiusInMeters >= _geoCalculator.Distance(latitude, longitude, item.Latitude, item.Longitude))
            .Select(item => item.ToItemLocationDetails())
            .ToListAsync();

        _logger.LogInformation("GetAllItemsInRadius - [{count}] item were found in radius of [{radius}] meters from [{lat} - {long}]",
            itemsInRadius.Count, radiusInMeters, latitude, longitude);

        return itemsInRadius;
    }

    public async Task UpdateItemInfo(Guid itemId, UpdateItemInfoInput updateInput)
    {
        var entry = await _dbContext.Items.FirstOrDefaultAsync() 
            ?? throw new DoesNotExistException(itemId.ToString());

        var updatedEntry = entry.UpdateEntry(updateInput);
        _dbContext.Items.Update(updatedEntry);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateItemLocation(Guid itemId, double latitude, double longitude)
    {
        var entry = await _dbContext.Items.FirstOrDefaultAsync()
            ?? throw new DoesNotExistException(itemId.ToString());

        var updatedEntry = entry.UpdateLocation(latitude, longitude);
        _dbContext.Items.Update(updatedEntry);
        await _dbContext.SaveChangesAsync();
    }
}
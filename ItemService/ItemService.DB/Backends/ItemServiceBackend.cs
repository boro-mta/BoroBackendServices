using Boro.Common.Exceptions;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using ItemService.DB.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ItemService.DB.Backends;

public class ItemServiceBackend : IItemServiceBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;

    public ItemServiceBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _dbContext = dbContext;
    }

    public async Task<ItemModel?> GetItemAsync(Guid id)
    {
        _logger.LogInformation("GetItem - Getting item with {id} from Items table", id);

        var item = await _dbContext.Items
                        .Include(item => item.Images)
                        .SingleOrDefaultAsync(item => item.ItemId.Equals(id));

        if (item is null) 
        {
            _logger.LogError("GetItem - no item with id: [{id}] was found", id);
            return null;
        }

        _logger.LogInformation("GetItem - returning [{id}, {title}]", item.ItemId, item.Title);

        return item.ToItemServiceModel();
    }

    public async Task<List<ItemModel>> GetItemsAsync(IEnumerable<Guid> ids)
    {
        _logger.LogInformation("GetItems - Getting items with ids from {@ids} from Items table", ids);

        var items = await _dbContext.Items
                        .Include(item => item.Images)
                        .Where(item => ids.Contains(item.ItemId))
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
        _logger.LogInformation("AddItem - Inserting [{id}]", entry.ItemId);

        await _dbContext.Items.AddAsync(entry);

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("AddItem - Successfully added item with [{id}]", entry.ItemId);
        return entry.ItemId;
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
        var items = _dbContext.Items
            .FilterByRadius(latitude, longitude, radiusInMeters)
            .ToList();

        var itemsWithImages = (from item in items
                            join imageGroup in _dbContext.ItemImages.GroupBy(image => image.ItemId)
                            on item.ItemId equals imageGroup.Key
                            select item.ToItemLocationDetails(imageGroup.AsEnumerable())).ToList();

        _logger.LogInformation("GetAllItemsInRadius - [{count}] item were found in radius of [{radius}] meters from [{lat} - {long}]",
            itemsWithImages.Count, radiusInMeters, latitude, longitude);

        return await Task.FromResult(itemsWithImages);
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
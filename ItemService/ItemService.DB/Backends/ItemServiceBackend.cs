using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using ItemService.API.Exceptions;
using ItemService.API.Interfaces;
using ItemService.API.Models;
using ItemService.DB.DbContexts;
using ItemService.DB.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ItemService.DB.Backends;

public class ItemServiceBackend : IItemServiceBackend
{
    private readonly ILogger _logger;
    private readonly ItemServiceDbContext _dbContext;

    public ItemServiceBackend(ILoggerFactory loggerFactory,
        ItemServiceDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _dbContext = dbContext;
    }

    public ItemModel? GetItem(Guid id)
    {
        _logger.LogInformation("GetItem - Getting item with {id} from Items table", id);

        var itemQ = _dbContext.Items
                        .Include(item => item.Images)
                        .Where(item => item.Id.Equals(id))
                        .Select(item => item.ToItemServiceModel());

        if (!itemQ.Any()) 
        {
            _logger.LogError("GetItem - no item with id: [{id}] was found", id);
            return null;
        }

        var item = itemQ.Single();
        _logger.LogInformation("GetItem - returning {@item} as a result", item);

        return item;
    }

    public List<ItemModel> GetItems(IEnumerable<Guid> ids)
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

        return itemsQ.ToList();
    }

    public Guid AddItem(ItemInput item)
    {
        _logger.LogInformation("AddItem with [{@item}]", item);

        var id = Guid.NewGuid();

        var entry = item.ToTableEntry(id);
        var coverImage = item.CoverImage?.ToTableEntry(id, true);
        var images = item.Images?.Select(i => i.ToTableEntry(id));
        _logger.LogInformation("AddItem - Inserting [{@entry}]", entry);
        _dbContext.Items.Add(entry);
        if (coverImage is not null)
        {
            _dbContext.ItemImages.Add(coverImage);
        }
        if (images is not null && images.Any())
        {
            _dbContext.ItemImages.AddRange(images);
        }

        _dbContext.SaveChanges();
        _logger.LogInformation("AddItem - Successfully added item with [{@id}]", entry.Id);
        return entry.Id;
    }

    public void UpdateItem(Guid id, ItemInput item)
    {
        if (_dbContext.Items.Any(item => item.Id.Equals(id)))
        {
            throw new DoesNotExistException(id.ToString());
        }
        var entry = item.ToTableEntry(id);

        _logger.LogInformation("Attempting to update item [{id}] with [{@entry}]", id, entry);

        _dbContext.Items.Update(entry);
        _dbContext.SaveChanges();

        _logger.LogInformation("Item with [{id}] was updated", id);
    }
}
using Boro.EntityFramework.DbContexts.BoroMainDb;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using Microsoft.Extensions.Logging;
using ItemService.DB.Extensions;
using Microsoft.IdentityModel.Tokens;
using Boro.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ItemService.DB.Backends;

public class ImagesBackend : IImagesBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;

    public ImagesBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("ItemService");
        _dbContext = dbContext;
    }

    public async Task<Guid> AddImageAsync(Guid itemId, ItemImageInput image)
    {
        var entry = image.ToTableEntry(itemId);

        await _dbContext.ItemImages.AddAsync(entry);
        await _dbContext.SaveChangesAsync();

        return entry.ImageId;
    }

    public async Task DeleteImageAsync(Guid imageId)
    {
        var image = await _dbContext.ItemImages.FirstOrDefaultAsync(image => image.ImageId.Equals(imageId));

        if (image is not null)
        {
            _dbContext.ItemImages.Remove(image);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            throw new DoesNotExistException(imageId.ToString());
        }
    }

    public async Task<List<ItemImage>> GetAllItemImagesAsync(Guid itemId)
    {
        var images = _dbContext.ItemImages
            .Where(image => image.ParentId.Equals(itemId))
            .Select(image => image.ToItemImageModel());

        if (images.IsNullOrEmpty())
        {
            throw new DoesNotExistException(itemId.ToString());
        }

        return await Task.FromResult(images.ToList());
    }

    public async Task<ItemImage> GetImageAsync(Guid imageId)
    {
        var image = await _dbContext.ItemImages
            .SingleOrDefaultAsync(image =>  image.ImageId.Equals(imageId));

        return image is null ? throw new DoesNotExistException(imageId.ToString()) : image.ToItemImageModel();
    }

    public async Task<List<ItemImage>> GetImagesAsync(IEnumerable<Guid> imageIds)
    {
        var images = imageIds.Select(id => GetImageAsync(id).Result).ToList();

        _logger.LogInformation("GetImages - [{count}] images returned from db", images.Count);

        return await Task.FromResult(images);
    }
}

using Boro.EntityFramework.DbContexts.BoroMainDb;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using Microsoft.Extensions.Logging;
using ItemService.DB.Extensions;
using ItemService.API.Exceptions;
using Microsoft.IdentityModel.Tokens;

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

    public Guid AddImage(Guid itemId, ItemImageInput image)
    {
        var entry = image.ToTableEntry(itemId);

        _dbContext.ItemImages.Add(entry);
        _dbContext.SaveChanges();

        return entry.ImageId;
    }

    public void DeleteImage(Guid imageId)
    {
        var image = _dbContext.ItemImages.FirstOrDefault(image => image.ImageId.Equals(imageId));

        if (image is not null)
        {
            _dbContext.ItemImages.Remove(image);
            _dbContext.SaveChanges();
        }
        else
        {
            throw new DoesNotExistException(imageId.ToString());
        }
    }

    public List<ItemImage> GetAllItemImages(Guid itemId)
    {
        var images = _dbContext.ItemImages
            .Where(image => image.ParentId.Equals(itemId))
            .Select(image => image.ToItemImageModel());

        if (images.IsNullOrEmpty())
        {
            throw new DoesNotExistException(itemId.ToString());
        }

        return images.ToList();
    }

    public ItemImage GetImage(Guid imageId)
    {
        var image = _dbContext.ItemImages
            .SingleOrDefault(image =>  image.ImageId.Equals(imageId));

        return image is null ? throw new DoesNotExistException(imageId.ToString()) : image.ToItemImageModel();
    }

    public List<ItemImage> GetImages(IEnumerable<Guid> imageIds)
    {
        var images = imageIds.Select(GetImage).ToList();

        _logger.LogInformation("GetImages - [{count}] images returned from db", images.Count);

        return images;
    }
}

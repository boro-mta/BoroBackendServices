using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

namespace ItemService.API.Interfaces;

public interface IImagesBackend
{
    Task<Guid> AddImageAsync(Guid itemId, ItemImageInput image);

    Task DeleteImageAsync(Guid imageId);

    Task<ItemImage> GetImageAsync(Guid imageId);

    Task<List<ItemImage>> GetImagesAsync(IEnumerable<Guid> imageIds);

    Task<List<ItemImage>> GetAllItemImagesAsync(Guid itemId);
}
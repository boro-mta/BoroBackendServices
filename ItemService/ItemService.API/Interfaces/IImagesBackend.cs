using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

namespace ItemService.API.Interfaces;

public interface IImagesBackend
{
    Guid AddImage(Guid itemId, ItemImageInput image);

    void DeleteImage(Guid imageId);

    ItemImage GetImage(Guid imageId);

    List<ItemImage> GetImages(IEnumerable<Guid> imageIds);

    List<ItemImage> GetAllItemImages(Guid itemId);
}
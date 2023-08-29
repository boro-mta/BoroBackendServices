using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

namespace ItemService.API.Interfaces;

public interface IItemServiceBackend
{
    Task<ItemModel?> GetItemAsync(Guid id);

    Task<List<ItemModel>> GetItemsAsync(IEnumerable<Guid> ids);

    Task<Guid> AddItemAsync(ItemInput item, Guid userId);

    Task<List<MinimalItemInfo>> GetAllUserItemsAsync(Guid userId);

    Task<List<ItemLocationDetails>> GetAllItemsInRadiusAsync(double latitude, double longitude, double radiusInMeters);
    Task UpdateItemInfo(Guid itemId, UpdateItemInfoInput updateInput);
    Task UpdateItemLocation(Guid itemId, double latitude, double longitude);
    Task DeleteItemAsync(Guid guid);
}

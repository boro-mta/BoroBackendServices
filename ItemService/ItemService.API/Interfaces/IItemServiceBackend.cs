using ItemService.API.Models;

namespace ItemService.API.Interfaces;

public interface IItemServiceBackend
{
    ItemModel? GetItem(Guid id);

    List<ItemModel> GetItems(IEnumerable<Guid> ids);

    Guid AddItem(ItemInput item);

    void UpdateItem(Guid id, ItemInput item);
}
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

namespace ItemService.API.Interfaces;

public interface IItemServiceBackend
{
    ItemModel? GetItem(Guid id);

    List<ItemModel> GetItems(IEnumerable<Guid> ids);

    Guid AddItem(ItemInput item);

    void UpdateItem(Guid id, ItemInput item);
}
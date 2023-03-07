using ItemService.API.Models;

namespace ItemService.API.Interfaces
{
    public interface IItemServiceBackend
    {
        ItemServiceModel GetItemService(int id);

        List<ItemServiceModel> GetItemServices();

        bool SetItemService(ItemServiceModel template);
    }
}
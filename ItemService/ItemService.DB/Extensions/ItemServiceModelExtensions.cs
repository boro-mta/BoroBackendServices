using ItemService.API.Models;
using ItemService.DB.DbContexts.Tables;

namespace ItemService.DB.Extensions
{
    internal static class ItemServiceModelExtensions
    {
        internal static ItemServiceModel ToItemServiceModel(this ItemServices templates)
        {
            return new ItemServiceModel
            {
                Id = templates.Id,
                Description = templates.Description,
            };
        }

        internal static ItemServices ToTableEntry(this ItemServiceModel templateModel)
        {
            return new ItemServices
            {
                Id = templateModel.Id,
                Description = templateModel.Description,
            };
        }
    }
}
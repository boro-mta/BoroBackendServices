using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using ItemService.API.Models;

namespace ItemService.DB.Extensions;

internal static class ItemServiceModelExtensions
{
    internal static ItemModel ToItemServiceModel(this ItemsTable entry)
    {
        return new ItemModel
        {
            Id = entry.Id,
            Description = entry.Description,
        };
    }

    internal static ItemsTable ToTableEntry(this ItemInput input, Guid itemId)
    {
        return new ItemsTable
        {
            Id = itemId,
            Description = input.Description,
        };
    }
}
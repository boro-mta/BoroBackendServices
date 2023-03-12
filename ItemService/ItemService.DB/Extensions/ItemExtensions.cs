using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ItemService.DB.Extensions;

internal static class ItemExtensions
{
    internal static ItemModel ToItemServiceModel(this Items entry)
    {
        List<ItemImage> images = entry.Images?.Select(i => i.ToItemImageModel()).ToList() ?? Enumerable.Empty<ItemImage>().ToList();
        return new ItemModel
        {
            Id = entry.Id,
            Title = entry.Title,
            Description = entry.Description,
            Images = images,
            OwnerId = entry.OwnerId,
            IncludedExtras = entry.IncludedExtras is null ? null : JsonSerializer.Deserialize<Dictionary<string, bool>?>(entry.IncludedExtras),
        };
    }

    internal static Items ToTableEntry(this ItemInput input, Guid itemId)
    {
        var images = input.Images?.Select(i => i.ToTableEntry(itemId)) ?? Enumerable.Empty<ItemImages>();

        return new Items
        {
            Id = itemId,
            Title = input.Title,
            Description = input.Description,
            OwnerId = input.OwnerId,
            Images = images.ToList(),
            IncludedExtras = JsonSerializer.Serialize(input.IncludedExtras)
        };
    }
}
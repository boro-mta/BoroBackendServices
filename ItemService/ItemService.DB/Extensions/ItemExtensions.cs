using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using System.Text.Json;

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
            Categories = JsonSerializer.Deserialize<string[]>(entry.Categories) ?? Array.Empty<string>(),
            Condition = entry.Condition,
            Latitude = entry.Latitude,
            Longitude = entry.Longitude,
        };
    }

    internal static MinimalItemInfo ToMinimalItemInfo(this Items entry)
    {
        return new MinimalItemInfo
        {
            Id = entry.Id,
            Title = entry.Title,
        };
    }

    internal static ItemLocationDetails ToItemLocationDetails(this Items entry)
    {
        return new ItemLocationDetails
        {
            Id = entry.Id,
            Title = entry.Title,
            Latitude = entry.Latitude,
            Longitude = entry.Longitude,
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
            Condition = input.Condition,
            Categories = JsonSerializer.Serialize(input.Categories)
        };
    }

    internal static Items UpdateEntry(this Items entry, UpdateItemInfoInput updateInput)
    {
        entry.Description = updateInput.Description;
        entry.Title = updateInput.Title;
        entry.Condition = updateInput.Condition;
        entry.Categories = JsonSerializer.Serialize(updateInput.Categories);
        return entry;
    }
}
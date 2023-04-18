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

    internal static Items UpdateItem(this Items entry, UpdateItemInput update) 
    { 
        if (update.Title != null)
        {
            entry.Title = update.Title;
        }
        if (update.Description != null)
        {
            entry.Description = update.Description;
        }
        if (update.OwnerId != null)
        {
            entry.OwnerId = update.OwnerId;
        }

        return entry;
    }
}
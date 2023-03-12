﻿using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Boro.EntityFramework.Extensions;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

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
            OwnerId = entry.OwnerId
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
            Images = images.ToList()
        };
    }
}
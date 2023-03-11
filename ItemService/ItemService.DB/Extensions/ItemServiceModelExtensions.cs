using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Boro.EntityFramework.Extensions;
using ItemService.API.Models;

namespace ItemService.DB.Extensions;

internal static class ItemServiceModelExtensions
{
    internal static ItemModel ToItemServiceModel(this Items entry)
    {
        string? coverImage = entry.Images?.Where(i => i.IsCover)?.FirstOrDefault()?.Image.ToBase64String();
        List<string>? stringImages = entry.Images?.Select(i => i.Image.ToBase64String()).ToList();
        return new ItemModel
        {
            Id = entry.Id,
            Title = entry.Title,
            Description = entry.Description,
            CoverImage = coverImage,
            Images = stringImages,
            OwnerId = entry.OwnerId
        };
    }

    internal static Items ToTableEntry(this ItemInput input, Guid itemId)
    {
        return new Items
        {
            Id = itemId,
            Title = input.Title,
            Description = input.Description,
            OwnerId = input.OwnerId
        };
    }

    internal static ItemImages ToTableEntry(this string image, Guid itemId, bool isCover = false)
    {
        return new ItemImages
        {
            ItemId = itemId,
            IsCover = isCover,
            Image = image.FromBase64String()
        };
    }

    internal static ItemImages ToTableEntry(this string image,  Guid itemId)
    {
        return new ItemImages
        {
            ItemId = itemId,
            Image = image.FromBase64String()
        };
    }
}
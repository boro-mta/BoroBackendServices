using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

namespace ItemService.DB.Extensions;

internal static class ItemImageExtensions
{
    internal static ItemImage ToItemImageModel(this ItemImages entry)
    {
        return new ItemImage
        {
            ImageId = entry.ImageId,
            Base64ImageData = entry.ImageData.ToBase64String(),
            Base64ImageMetaData = entry.ImageMetaData,
        };
    }

    internal static ItemImages ToTableEntry(this ItemImageInput image, Guid parentId)
    {
        return new ItemImages
        {
            ImageId = Guid.NewGuid(),
            ParentId = parentId,
            ImageData = image.Base64ImageData.FromBase64String(),
            ImageMetaData = image.Base64ImageMetaData,
        };
    }
}
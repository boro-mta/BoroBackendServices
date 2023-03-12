using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Boro.EntityFramework.Extensions;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

namespace ItemService.DB.Extensions
{
    internal static class ItemImageExtensions
    {

        internal static ItemImage ToItemImageModel(this ItemImages entry)
        {
            return new ItemImage
            {
                FileName = entry.FileName,
                ImageFormat = entry.ImageFormat,
                IsCover = entry.IsCover,
                Base64ImageData = entry.ImageData.ToBase64String()
            };
        }

        internal static ItemImages ToTableEntry(this ItemImageInput image, Guid itemId)
        {
            return new ItemImages
            {
                FileName = image.FileName,
                ImageFormat = image.ImageFormat,
                ItemId = itemId,
                IsCover = image.IsCover,
                ImageData = image.Base64ImageData.FromBase64String()
            };
        }
    }
}
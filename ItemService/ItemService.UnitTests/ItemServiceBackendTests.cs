using Boro.EntityFramework.Extensions;
using ItemService.API.Interfaces;
using ItemService.API.Models;
using ItemService.API.Models.Input;
using ItemService.API.Models.Output;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ItemService.UnitTests;

[TestClass]
public class ItemServiceBackendTests
{
    [TestMethod]
    public void InsertItemAndThenGetIt()
    {
        using var app = TestUtilities.GenerateApp();
        var backend = app?.Services.GetService<IItemServiceBackend>();
        var item1Guid = backend?.AddItem(new());
        Assert.IsNotNull(item1Guid);
        Assert.IsFalse(item1Guid.Equals(Guid.Empty));

        var item = backend?.GetItem(item1Guid.Value);
        Assert.IsNotNull(item);
        Assert.AreEqual(item1Guid, item.Id);
    }

    [TestMethod]
    public void InsertItemWithACoverImageAndThenGetIt()
    {
        using var app = TestUtilities.GenerateApp();
        var backend = app?.Services.GetService<IItemServiceBackend>();
        var imagesPath = @".\Resources";
        var jpeg = imagesPath + @"\tomato.jpeg";
        var png = imagesPath + @"\pngTomato.png";
        var jpegBytes = File.ReadAllBytes(jpeg);
        var pngBytes = File.ReadAllBytes(png);
        var jpegBase64 = jpegBytes.ToBase64String();
        var pngBase64 = pngBytes.ToBase64String();

        var pngCoverImageInput = new ItemImageInput
        {
            FileName = "pngTomato",
            ImageFormat = "png",
            IsCover = true,
            Base64ImageData = pngBase64,
        };
        var jpegImageInput = new ItemImageInput
        {
            FileName = "tomato",
            ImageFormat = "jpeg",
            IsCover = false,
            Base64ImageData = jpegBase64,
        };

        var itemInput = new ItemInput
        {
            Description = "A tomato. Beautiful tomato",
            Title = "My Tomato!",
            Images = new List<ItemImageInput> { pngCoverImageInput, jpegImageInput }
        };

        var itemGuid = backend?.AddItem(itemInput);
        Assert.IsNotNull(itemGuid);
        Assert.IsFalse(itemGuid.Equals(Guid.Empty));

        var item = backend?.GetItem(itemGuid.Value);
        Assert.IsNotNull(item?.Images);
        Assert.AreEqual(itemGuid, item.Id);
        Assert.AreEqual(2, item.Images.Count);
        var actualCover = item.Images.Where(item => item.IsCover).First();
        Assert.AreEqual(pngBase64, actualCover.Base64ImageData);
    }

    [TestMethod]
    public void InsertMultipleItemsAndGetThemInSteps()
    {
        using var app = TestUtilities.GenerateApp();
        var backend = app?.Services.GetService<IItemServiceBackend>();

        var item1Guid = backend?.AddItem(new());
        Assert.IsNotNull(item1Guid);
        var item2Guid = backend?.AddItem(new());
        Assert.IsNotNull(item2Guid);

        Guid[] guids1 = { item1Guid.Value, item2Guid.Value };
        var items1 = backend?.GetItems(guids1);
        Assert.IsNotNull(items1);
        Assert.AreEqual(2, items1.Count);

        var item3Guid = backend?.AddItem(new());
        Assert.IsNotNull(item3Guid);
        var item4Guid = backend?.AddItem(new());
        Assert.IsNotNull(item4Guid);
        var item5Guid = backend?.AddItem(new());
        Assert.IsNotNull(item5Guid);

        Guid[] guids2 = { item4Guid.Value, item2Guid.Value, Guid.NewGuid() };
        var items2 = backend?.GetItems(guids2);
        Assert.IsNotNull(items2);
        Assert.AreEqual(2, items2.Count);

        Guid[] guids3 = { item1Guid.Value, item2Guid.Value, item3Guid.Value, item4Guid.Value, item5Guid.Value};
        var items3 = backend?.GetItems(guids3);
        Assert.IsNotNull(items3);
        Assert.AreEqual(5, items3.Count);
    }
}
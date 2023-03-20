using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
using ItemService.API.Interfaces;
using ItemService.API.Models.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.UnitTests.ItemService;

[TestClass]
public class ItemServiceBackendTests
{
    private IItemServiceBackend? _backend;

    [TestInitialize]
    public void Initialize()
    {
        var app = TestUtilities.GenerateApp();
        _backend = app?.Services.GetService<IItemServiceBackend>();
    }

    [TestMethod]
    public void InsertItemAndThenGetIt()
    {
        Assert.IsNotNull(_backend);

        var item1Guid = _backend.AddItem(new());
        Assert.IsNotNull(item1Guid);
        Assert.IsFalse(item1Guid.Equals(Guid.Empty));

        var item = _backend.GetItem(item1Guid);
        Assert.IsNotNull(item);
        Assert.AreEqual(item1Guid, item.Id);
    }

    [TestMethod]
    public void InsertItemWithIncludedExtrasAndThenGetIt()
    {
        Assert.IsNotNull(_backend);

        Dictionary<string, bool> includedExtras = new()
        {
            ["Bit set"] = true,
            ["Extra Battery"] = true,
            ["Battery Charger"] = false
        };
        ItemInput inputItem = new()
        {
            Title = "Drill",
            Description = "A battery powered drill by Makita",
            IncludedExtras = includedExtras
        };

        var item1Guid = _backend.AddItem(inputItem);
        Assert.IsNotNull(item1Guid);
        Assert.IsFalse(item1Guid.Equals(Guid.Empty));

        var item = _backend.GetItem(item1Guid);
        Assert.IsNotNull(item);
        Assert.AreEqual(item1Guid, item.Id);
        var extras = item.IncludedExtras;
        Assert.IsNotNull(extras);
        Assert.AreEqual(3, extras.Count);
        Assert.AreEqual(includedExtras["Bit set"], extras["Bit set"]);
        Assert.AreEqual(includedExtras["Extra Battery"], extras["Extra Battery"]);
        Assert.AreEqual(includedExtras["Battery Charger"], extras["Battery Charger"]);
    }

    [TestMethod]
    public void InsertItemWithACoverImageAndThenGetIt()
    {
        Assert.IsNotNull(_backend);

        var imagesPath = @".\Resources";
        var jpeg = imagesPath + @"\tomato.jpeg";
        var png = imagesPath + @"\pngTomato.png";
        var jpegBytes = File.ReadAllBytes(jpeg);
        var pngBytes = File.ReadAllBytes(png);
        var jpegBase64 = jpegBytes.ToBase64String();
        var pngBase64 = pngBytes.ToBase64String();

        var pngCoverImageInput = new ItemImageInput
        {
            IsCover = true,
            Base64ImageData = pngBase64,
        };
        var jpegImageInput = new ItemImageInput
        {
            IsCover = false,
            Base64ImageData = jpegBase64,
        };

        var itemInput = new ItemInput
        {
            Description = "A tomato. Beautiful tomato",
            Title = "My Tomato!",
            Images = new List<ItemImageInput> { pngCoverImageInput, jpegImageInput }
        };

        var itemGuid = _backend.AddItem(itemInput);
        Assert.IsNotNull(itemGuid);
        Assert.IsFalse(itemGuid.Equals(Guid.Empty));

        var item = _backend.GetItem(itemGuid);
        Assert.IsNotNull(item?.Images);
        Assert.AreEqual(itemGuid, item.Id);
        Assert.AreEqual(2, item.Images.Count);
        var actualCover = item.Images.Where(item => item.IsCover).First();
        Assert.AreEqual(pngBase64, actualCover.Base64ImageData);
    }

    [TestMethod]
    public void InsertMultipleItemsAndGetThemInSteps()
    {
        Assert.IsNotNull(_backend);

        var item1Guid = _backend.AddItem(new());
        Assert.IsNotNull(item1Guid);
        var item2Guid = _backend.AddItem(new());
        Assert.IsNotNull(item2Guid);

        Guid[] guids1 = { item1Guid, item2Guid };
        var items1 = _backend.GetItems(guids1);
        Assert.IsNotNull(items1);
        Assert.AreEqual(2, items1.Count);

        var item3Guid = _backend.AddItem(new());
        Assert.IsNotNull(item3Guid);
        var item4Guid = _backend.AddItem(new());
        Assert.IsNotNull(item4Guid);
        var item5Guid = _backend.AddItem(new());
        Assert.IsNotNull(item5Guid);

        Guid[] guids2 = { item4Guid, item2Guid, Guid.NewGuid() };
        var items2 = _backend.GetItems(guids2);
        Assert.IsNotNull(items2);
        Assert.AreEqual(2, items2.Count);

        Guid[] guids3 = { item1Guid, item2Guid, item3Guid, item4Guid, item5Guid };
        var items3 = _backend.GetItems(guids3);
        Assert.IsNotNull(items3);
        Assert.AreEqual(5, items3.Count);
    }
}
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
    public async Task InsertItemAndThenGetIt()
    {
        Assert.IsNotNull(_backend);

        var item1Guid = await _backend.AddItemAsync(new(), Guid.NewGuid());
        Assert.IsNotNull(item1Guid);
        Assert.IsFalse(item1Guid.Equals(Guid.Empty));

        var item = await _backend.GetItemAsync(item1Guid);
        Assert.IsNotNull(item);
        Assert.AreEqual(item1Guid, item.Id);
    }

    [TestMethod]
    public async Task InsertItemWithIncludedExtrasAndThenGetIt()
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
            //IncludedExtras = includedExtras
        };

        var item1Guid = await _backend.AddItemAsync(inputItem, Guid.NewGuid());
        Assert.IsNotNull(item1Guid);
        Assert.IsFalse(item1Guid.Equals(Guid.Empty));

        var item = await _backend.GetItemAsync(item1Guid);
        Assert.IsNotNull(item);
        Assert.AreEqual(item1Guid, item.Id);
        //var extras = item.IncludedExtras;
        //Assert.IsNotNull(extras);
        //Assert.AreEqual(3, extras.Count);
        //Assert.AreEqual(includedExtras["Bit set"], extras["Bit set"]);
        //Assert.AreEqual(includedExtras["Extra Battery"], extras["Extra Battery"]);
        //Assert.AreEqual(includedExtras["Battery Charger"], extras["Battery Charger"]);
    }

    [TestMethod]
    public async Task InsertItemWithMultipleImages()
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
            Base64ImageData = pngBase64,
        };
        var jpegImageInput = new ItemImageInput
        {
            Base64ImageData = jpegBase64,
        };

        var itemInput = new ItemInput
        {
            Description = "A tomato. Beautiful tomato",
            Title = "My Tomato!",
            Images = new List<ItemImageInput> { pngCoverImageInput, jpegImageInput }
        };

        var itemGuid = await _backend.AddItemAsync(itemInput, Guid.NewGuid());
        Assert.IsNotNull(itemGuid);
        Assert.IsFalse(itemGuid.Equals(Guid.Empty));

        var item = await _backend.GetItemAsync(itemGuid);
        Assert.IsNotNull(item?.Images);
        Assert.AreEqual(itemGuid, item.Id);
        Assert.AreEqual(2, item.Images.Count);
    }

    [TestMethod]
    public async Task InsertItemWithACoverImageAndThenGetIt()
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
            Base64ImageData = pngBase64,
        };
        var jpegImageInput = new ItemImageInput
        {
            Base64ImageData = jpegBase64,
        };
        var jpeg2ImageInput = new ItemImageInput
        {
            Base64ImageData = jpegBase64,
        };

        var itemInput = new ItemInput
        {
            Description = "A tomato. Beautiful tomato",
            Title = "My Tomato!",
            Images = new List<ItemImageInput> { pngCoverImageInput, jpegImageInput, jpeg2ImageInput }
        };

        var itemGuid = await _backend.AddItemAsync(itemInput, Guid.NewGuid());
        Assert.IsNotNull(itemGuid);
        Assert.IsFalse(itemGuid.Equals(Guid.Empty));

        var item = await _backend.GetItemAsync(itemGuid);
        Assert.IsNotNull(item?.Images);
        Assert.AreEqual(itemGuid, item.Id);
        Assert.AreEqual(3, item.Images.Count);
    }

    [TestMethod]
    public async Task InsertMultipleItemsAndGetThemInSteps()
    {
        Assert.IsNotNull(_backend);

        var item1Guid = await _backend.AddItemAsync(new(), Guid.NewGuid());
        Assert.IsNotNull(item1Guid);
        var item2Guid = await _backend.AddItemAsync(new(), Guid.NewGuid());
        Assert.IsNotNull(item2Guid);

        Guid[] guids1 = { item1Guid, item2Guid };
        var items1 = await _backend.GetItemsAsync(guids1);
        Assert.IsNotNull(items1);
        Assert.AreEqual(2, items1.Count);

        var item3Guid = await _backend.AddItemAsync(new(), Guid.NewGuid());
        Assert.IsNotNull(item3Guid);
        var item4Guid = await _backend.AddItemAsync(new(), Guid.NewGuid());
        Assert.IsNotNull(item4Guid);
        var item5Guid = await _backend.AddItemAsync(new(), Guid.NewGuid());
        Assert.IsNotNull(item5Guid);

        Guid[] guids2 = { item4Guid, item2Guid, Guid.NewGuid() };
        var items2 = await _backend.GetItemsAsync(guids2);
        Assert.IsNotNull(items2);
        Assert.AreEqual(2, items2.Count);

        Guid[] guids3 = { item1Guid, item2Guid, item3Guid, item4Guid, item5Guid };
        var items3 = await _backend.GetItemsAsync(guids3);
        Assert.IsNotNull(items3);
        Assert.AreEqual(5, items3.Count);
    }
}
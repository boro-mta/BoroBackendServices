using ItemService.API.Interfaces;
using ItemService.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ItemService.UnitTests
{
    [TestClass]
    public class ItemServiceBackendTests
    {
        private IServiceProvider? _serviceProvider;
        private WebApplication? _app;
        private IItemServiceBackend? _backend;

        [TestInitialize]
        public void Initialize()
        {
            (_serviceProvider, _app) = TestUtilities.GenerateApp();
            _backend = _app.Services.GetService<IItemServiceBackend>();
        }

        [TestMethod]
        public void ItemServiceBackendTests_InsertItemAndThenGetIt()
        {
            var success = _backend?.SetItemService(new ItemServiceModel { Id = 1 });
            Assert.IsNotNull(success);
            Assert.IsTrue(success);

            var template = _backend?.GetItemService(1);
            Assert.IsNotNull(template);
            Assert.AreEqual(1, template.Id);
        }

        [TestMethod]
        public void ItemServiceBackendTests_InsertMultipleItemsAndGetThem()
        {
            _backend?.SetItemService(new ItemServiceModel { Id = 2 });
            _backend?.SetItemService(new ItemServiceModel { Id = 5 });

            var templates = _backend?.GetItemServices();
            Assert.IsNotNull(templates);
            Assert.AreEqual(2, templates.Count);

            _backend?.SetItemService(new ItemServiceModel { Id = 1 });
            _backend?.SetItemService(new ItemServiceModel { Id = 4 });
            _backend?.SetItemService(new ItemServiceModel { Id = 3 });

            templates = _backend?.GetItemServices();
            Assert.IsNotNull(templates);
            Assert.AreEqual(5, templates.Count);


        }
    }
}
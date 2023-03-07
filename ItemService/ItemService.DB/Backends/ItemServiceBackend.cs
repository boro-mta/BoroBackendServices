using ItemService.API.Interfaces;
using ItemService.API.Models;
using ItemService.DB.DbContexts;
using ItemService.DB.Extensions;
using Microsoft.Extensions.Logging;

namespace ItemService.DB.Backends
{
    public class ItemServiceBackend : IItemServiceBackend
    {
        private readonly ILogger _logger;
        private readonly ItemServiceDbContext _dbContext;

        public ItemServiceBackend(ILoggerFactory loggerFactory,
            ItemServiceDbContext dbContext)
        {
            _logger = loggerFactory.CreateLogger("ItemServiceService");
            _dbContext = dbContext;
        }

        public ItemServiceModel GetItemService(int id)
        {
            var query = from t in _dbContext.ItemServices
                        where t.Id == id
                        select t.ToItemServiceModel();

            return query.Single();
        }

        public List<ItemServiceModel> GetItemServices()
        {
            var query = from t in _dbContext.ItemServices
                        select t.ToItemServiceModel();
            return query.ToList();
        }

        public bool SetItemService(ItemServiceModel template)
        {
            _dbContext.ItemServices.Add(template.ToTableEntry());

            _dbContext.SaveChanges();

            return true;
        }
    }
}
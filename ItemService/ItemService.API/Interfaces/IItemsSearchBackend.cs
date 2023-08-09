using ItemService.API.Models.Input;
using ItemService.API.Models.Output;

namespace ItemService.API.Interfaces;

public interface IItemsSearchBackend
{
    Task<List<SearchResult>> SearchByTitle(string partialTitle, double latitude, double longitude, int radiusInMeters = 5000, int limit = 100);
}

using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Output;
using ReservationsService.DB.Extensions;

namespace ReservationsService.DB.Backends;

public class ReservationsServiceBackend : IReservationsServiceBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;

    public ReservationsServiceBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("ReservationsService");
        _dbContext = dbContext;
    }

    public List<ReservationDates> GetReservedDates(Guid itemId, DateTime fromDate, DateTime toDate)
    {
        var reservationDatesQ = from r in _dbContext.Reservations
                                where r.ItemId.Equals(itemId)
                                && fromDate <= r.From && r.From <= toDate
                                && fromDate <= r.To && r.To <= toDate
                                select r.ToReservationDates();

        return reservationDatesQ.ToList();
    }

    public void AddReservations(Guid itemId, IEnumerable<ReservationDates> reservationDates)
    {
        var itemQ = from i in _dbContext.Items where i.Id.Equals(itemId) select i;
        if (itemQ.Any())
        {
            foreach (var reservation in reservationDates)
            {
                _dbContext.Reservations.Add(reservation.ToTableEntry(itemId));
            }
            _dbContext.SaveChanges();
        }
        else
        {
            throw new Exception("Item not found");
        }
    }
}
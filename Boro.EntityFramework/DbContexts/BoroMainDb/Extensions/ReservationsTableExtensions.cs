using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;

public static class ReservationsTableExtensions
{
    public static IQueryable<Reservations> GetResevationsInPeriod(this DbSet<Reservations> reservations, Guid itemId, DateTime startDate, DateTime endDate)
    {
        return from r in reservations
               where r.ItemId.Equals(itemId)
                     && r.StartDate <= endDate
                     && r.EndDate >= startDate
               select r;
    }

    public static IQueryable<Reservations> GetReservationsInPeriodAndStatus(this DbSet<Reservations> reservations,
                                                                            Guid itemId,
                                                                            DateTime startDate,
                                                                            DateTime endDate,
                                                                            params ReservationStatus[] statuses)
    {
        return from r in reservations
               where r.ItemId.Equals(itemId)
                     && r.StartDate <= endDate
                     && r.EndDate >= startDate
                     && statuses.Select(s => (int)s).ToArray().Contains((int)r.Status)
               select r;
    }

    public static IQueryable<Reservations> GetBlockingResevations(this DbSet<Reservations> reservations, Guid itemId, DateTime startDate, DateTime endDate)
    {
        return reservations.GetReservationsInPeriodAndStatus(itemId,
                                                             startDate,
                                                             endDate,
                                                             Statuses.BlockingStatuses);
    }

    public static IQueryable<Reservations> GetActiveResevations(this DbSet<Reservations> reservations, Guid itemId, DateTime startDate, DateTime endDate)
    {
        return reservations.GetReservationsInPeriodAndStatus(itemId,
                                                             startDate,
                                                             endDate,
                                                             Statuses.ActiveStatuses);
    }

    public static IQueryable<Reservations> GetPendingResevations(this DbSet<Reservations> reservations, Guid itemId, DateTime startDate, DateTime endDate)
    {
        return from r in reservations.GetResevationsInPeriod(itemId, startDate, endDate)
               where r.Status == ReservationStatus.Pending
               select r;
    }

    public static IQueryable<Reservations> GetConcludedResevations(this DbSet<Reservations> reservations, Guid itemId, DateTime startDate, DateTime endDate)
    {
        return reservations.GetReservationsInPeriodAndStatus(itemId,
                                                             startDate,
                                                             endDate,
                                                             Statuses.ConcludedStatuses);
    }
}
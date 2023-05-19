using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Output;
using ReservationsService.DB.Extensions;
using System;

namespace ReservationsService.DB.Backends;

public class ReservationsDashboardBackend : IReservationsDashboardBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;

    public ReservationsDashboardBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("ReservationsService");
        _dbContext = dbContext;
    }

    public async Task<List<ReservationDetails>> GetBorrowersDashboard(Guid borrowerId, DateTime from, DateTime to)
    {
        var active = Statuses.ActiveStatuses;
        var outgoingReservations = from r in _dbContext.Reservations
                                   where r.BorrowerId == borrowerId
                                   && active.Any(s => s == r.Status)
                                   select r.ToReservationDetails();
        return await outgoingReservations.ToListAsync();
    }

    public async Task<List<ReservationDetails>> GetBorrowersUpcoming(Guid borrowerId)
    {
        var active = Statuses.ActiveStatuses;
        var upcomingQ = from r in _dbContext.Reservations
                        where r.BorrowerId == borrowerId
                        && r.StartDate >= DateTime.UtcNow
                        && active.Any(s => s == r.Status)
                        select r.ToReservationDetails();
        return await upcomingQ.ToListAsync();
    }

    public async Task<List<ReservationDetails>> GetLendersDashboard(Guid lenderId, DateTime from, DateTime to)
    {
        var active = Statuses.ActiveStatuses;

        var incomingReservations = from r in _dbContext.Reservations
                                   where r.LenderId == lenderId
                                   && active.Any(s => s == r.Status)
                                   select r.ToReservationDetails();
        return await incomingReservations.ToListAsync();
    }

    public async Task<List<ReservationDetails>> GetLendersUpcoming(Guid lenderId)
    {
        var active = Statuses.ActiveStatuses;
        var upcomingQ = from r in _dbContext.Reservations
                        where r.LenderId == lenderId
                        && r.StartDate >= DateTime.UtcNow
                        && active.Any(s => s == r.Status)
                        select r.ToReservationDetails();
        return await upcomingQ.ToListAsync();
    }
}

using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Output;
using ReservationsService.DB.Extensions;
using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
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

    public async Task<List<ReservationDetails>> GetBorrowersDashboard(Guid borrowerId, DateTime startDate, DateTime endDate)
    {
        var outgoingReservations = from r in _dbContext.Reservations.GetResevationsInPeriod(startDate, endDate)
                                   where r.BorrowerId == borrowerId
                                   select r;

        return await outgoingReservations
            .Select(r => r.ToReservationDetails())
            .ToListAsync();
    }

    public async Task<List<ReservationDetails>> GetBorrowersUpcoming(Guid borrowerId)
    {
        var upcomingQ = from r in _dbContext.Reservations.GetReservationsInStatus(Statuses.ActiveStatuses)
                        where r.BorrowerId == borrowerId
                        && r.StartDate >= DateTime.UtcNow
                        select r;

        return await upcomingQ
            .Select(r => r.ToReservationDetails())
            .ToListAsync();
    }

    public async Task<List<ReservationDetails>> GetLendersDashboard(Guid lenderId, DateTime startDate, DateTime endDate)
    {
        var incomingReservations = from r in _dbContext.Reservations.GetResevationsInPeriod(startDate, endDate)
                                   where r.LenderId == lenderId
                                   select r;

        return await incomingReservations
            .Select(r => r.ToReservationDetails())
            .ToListAsync();
    }

    public async Task<List<ReservationDetails>> GetLendersUpcoming(Guid lenderId)
    {
        var upcomingQ = from r in _dbContext.Reservations.GetReservationsInStatus(Statuses.ActiveStatuses)
                        where r.LenderId == lenderId
                        && r.StartDate >= DateTime.UtcNow
                        select r;

        return await upcomingQ
            .Select(r => r.ToReservationDetails())
            .ToListAsync();
    }
}

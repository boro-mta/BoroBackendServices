using Boro.Common.Exceptions;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Input;
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

    public async Task<List<ReservedDates>> GetReservedDates(Guid itemId, DateTime startDate, DateTime endDate)
    {
        var reservedDatesQ = from r in _dbContext.Reservations
                             where r.ItemId.Equals(itemId)
                                   && r.StartDate <= endDate
                                   && r.EndDate >= startDate
                                   && r.Status >= ReservationStatus.Approved
                             select r.ToReservedDates();

        var blockedDatesQ = from bd in _dbContext.BlockedDates
                            where bd.ItemId.Equals(itemId)
                                  && startDate <= bd.Date
                                  && bd.Date <= endDate
                            select bd.Date;

        if (await blockedDatesQ.AnyAsync())
        {
            var blocked = blockedDatesQ.GetContiguousPeriods().Select(p => new ReservedDates
            {
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                IsBlockedByOwner = true,
                Status = ReservationStatus.None
            });

            reservedDatesQ = reservedDatesQ.Union(blocked);
        }

        return await reservedDatesQ.ToListAsync();
    }

    public async Task<ReservationRequestResult> AddReservationRequest(Guid itemId, ReservationRequestInput reservationRequestInput)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id.Equals(itemId))
            ?? throw new DoesNotExistException(itemId.ToString());

        var startDate = reservationRequestInput.StartDate;
        var endDate = reservationRequestInput.EndDate;

        var reserved = await GetReservedDates(itemId, startDate, endDate);
        if (reserved.Any())
        {
            return ReservationRequestResult.DateConflict;
        }

        var reservationId = Guid.NewGuid();
        var entry = reservationRequestInput.ToTableEntry(reservationId, itemId, item.OwnerId);
        await _dbContext.Reservations.AddAsync(entry);

        return ReservationRequestResult.RequestCreated;
    }

    public async Task<List<ReservationDetails>> GetPendingReservations(Guid itemId)
    {
        var reservationDatesQ = from r in _dbContext.Reservations
                                where r.ItemId.Equals(itemId)
                                      && r.Status == ReservationStatus.Pending
                                select r.ToReservationDetails();

        return await reservationDatesQ.ToListAsync();
    }

    public async Task BlockDates(Guid itemId, DateTime startDate, DateTime endDate)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id.Equals(itemId))
            ?? throw new DoesNotExistException(itemId.ToString());

        var existingReservationsQ = from r in _dbContext.Reservations
                                    where r.ItemId.Equals(itemId)
                                          && r.StartDate <= endDate
                                          && r.EndDate >= startDate
                                          && r.Status >= ReservationStatus.Pending
                                    select r;

        if (existingReservationsQ.Any())
        {
            throw new DateConflictException(startDate, endDate);
        }

        var blockedDatesQ = from bd in _dbContext.BlockedDates
                            where bd.ItemId.Equals(itemId)
                                  && startDate <= bd.Date
                                  && bd.Date <= endDate
                            select bd.Date;

        var blockedDates = await blockedDatesQ.ToListAsync();
        var unblockedDates = new List<DateTime>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {

        }
    }
}
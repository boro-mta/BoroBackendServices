﻿using Boro.Common.Exceptions;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
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

    public async Task<List<DateTime>> GetReservedDates(Guid itemId, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("GetReservedDates - fetching all blocked or reserved dates for item {itemId} between {from} and {to}",
            itemId, startDate, endDate);

        var reservedPeriodsQ = _dbContext.Reservations
            .GetBlockingResevations(itemId, startDate, endDate)
            .Select(r => r.ReservationPeriod());

        var blockedDatesQ = _dbContext.BlockedDates
            .GetBlockedDates(itemId, startDate, endDate)
            .GetContiguousPeriods();

        reservedPeriodsQ = reservedPeriodsQ.Union(blockedDatesQ);

        var datesQ = reservedPeriodsQ
            .SelectMany(p => p.Dates())
            .DistinctBy(d => d.Date)
            .OrderBy(d => d.Date);

        return await datesQ.ToListAsync();
    }

    public async Task<List<DateTime>> GetUserBlockedDates(Guid itemId, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("GetUserBlockedDates - fetching all dates blocked by the owner for item {itemId} between {from} and {to}",
            itemId, startDate, endDate);

        return await _dbContext.BlockedDates
            .GetBlockedDates(itemId, startDate, endDate)
            .ToListAsync();
    }

    public async Task<ReservationRequestResult> AddReservationRequest(Guid itemId, Guid borrowerId, ReservationRequestInput reservationRequestInput)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(i => i.ItemId.Equals(itemId))
            ?? throw new DoesNotExistException(itemId.ToString());

        var startDate = reservationRequestInput.StartDate;
        var endDate = reservationRequestInput.EndDate;

        var reserved = await GetReservedDates(itemId, startDate, endDate);
        if (reserved.Any())
        {
            throw new DateConflictException(startDate, endDate);
        }

        var reservationId = Guid.NewGuid();
        var entry = reservationRequestInput.ToTableEntry(reservationId,
                                                         itemId,
                                                         item.OwnerId,
                                                         borrowerId);
        await _dbContext.Reservations.AddAsync(entry);
        await _dbContext.SaveChangesAsync();

        return new ReservationRequestResult
        {
            ReservationId = reservationId,
        };
    }

    public async Task<List<ReservationDetails>> GetPendingReservations(Guid itemId, DateTime from, DateTime to)
    {
        var reservationDatesQ = _dbContext.Reservations
            .GetPendingResevations(itemId, from, to)
            .Select(r => r.ToReservationDetails());

        return await reservationDatesQ.ToListAsync();
    }

    public async Task BlockDates(Guid itemId, IEnumerable<DateTime> dates)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(i => i.ItemId.Equals(itemId))
            ?? throw new DoesNotExistException(itemId.ToString());

        dates = dates.Order();
        var startDate = dates.First();
        var endDate = dates.Last();

        var existingReservationsQ = _dbContext.Reservations.GetActiveResevations(itemId, startDate, endDate);

        if (existingReservationsQ.Any())
        {
            throw new DateConflictException(startDate, endDate);
        }

        var alreadyBlocked = _dbContext.BlockedDates.GetBlockedDates(itemId, startDate, endDate);
        var unblocked = dates.Except(alreadyBlocked).Select(d => d.ToTableEntry(itemId));

        await _dbContext.BlockedDates.AddRangeAsync(unblocked);
        await _dbContext.SaveChangesAsync();
    }
}
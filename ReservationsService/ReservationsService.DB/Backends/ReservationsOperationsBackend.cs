using Boro.Common.Exceptions;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Exceptions;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Output;
using ReservationsService.DB.Extensions;
using ReservationsService.DB.Services;

namespace ReservationsService.DB.Backends;

public class ReservationsOperationsBackend : IReservationsOperationsBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;
    private readonly ReservationsEmailNotifier _notifier;

    public ReservationsOperationsBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext,
        ReservationsEmailNotifier notifier)
    {
        _logger = loggerFactory.CreateLogger("ReservationsService");
        _dbContext = dbContext;
        _notifier = notifier;
    }

    private async Task<Reservations> FindReservationAsync(Guid reservationId)
    {
        var entry = await _dbContext.Reservations.FindAsync(reservationId) 
            ?? throw new DoesNotExistException(reservationId.ToString());
        return entry;
    }

    public async Task ApproveAsync(Guid reservationId)
    {
        var entry = await FindReservationAsync(reservationId);
        if (entry.Status != ReservationStatus.Pending)
        {
            throw new IllegalReservationOperationException(reservationId, entry.Status, ReservationStatus.Approved);
        }
        entry.Status = ReservationStatus.Approved; 
        await _dbContext.SaveChangesAsync();
        await _notifier.NotifyApproval(reservationId);
    }

    public async Task CancelAsync(Guid reservationId)
    {
        var entry = await FindReservationAsync(reservationId);
        if (!entry.Status.IsActiveStatus())
        {
            throw new IllegalReservationOperationException(reservationId, entry.Status, ReservationStatus.Canceled);
        }
        entry.Status = ReservationStatus.Canceled;
        await _dbContext.SaveChangesAsync();
        await _notifier.NotifyCancellation(reservationId);
    }

    public async Task DeclineAsync(Guid reservationId)
    {
        var entry = await FindReservationAsync(reservationId);
        if (entry.Status != ReservationStatus.Pending)
        {
            throw new IllegalReservationOperationException(reservationId, entry.Status, ReservationStatus.Declined);
        }
        entry.Status = ReservationStatus.Declined;
        await _dbContext.SaveChangesAsync();
        await _notifier.NotifyDecline(reservationId);
    }

    public async Task HandOverToBorrowerAsync(Guid reservationId)
    {
        var entry = await FindReservationAsync(reservationId);
        if (entry.Status != ReservationStatus.Approved)
        {
            throw new IllegalReservationOperationException(reservationId, entry.Status, ReservationStatus.Borrowed);
        }
        entry.Status = ReservationStatus.Borrowed;
        await _dbContext.SaveChangesAsync();
        await _notifier.NotifyHandoverToBorrower(reservationId);
    }

    public async Task ReturnToLenderAsync(Guid reservationId)
    {
        var entry = await FindReservationAsync(reservationId);
        if (entry.Status != ReservationStatus.Borrowed)
        {
            throw new IllegalReservationOperationException(reservationId, entry.Status, ReservationStatus.Returned);
        }
        entry.Status = ReservationStatus.Returned;
        await _dbContext.SaveChangesAsync();
        await _notifier.NotifyReturnToLender(reservationId);
    }

    public async Task<ReservationDetails> GetReservationDetailsAsync(Guid reservationId)
    {
        var entry = await FindReservationAsync(reservationId);
        return entry.ToReservationDetails();
    }
}

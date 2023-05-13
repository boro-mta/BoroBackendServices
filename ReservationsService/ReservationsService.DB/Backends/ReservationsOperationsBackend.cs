using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;

namespace ReservationsService.DB.Backends;

public class ReservationsOperationsBackend :IReservationsOperationsBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;

    public ReservationsOperationsBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("ReservationsService");
        _dbContext = dbContext;
    }

    public Task Approve(string reservationId)
    {
        throw new NotImplementedException();
    }

    public Task Cancel(string reservationId)
    {
        throw new NotImplementedException();
    }

    public Task Decline(string reservationId)
    {
        throw new NotImplementedException();
    }

    public Task HandOverToBorrower(string reservationId)
    {
        throw new NotImplementedException();
    }

    public Task ReturnToLender(string reservationId)
    {
        throw new NotImplementedException();
    }
}

using Boro.Common.Authentication;
using Boro.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;

namespace ReservationsService.Controller.Controllers;

[Route("Reservations/{reservationId}")]
[ApiController]
[ValidatesGuid("reservationId")]
[Authorize]
public class ReservationOperationsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IReservationsOperationsBackend _backend;

    public ReservationOperationsController(ILoggerFactory loggerFactory,
        IReservationsOperationsBackend backend)
    {
        _logger = loggerFactory.CreateLogger("ReservationsService");
        _backend = backend;
    }

    [HttpPost("Approve")]
    [Authorize(Policy = AuthPolicies.ReservationLender)]
    public Task Approve(string reservationId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("Cancel")]
    [Authorize(Policy = AuthPolicies.ReservationLenderOrBorrower)]
    public Task Cancel(string reservationId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("Decline")]
    [Authorize(Policy = AuthPolicies.ReservationLender)]
    public ActionResult Decline(string reservationId)
    {
        return null;
    }

    [HttpPost("HandOverToBorrower")]
    [Authorize(Policy = AuthPolicies.ReservationLender)]
    public Task HandOverToBorrower(string reservationId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("ReturnToLender")]
    [Authorize(Policy = AuthPolicies.ReservationBorrower)]
    public Task ReturnToLender(string reservationId)
    {
        throw new NotImplementedException();
    }
}

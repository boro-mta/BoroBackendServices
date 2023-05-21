using Boro.Common.Authentication;
using Boro.Common.Exceptions;
using Boro.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Exceptions;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Output;

namespace ReservationsService.Controller.Controllers;

[Route("Reservations/{reservationId}")]
[ApiController]
[ValidatesGuid("reservationId")]
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
    public ActionResult Approve(string reservationId)
    {
        try
        {
            var guid = Guid.Parse(reservationId);
            _backend.ApproveAsync(guid).Wait();
            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"reservation [{reservationId}] could not be found");
        }
        catch (IllegalReservationOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost("Cancel")]
    [Authorize(Policy = AuthPolicies.ReservationLenderOrBorrower)]
    public ActionResult Cancel(string reservationId)
    {
        try
        {
            var guid = Guid.Parse(reservationId);
            _backend.CancelAsync(guid).Wait();
            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"reservation [{reservationId}] could not be found");
        }
        catch (IllegalReservationOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost("Decline")]
    [Authorize(Policy = AuthPolicies.ReservationLender)]
    public ActionResult Decline(string reservationId)
    {
        try
        {
            var guid = Guid.Parse(reservationId);
            _backend.DeclineAsync(guid).Wait();
            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"reservation [{reservationId}] could not be found");
        }
        catch (IllegalReservationOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost("HandOverToBorrower")]
    [Authorize(Policy = AuthPolicies.ReservationLender)]
    public ActionResult HandOverToBorrower(string reservationId)
    {
        try
        {
            var guid = Guid.Parse(reservationId);
            _backend.HandOverToBorrowerAsync(guid).Wait();
            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"reservation [{reservationId}] could not be found");
        }
        catch (IllegalReservationOperationException e) 
        { 
            return Conflict(e.Message);
        }
    }

    [HttpPost("ReturnToLender")]
    [Authorize(Policy = AuthPolicies.ReservationBorrower)]
    public ActionResult ReturnToLender(string reservationId)
    {
        try
        {
            var guid = Guid.Parse(reservationId);
            _backend.ReturnToLenderAsync(guid).Wait();
            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"reservation [{reservationId}] could not be found");
        }
        catch (IllegalReservationOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpGet()]
    [Authorize(Policy = AuthPolicies.ReservationLenderOrBorrower)]
    public ActionResult<ReservationDetails> GetReservationDetails(string reservationId)
    {
        try
        {
            var guid = Guid.Parse(reservationId);
            var reservation = _backend.GetReservationDetailsAsync(guid).Result;
            return Ok(reservation);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"reservation [{reservationId}] could not be found");
        }
    }
}

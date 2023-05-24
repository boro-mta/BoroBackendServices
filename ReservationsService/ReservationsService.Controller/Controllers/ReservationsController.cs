using Boro.Common.Authentication;
using Boro.Common.Exceptions;
using Boro.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Input;
using ReservationsService.API.Models.Output;
using System.ComponentModel.DataAnnotations;

namespace ReservationsService.Controller.Controllers;

[Route("[controller]/{itemId}")]
[ApiController]
[ValidatesGuid("itemId")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IReservationsServiceBackend _backend;

    public ReservationsController(ILoggerFactory loggerFactory,
        IReservationsServiceBackend backend)
    {
        _logger = loggerFactory.CreateLogger("ReservationsService");
        _backend = backend;
    }

    [HttpGet("BlockedDates")]
    public async Task<ActionResult<List<DateTime>>> GetBlockedDates(string itemId, DateTime from, DateTime to)
    {
        _logger.LogInformation("GetBlockedDates was called with id: [{id}], from: [{from}], to: [{to}]", 
            itemId, from, to);

        if (from > to)
        {
            return BadRequest("the 'from' date is later than the 'to' date");
        }
        var guid = Guid.Parse(itemId);
        var dates = await _backend.GetBlockedDates(guid, from, to);

        _logger.LogInformation("GetBlockedDates - Finished with: [{@dates}]", dates);

        return Ok(dates);
    }

    [HttpGet("Pending")]
    [Authorize(Policy = AuthPolicies.ItemOwner)]
    public async Task<ActionResult<List<ReservedDates>>> GetPendingReservations(string itemId, DateTime from, DateTime to)
    {
        _logger.LogInformation("GetPendingReservations was called with item id: [{id}], from: [{from}], to: [{to}]",
            itemId, from, to);

        if (from > to)
        {
            return BadRequest("the 'from' date is later than the 'to' date");
        }
        var guid = Guid.Parse(itemId);
        List<ReservationDetails> reservations = await _backend.GetPendingReservations(guid, from, to);

        _logger.LogInformation("GetPendingReservations - There are [{count}] pending reservations for [{@itemId}]",
            reservations.Count, guid);

        return reservations.Any() ? Ok(reservations) : NotFound($"No pending reservations were found for item: [{itemId}]");
    }

    [HttpPost("Request")]
    [Authorize(Policy = AuthPolicies.NotItemOwner)]
    public async Task<ActionResult<ReservationRequestResult>> RequestReservation(string itemId, [FromBody] ReservationRequestInput reservationRequestInput)
    {
        try
        {
            var borrowerId = User.UserId();
            var guid = Guid.Parse(itemId);
            _logger.LogInformation("RequestReservation was called with id: [{id}], [{@dates}]",
                                itemId, reservationRequestInput);

            var result = await _backend.AddReservationRequest(guid, borrowerId, reservationRequestInput);

            _logger.LogInformation("RequestReservation - Finished with {@result}", result);

            return Ok(result);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id {itemId} was not found");
        }
        catch (DateConflictException)
        {
            return Conflict($"item with id {itemId} has conflicting reservations between {reservationRequestInput.StartDate} and {reservationRequestInput.EndDate}");
        }
    }

    [HttpPost("BlockDates")]
    [Authorize(Policy = AuthPolicies.ItemOwner)]
    public async Task<ActionResult> BlockDates(string itemId, [FromBody][MinLength(1)] List<DateTime> dates)
    {
        try
        {
            _logger.LogInformation("BlockDates was called with id: [{id}], [{@dates}]",
                                itemId, dates);
            var guid = Guid.Parse(itemId);

            await _backend.BlockDates(guid, dates);

            _logger.LogInformation("BlockDates - Finished");

            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id {itemId} was not found");
        }
        catch (DateConflictException)
        {
            return Conflict("can't block dates where an existing reservation exists");
        }
    }

    [HttpPost("UnblockDates")]
    [Authorize(Policy = AuthPolicies.ItemOwner)]
    public async Task<ActionResult> UnblockDates(string itemId, [FromBody][MinLength(1)] List<DateTime> dates)
    {
        try
        {
            _logger.LogInformation("UnblockDates was called with id: [{id}], [{@dates}]",
                                itemId, dates);
            var guid = Guid.Parse(itemId);

            await _backend.UnblockDates(guid, dates);

            _logger.LogInformation("UnblockDates - Finished");

            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id {itemId} was not found");
        }
    }
}
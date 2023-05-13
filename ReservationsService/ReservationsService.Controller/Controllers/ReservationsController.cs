﻿using Boro.Common.Authentication;
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

    [HttpGet("Dates")]
    public ActionResult<List<DateTime>> GetReservedDates(string itemId, DateTime from, DateTime to)
    {
        _logger.LogInformation("GetReservedDates was called with id: [{id}], from: [{from}], to: [{to}]", 
            itemId, from, to);

        if (from > to)
        {
            return BadRequest("the 'from' date is later than the 'to' date");
        }
        var guid = Guid.Parse(itemId);
        var dates = _backend.GetReservedDates(guid, from, to).Result;

        _logger.LogInformation("GetReservedDates - Finished with: [{@dates}]", dates);

        return dates.Any() ? Ok(dates) : NotFound($"No reservations were found for item: [{itemId}]");
    }

    [HttpGet("Pending")]
    public ActionResult<List<ReservedDates>> GetPendingReservations(string itemId, DateTime from, DateTime to)
    {
        _logger.LogInformation("GetPendingReservations was called with item id: [{id}], from: [{from}], to: [{to}]",
            itemId, from, to);

        if (from > to)
        {
            return BadRequest("the 'from' date is later than the 'to' date");
        }
        var guid = Guid.Parse(itemId);
        List<ReservationDetails> reservations = _backend.GetPendingReservations(guid, from, to).Result;

        _logger.LogInformation("GetPendingReservations - There are [{count}] pending reservations for [{@itemId}]",
            reservations.Count, guid);

        return reservations.Any() ? Ok(reservations) : NotFound($"No pending reservations were found for item: [{itemId}]");
    }

    [HttpPost("Request")]
    [Authorize(Policy = AuthPolicies.NotItemOwner)]
    public ActionResult<ReservationRequestResult> RequestReservation(string itemId, [FromBody] ReservationRequestInput reservationRequestInput)
    {
        try
        {
            var borrowerId = User.UserId();
            var guid = Guid.Parse(itemId);
            _logger.LogInformation("RequestReservation was called with id: [{id}], [{@dates}]",
                                itemId, reservationRequestInput);

            var result = _backend.AddReservationRequest(guid,
                                                        borrowerId,
                                                        reservationRequestInput).Result;

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
    public ActionResult<ReservationRequestResult> BlockDates(string itemId, [MinLength(1)] List<DateTime> dates)
    {
        try
        {
            _logger.LogInformation("BlockDates was called with id: [{id}], [{@dates}]",
                                itemId, dates);
            var guid = Guid.Parse(itemId);

            _backend.BlockDates(guid, dates).Wait();

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
}
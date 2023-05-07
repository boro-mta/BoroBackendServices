using Boro.Common.Exceptions;
using Boro.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Input;
using ReservationsService.API.Models.Output;

namespace ReservationsService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
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

    [HttpGet("{itemId}/Dates")]
    [ValidatesGuid("itemId")]
    public ActionResult<List<ReservedDates>> GetReservedDates(string itemId, DateTime from, DateTime to)
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

    [HttpGet("{itemId}/Pending")]
    [ValidatesGuid("itemId")]
    public ActionResult<List<ReservedDates>> GetPendingReservations(string itemId)
    {
        _logger.LogInformation("GetPendingReservations was called with item id: [{id}]",
            itemId);

        var guid = Guid.Parse(itemId);
        List<ReservationDetails> reservations = _backend.GetPendingReservations(guid).Result;

        _logger.LogInformation("GetPendingReservations - There are [{count}] pending reservations for [{@itemId}]",
            reservations.Count, guid);

        return reservations.Any() ? Ok(reservations) : NotFound($"No pending reservations were found for item: [{itemId}]");
    }

    [HttpPost("{itemId}/Request")]
    [ValidatesGuid("itemId")]
    [ValidatesGuid("reservationRequestInput.BorrowerId")]
    public ActionResult<ReservationRequestResult> RequestReservation(string itemId, [FromBody] ReservationRequestInput reservationRequestInput)
    {
        try
        {
            var guid = Guid.Parse(itemId);
            _logger.LogInformation("RequestReservation was called with id: [{id}], [{@dates}]",
                                itemId, reservationRequestInput);

            var result = _backend.AddReservationRequest(guid, reservationRequestInput).Result;

            _logger.LogInformation("RequestReservation - Finished with {@result}", result);

            return result switch
            {
                ReservationRequestResult.DateConflict => Conflict(result),
                ReservationRequestResult.RequestCreated => Ok(result),
                _ => Conflict(result)
            };
        }
        catch (Exception)
        {
            return NotFound($"item with id {itemId} was not found");
        }
    }

    [HttpPost("{itemId}/Block")]
    [ValidatesGuid("itemId")]
    public ActionResult<ReservationRequestResult> BlockDates(string itemId, DateTime startDate, DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
            {
                return BadRequest("start date is later than end date");
            }

            _logger.LogInformation("BlockDates was called with id: [{id}], [{startDate} - {endDate}]",
                                itemId, startDate, endDate);
            var guid = Guid.Parse(itemId);

            _backend.BlockDates(guid, startDate, endDate).Wait();

            _logger.LogInformation("BlockDates - Finished");

            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"item with id {itemId} was not found");
        }
        catch (DateConflictException)
        {
            return Conflict("dates are conflicted with existing reservations");
        }
    }
}
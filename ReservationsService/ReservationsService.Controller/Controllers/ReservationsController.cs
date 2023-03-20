using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Output;
using System.ComponentModel.DataAnnotations;

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

    [HttpGet("ReservedDates/{itemId}")]
    public ActionResult<List<ReservationDates>> GetReservedDates(string itemId, DateTime from, DateTime to)
    {
        _logger.LogInformation("GetReservedDates was called with id: [{id}], from: [{from}], to: [{to}]", 
            itemId, from, to);

        if (from > to)
        {
            return BadRequest("the 'from' date is later than the 'to' date");
        }
        else if(!Guid.TryParse(itemId, out var id))
        {
            return BadRequest("itemId is invalid");
        }
        else
        {
            var dates = _backend.GetReservedDates(id, from, to);

            _logger.LogInformation("GetReservedDates - Finished with: [{@dates}]", dates);

            return dates.Any() ? Ok(dates) : NotFound($"No reservations were found for item: [{itemId}]");
        }
    }

    [HttpPost("Reserve/{itemId}")]
    public ActionResult AddReservations(string itemId, [FromBody][MinLength(1)] IEnumerable<ReservationDates> reservationDates)
    {
        try
        {
            if (Guid.TryParse(itemId, out var id))
            {
                _logger.LogInformation("AddReservations was called with id: [{id}], [{@dates}]",
                                itemId, reservationDates);

                _backend.AddReservations(id, reservationDates);

                _logger.LogInformation("AddReservations - Finished");

                return Ok(); 
            }
            else
            {
                return BadRequest("itemId is invalid");
            }
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
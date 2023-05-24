using Boro.Common.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Interfaces;
using ReservationsService.API.Models.Output;

namespace ReservationsService.Controller.Controllers;

[Route("Reservations/Dashboard")]
[ApiController]
[Authorize]
public class ReservationsDashboardController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IReservationsDashboardBackend _backend;

    public ReservationsDashboardController(ILoggerFactory loggerFactory,
        IReservationsDashboardBackend backend)
    {
        _logger = loggerFactory.CreateLogger("ReservationsService");
        _backend = backend;
    }

    [HttpGet("Borrower")]
    public async Task<ActionResult<List<ReservationDetails>>> GetBorrowersReservationsDashboard(DateTime from, DateTime to)
    {
        var borrower = User.UserId();
        var reservations = await _backend.GetBorrowersDashboard(borrower, from, to);
        return Ok(reservations);
    }

    [HttpGet("Borrower/Upcoming")]
    public async Task<ActionResult<List<ReservationDetails>>> GetBorrowersUpcoming()
    {
        var borrower = User.UserId();
        var reservations = await _backend.GetBorrowersUpcoming(borrower);
        return Ok(reservations);
    }

    [HttpGet("Lender")]
    public async Task<ActionResult<List<ReservationDetails>>> GetLendersReservationDashboard(DateTime from, DateTime to)
    {
        var lender = User.UserId();
        var reservations = await _backend.GetLendersDashboard(lender, from, to);
        return Ok(reservations);
    }

    [HttpGet("Lender/Upcoming")]
    public async Task<ActionResult<List<ReservationDetails>>> GetLendersUpcoming()
    {
        var lender = User.UserId();
        var reservations = await _backend.GetLendersUpcoming(lender);
        return Ok(reservations);
    }
}

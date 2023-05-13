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
    public ActionResult<List<ReservationDetails>> GetBorrowersReservationsDashboard(DateTime from, DateTime to)
    {
        var borrower = User.UserId();
        var reservations = _backend.GetBorrowersDashboard(borrower, from, to).Result;
        return Ok(reservations);
    }

    [HttpGet("Borrower/Upcoming")]
    public ActionResult<List<ReservationDetails>> GetBorrowersUpcoming(DateTime from, DateTime to)
    {
        var borrower = User.UserId();
        var reservations = _backend.GetBorrowersUpcoming(borrower).Result;
        return Ok(reservations);
    }

    [HttpGet("Lender")]
    public ActionResult<List<ReservationDetails>> GetLendersReservationDashboard(DateTime from, DateTime to)
    {
        var lender = User.UserId();
        var reservations = _backend.GetLendersDashboard(lender, from, to).Result;
        return Ok(reservations);
    }

    [HttpGet("Lender/Upcoming")]
    public ActionResult<List<ReservationDetails>> GetLendersUpcoming(DateTime from, DateTime to)
    {
        var lender = User.UserId();
        var reservations = _backend.GetLendersUpcoming(lender).Result;
        return Ok(reservations);
    }
}

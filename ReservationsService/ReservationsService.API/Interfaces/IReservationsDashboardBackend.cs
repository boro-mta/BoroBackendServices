using ReservationsService.API.Models.Output;

namespace ReservationsService.API.Interfaces;

public interface IReservationsDashboardBackend
{
    Task<List<ReservationDetails>> GetBorrowersUpcoming(Guid borrowerId);
    Task<List<ReservationDetails>> GetLendersUpcoming(Guid lenderId);

    Task<List<ReservationDetails>> GetBorrowersDashboard(Guid borrowerId, DateTime from, DateTime to);
    Task<List<ReservationDetails>> GetLendersDashboard(Guid lenderId, DateTime from, DateTime to);
}
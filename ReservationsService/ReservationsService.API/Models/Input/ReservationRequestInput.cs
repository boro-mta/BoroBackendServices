namespace ReservationsService.API.Models.Input;

public class ReservationRequestInput
{
    public string BorrowerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

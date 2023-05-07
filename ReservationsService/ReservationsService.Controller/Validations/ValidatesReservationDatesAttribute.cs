using Boro.Validations;

namespace ReservationsService.Controller.Validations;

internal class ValidatesReservationDatesAttribute : ValidatesAttribute
{
    public ValidatesReservationDatesAttribute(string parameterName) : base(parameterName)
    {
    }

    public override (bool valid, IEnumerable<string> errors) Validate(object? parameter)
    {
        throw new NotImplementedException();
    }
}

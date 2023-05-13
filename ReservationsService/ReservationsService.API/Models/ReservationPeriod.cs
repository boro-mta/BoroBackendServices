using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationsService.API.Models
{
    public record ReservationPeriod(DateTime StartDate, DateTime EndDate);
}

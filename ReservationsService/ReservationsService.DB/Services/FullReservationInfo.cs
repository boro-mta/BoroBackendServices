using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationsService.DB.Services
{
    internal class FullReservationInfo
    {
        public Reservations Reservation { get; set; }
        public Users BorrowerInfo { get; set; }
        public Users LenderInfo { get; set; }
        public Items ItemInfo { get; set; }
    }
}

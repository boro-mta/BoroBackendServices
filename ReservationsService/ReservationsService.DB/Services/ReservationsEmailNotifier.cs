using Boro.Email.API;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationsService.API.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationsService.DB.Services
{
    public class ReservationsEmailNotifier
    {
        private readonly ILogger _logger;
        private readonly BoroMainDbContext _dbContext;
        private readonly IEmailService _emailService;

        public ReservationsEmailNotifier(ILoggerFactory loggerFactory,
            BoroMainDbContext dbContext,
            IEmailService emailService)
        {
            _logger = loggerFactory.CreateLogger("ReservationsService");
            _dbContext = dbContext;
            _emailService = emailService;
        }
        private async Task<FullReservationInfo> GetReservationInfoAsync(Guid reservationId)
        {
            var reservation = await _dbContext.Reservations.FirstAsync(r => r.ReservationId.Equals(reservationId));
            var lenderInfo = await _dbContext.Users.FirstAsync(u => u.UserId.Equals(reservation.LenderId));
            var borrowerInfo = await _dbContext.Users.FirstAsync(u => u.UserId.Equals(reservation.BorrowerId));
            var itemInfo = await _dbContext.Items.FirstAsync(i => i.ItemId.Equals(reservation.ItemId));
            return new FullReservationInfo
            {
                Reservation = reservation,
                LenderInfo = lenderInfo,
                BorrowerInfo = borrowerInfo,
                ItemInfo = itemInfo
            };
        }

        public async Task NewReservationRequest(Guid reservationId)
        {
            var info = await GetReservationInfoAsync(reservationId);

            var itemInfo = info.ItemInfo;
            var borrowerInfo = info.BorrowerInfo;
            var lenderInfo = info.LenderInfo;

            var itemTitle = itemInfo.Title;
            var borrowerName = $"{borrowerInfo.FirstName} {borrowerInfo.LastName}";
            var lenderName = $"{lenderInfo.FirstName} {lenderInfo.LastName}";
            var startDate = info.Reservation.StartDate;
            var endDate = info.Reservation.EndDate;

            if (lenderInfo?.Email is not null)
            {
                var lenderEmailTitle = $"A new request to borrow '{itemTitle}' from you has been received";
                var lenderHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>{{borrowerName}} wants to borrow {{itemTitle}} from you!</strong></h1>

                    <h2 style="text-align:center"><em>To approve or decline their request, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">{{borrowerName}} has requested to borrow {{itemTitle}} from you starting from {{startDate}} and until {{endDate}}</span><br />
                    <span style="background-color:#2ecc71">You can go in the app to see more information about {{borrowerName}}, approve/decline, and maybe send them a message in our chat.</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { lenderInfo.Email }, lenderEmailTitle, lenderHtmlMessage);
            }

            if (borrowerInfo?.Email is not null)
            {
                var borrowerEmailTitle = $"Your request to borrow '{itemTitle}' has been created";
                var borrowerHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>your request to borrow {{itemTitle}} from {{lenderName}} has been created!</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">your request to borrow {{itemTitle}} from {{lenderName}} starting from {{startDate}} and until {{endDate}} has been created</span><br />
                    <span style="background-color:#2ecc71">It is now pending the owner&#39;s approval. You will be notified for any updates.</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { borrowerInfo.Email }, borrowerEmailTitle, borrowerHtmlMessage);
            }
        }

        public async Task NotifyApproval(Guid reservationId)
        {
            var info = await GetReservationInfoAsync(reservationId);

            var itemInfo = info.ItemInfo;
            var borrowerInfo = info.BorrowerInfo;
            var lenderInfo = info.LenderInfo;

            var itemTitle = itemInfo.Title;
            var borrowerName = $"{borrowerInfo.FirstName} {borrowerInfo.LastName}";
            var lenderName = $"{lenderInfo.FirstName} {lenderInfo.LastName}";
            var startDate = info.Reservation.StartDate;
            var endDate = info.Reservation.EndDate;

            if (lenderInfo?.Email is not null)
            {
                var lenderEmailTitle = $"You approved a request to borrow '{itemTitle}'";
                var lenderHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>you approved {{borrowerName}}&#39;s request to borrow {{itemTitle}}</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">you approved {{borrowerName}}&#39;s request to borrow {{itemTitle}} starting from {{startDate}} and until {{endDate}}</span><br />
                    <span style="background-color:#2ecc71">Once they update that the item has been handed over to them, you will be updated.<br />
                    To cancel, log in to boro.</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { lenderInfo.Email }, lenderEmailTitle, lenderHtmlMessage);
            }

            if (borrowerInfo?.Email is not null)
            {
                var borrowerEmailTitle = $"Your request to borrow '{itemTitle}' has been approved!";
                var borrowerHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>your request to borrow {{itemTitle}} from {{lenderName}} has been approved</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">{{lenderName}} has approved your request to borrow {{itemTitle}}, starting from {{startDate}} and until {{endDate}}</span><br />
                    <span style="background-color:#2ecc71">When you receive the item, update it in the app.<br />
                    To cancel, log in to boro.</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { borrowerInfo.Email }, borrowerEmailTitle, borrowerHtmlMessage);
            }
        }

        public async Task NotifyDecline(Guid reservationId)
        {
            var info = await GetReservationInfoAsync(reservationId);

            var itemInfo = info.ItemInfo;
            var borrowerInfo = info.BorrowerInfo;
            var lenderInfo = info.LenderInfo;

            var itemTitle = itemInfo.Title;
            var borrowerName = $"{borrowerInfo.FirstName} {borrowerInfo.LastName}";
            var lenderName = $"{lenderInfo.FirstName} {lenderInfo.LastName}";
            var startDate = info.Reservation.StartDate;
            var endDate = info.Reservation.EndDate;

            if (lenderInfo?.Email is not null)
            {
                var lenderEmailTitle = $"You declined a request to borrow '{itemTitle}'";
                var lenderHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>you have declined {{borrowerName}}&#39;s request to borrow {{itemTitle}}</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">You have declined {{borrowerName}}&#39;s request to borrow {{itemTitle}}, starting from {{startDate}} and until {{endDate}}</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { lenderInfo.Email }, lenderEmailTitle, lenderHtmlMessage);
            }

            if (borrowerInfo?.Email is not null)
            {
                var borrowerEmailTitle = $"Your request to borrow '{itemTitle}' has been declined";
                var borrowerHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>your request to borrow {{itemTitle}} has been declined</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">{{lenderName}} declined your request to borrow {{itemTitle}}, starting from {{startDate}} and until {{endDate}}</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { borrowerInfo.Email }, borrowerEmailTitle, borrowerHtmlMessage);
            }
        }

        public async Task NotifyCancellation(Guid reservationId)
        {
            var info = await GetReservationInfoAsync(reservationId);

            var itemInfo = info.ItemInfo;
            var borrowerInfo = info.BorrowerInfo;
            var lenderInfo = info.LenderInfo;

            var itemTitle = itemInfo.Title;
            var borrowerName = $"{borrowerInfo.FirstName} {borrowerInfo.LastName}";
            var lenderName = $"{lenderInfo.FirstName} {lenderInfo.LastName}";
            var startDate = info.Reservation.StartDate;
            var endDate = info.Reservation.EndDate;

            if (lenderInfo?.Email is not null)
            {
                var lenderEmailTitle = $"A request to borrow '{itemTitle}' was cancelled";
                var lenderHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>a request to borrow {{itemTitle}} has been cancelled</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">a request to borrow {{itemTitle}} by {{borrowerName}}, starting from {{startDate}} and until {{endDate}}, has been cancelled</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { lenderInfo.Email }, lenderEmailTitle, lenderHtmlMessage);
            }

            if (borrowerInfo?.Email is not null)
            {
                var borrowerEmailTitle = $"Your request to borrow '{itemTitle}' was cancelled";
                var borrowerHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>your request to borrow {{itemTitle}} has been cancelled</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">your request to borrow {{itemTitle}}, starting from {{startDate}} and until {{endDate}}, has been cancelled</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { borrowerInfo.Email }, borrowerEmailTitle, borrowerHtmlMessage);
            }
        }

        public async Task NotifyHandoverToBorrower(Guid reservationId)
        {
            var info = await GetReservationInfoAsync(reservationId);

            var itemInfo = info.ItemInfo;
            var borrowerInfo = info.BorrowerInfo;
            var lenderInfo = info.LenderInfo;

            var itemTitle = itemInfo.Title;
            var borrowerName = $"{borrowerInfo.FirstName} {borrowerInfo.LastName}";
            var lenderName = $"{lenderInfo.FirstName} {lenderInfo.LastName}";
            var startDate = info.Reservation.StartDate;
            var endDate = info.Reservation.EndDate;

            if (lenderInfo?.Email is not null)
            {
                var lenderEmailTitle = $"Your item has been picked up by borrower";
                var lenderHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>{{itemTitle}} has been handed over to {{borrowerName}}</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">your item: {{itemTitle}} has been picked up by {{borrowerName}}, and will be with them starting from {{startDate}} and until {{endDate}}</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { lenderInfo.Email }, lenderEmailTitle, lenderHtmlMessage);
            }

            if (borrowerInfo?.Email is not null)
            {
                var borrowerEmailTitle = $"you have picked up '{itemTitle}' from {lenderName}";
                var borrowerHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>You have picked up your reservation of {{itemTitle}} from {{lenderName}}</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">you have picked up {{itemTitle}} from {{lenderName}}. The item will be with you starting from {{startDate}} and until {{endDate}}</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { borrowerInfo.Email }, borrowerEmailTitle, borrowerHtmlMessage);
            }
        }

        public async Task NotifyReturnToLender(Guid reservationId)
        {
            var info = await GetReservationInfoAsync(reservationId);

            var itemInfo = info.ItemInfo;
            var borrowerInfo = info.BorrowerInfo;
            var lenderInfo = info.LenderInfo;

            var itemTitle = itemInfo.Title;
            var borrowerName = $"{borrowerInfo.FirstName} {borrowerInfo.LastName}";
            var lenderName = $"{lenderInfo.FirstName} {lenderInfo.LastName}";
            var startDate = info.Reservation.StartDate;
            var endDate = info.Reservation.EndDate;

            if (lenderInfo?.Email is not null)
            {
                var lenderEmailTitle = $"You have received {itemTitle} back";
                var lenderHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>Your item {{itemTitle}} has been returned by {{borrowerName}}</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">you have received {{itemTitle}} back from {{borrowerName}}. Thank you for participating in our community</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { lenderInfo.Email }, lenderEmailTitle, lenderHtmlMessage);
            }

            if (borrowerInfo?.Email is not null)
            {
                var borrowerEmailTitle = $"You have returned {itemTitle}";
                var borrowerHtmlMessage =
                    $$"""
                    <h1 style="text-align:center"><strong>You have returned your reservation of {{itemTitle}} to {{lenderName}}</strong></h1>

                    <h2 style="text-align:center"><em>To make changes, log in to boro.</em></h2>

                    <p style="text-align:center"><strong><span style="background-color:#2ecc71">you have return {{itemTitle}} back to {{lenderName}}. Thank you for participating in our community</span></strong></p>

                    <p style="text-align:center"><img alt="" src="https://i.ibb.co/DGz9R82/logo-public-1.png" style="height:176px; width:220px" /></p>
                    
                    """;

                await _emailService.SendHTMLEmailAsync(new string[] { borrowerInfo.Email }, borrowerEmailTitle, borrowerHtmlMessage);
            }
        }
    }
}

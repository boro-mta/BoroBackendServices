using Boro.Authentication.Policies.Requirements;
using Boro.Common.Authentication;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Boro.Authentication.Policies.Handlers;

public class ReservationParticipantAuthorizationHandler : AuthorizationHandler<ReservationParticipantRequirement>
{
    private readonly BoroMainDbContext _dbContext;

    public ReservationParticipantAuthorizationHandler(BoroMainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReservationParticipantRequirement requirement)
    {
        var userId = context.User.UserId();
        var reservationId = context.GetFromRoute<string>(requirement.ReservationIdParameterName);
        var requireLender = requirement.Lender;
        var requireBorrower = requirement.Borrower;

        if (reservationId is not null
            && Guid.TryParse(reservationId, out var reservationIdGuid))
        {
            var isLender = await _dbContext.IsLender(reservationIdGuid, userId);
            var isBorrower = await _dbContext.IsBorrower(reservationIdGuid, userId);
            var lenderOrBorrowerRequiredAndFulfilled = requireLender && requireBorrower && (isLender || isBorrower);
            var lenderRequiredAndFulfilled = requireLender && isLender;
            var borrowerRequiredAndFulfilled = requireBorrower && isBorrower;

            if (lenderOrBorrowerRequiredAndFulfilled 
                || lenderRequiredAndFulfilled
                || borrowerRequiredAndFulfilled)
            {
                context.Succeed(requirement);
            }
        }
    }
}


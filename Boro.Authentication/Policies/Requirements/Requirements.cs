using Microsoft.AspNetCore.Authorization;

namespace Boro.Authentication.Policies.Requirements;

public record class ItemOwnerRequirement(string ItemIdParameterName = "itemId", bool Owner = true) : IAuthorizationRequirement;
public record class ImageOwnerRequirement(string ImageIdParameterName = "imageId", bool Owner = true) : IAuthorizationRequirement;
public record class ReservationParticipantRequirement(string ReservationIdParameterName = "reservationId", bool Lender = false, bool Borrower = false) : IAuthorizationRequirement;
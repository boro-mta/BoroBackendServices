namespace Boro.Common.Authentication;

public static class AuthPolicies
{
    /// <summary>
    /// Requires endpoint methods to have a parameter named <code>itemId</code> as part of their <b>route</b>
    /// </summary>
    public const string ItemOwner = "itemOwner";

    /// <summary>
    /// Requires endpoint methods to have a parameter named <code>itemId</code> as part of their <b>route</b>
    /// </summary>
    public const string NotItemOwner = "notItemOwner";

    /// <summary>
    /// Requires endpoint methods to have a parameter named <code>imageId</code> as part of their <b>route</b>
    /// </summary>
    public const string ImageOwner = "imageOwner";

    /// <summary>
    /// Requires endpoint methods to have a parameter named <code>reservationId</code> as part of their <b>route</b>
    /// </summary>
    public const string ReservationLender = "reservationLender";

    /// <summary>
    /// Requires endpoint methods to have a parameter named <code>reservationId</code> as part of their <b>route</b>
    /// </summary>
    public const string ReservationBorrower = "reservationBorrower";

    /// <summary>
    /// Requires endpoint methods to have a parameter named <code>reservationId</code> as part of their <b>route</b>
    /// </summary>
    public const string ReservationLenderOrBorrower = "reservationLenderOrBorrower";

}

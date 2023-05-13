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

}

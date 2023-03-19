namespace Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;

public static class ItemImagesExtensions
{
    public static string ToBase64String(this byte[] bytes)
        => Convert.ToBase64String(bytes);

    public static byte[] FromBase64String(this string s)
        => Convert.FromBase64String(s);
}

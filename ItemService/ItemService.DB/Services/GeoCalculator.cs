namespace ItemService.DB.Services;

public class GeoCalculator
{
    private const int _earthRadius = 6371000;

    public double Distance(double longitude1, double latitude1, double longitude2, double latitude2)
    {
        var dLat = ToRadians(latitude1 - latitude2);
        var dLon = ToRadians(longitude1 - longitude2);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(latitude1)) * Math.Cos(ToRadians(latitude2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return _earthRadius * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}

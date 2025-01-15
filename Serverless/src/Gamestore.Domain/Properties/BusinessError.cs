namespace Gamestore.Domain.Properties;

public static class BusinessError
{
    public static string CustomerDoesNotExist(string customerId)
    {
        return BusinessErrorRes.CustomerDoesNotExist.Replace("{CustomerId}", customerId,
            StringComparison.OrdinalIgnoreCase);
    }
}
namespace Client.Extensions;

public static class HttpRequestExtensions
{
    public static Guid? GetClientId(this HttpRequest request)
    {
        string? id = request.Headers["AccountId"].FirstOrDefault();

        if (id is not null && Guid.TryParse(id, out var guid))
            return guid;
        
        return null;
    }

    public static string? GetClientEmail(this HttpRequest request)
    {
        return request.Headers["Email"].FirstOrDefault();
    }
    
    public static string? GetClientName(this HttpRequest request)
    {
        return request.Headers["AccountName"].FirstOrDefault();
    }
}
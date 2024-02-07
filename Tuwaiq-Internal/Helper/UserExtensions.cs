using System.Security.Claims;

namespace Shared.Extensions;

public static class UserExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        //try
        //{
        if (user == null) throw new Exception("User");
        var s = user.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier) ?? user.Claims.FirstOrDefault(s => s.Type == "sub");
        if (s != null)
        {
            Guid.TryParse((string?)s.Value, out var result);
            return result;
        }

        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    throw e;
        //}
        return Guid.Empty;
    }

    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(s => s.Type == "name")?.Value ?? "";
    }

    public static string GetName(this ClaimsPrincipal user)
    {
        return user.FindFirst("name")?.Value +" "+ user.FindFirst("family_name")?.Value  ?? "";
    }

}
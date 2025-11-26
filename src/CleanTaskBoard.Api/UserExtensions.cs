using System.Security.Claims;

namespace CleanTaskBoard.Api;

public static class UserExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst("sub") ?? user.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim is null || !Guid.TryParse(idClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user id claim.");
        }

        return userId;
    }
}

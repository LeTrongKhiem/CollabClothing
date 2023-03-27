using System;
using System.Security.Claims;

namespace CollabClothing.BackendApi.Extensions
{
    public static class AuthExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.Sid);
            if (userIdClaim == default)
                throw new Exception("No userId claim");
            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                return userId;
            }
            throw new Exception("Not authenicated");
        }
        public static Guid GetCurrentUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return Guid.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId)
                ? userId
                : throw new Exception("Not authenticated");
        }
    }
}
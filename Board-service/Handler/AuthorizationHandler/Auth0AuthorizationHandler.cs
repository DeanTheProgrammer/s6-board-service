using CustomExceptions.ObjectExceptions;
using DTO.DTO_s;

namespace Board_service.Handler.AuthorizationHandler
{
    public static class Auth0AuthorizationHandler
    {
        public static string GetUserIdFromContext(HttpContext context)
        {
            string result = context.Request.Headers["X-User-Id"].ToString();
            return result;
        }
        public static List<string> GetUserRoleFromContext(HttpContext context)
        {
            List<string> roles = context.Request.Headers["X-Roles"].ToString().Split(',').ToList();
            return roles;
        }

        public static string GetAuth0IdFromContext(HttpContext context)
        {
            string result = context.Request.Headers["X-Auth0-Id"].ToString();
            return result;
        }

        public static AuthorizationUserDTO GetAllInformationFromContext(HttpContext context)
        {
            string UserId = context.Request.Headers["X-User-Id"].ToString();
            List<string> roles = context.Request.Headers["X-Roles"].ToString().Split(',').ToList();
            string result = context.Request.Headers["X-Auth0-Id"].ToString();
            AuthorizationUserDTO user = new AuthorizationUserDTO()
            {
                UserId = UserId,
                Roles = roles,
                Auth0Id = result
            };
            return user;
        }

        public static bool IsDeleted(HttpContext context)
        {
            List<string> roles = GetUserRoleFromContext(context);
            if (roles != null)
            {
                bool result = roles.Contains("Deleted");
                return result;
            }
            else
            {
                throw new NotFoundException("Roles not found");
            }
        }
        public static bool IsAdmin(HttpContext context)
        {
            List<string> roles = GetUserRoleFromContext(context);
            if (roles != null)
            {
                bool result = roles.Contains("Admin");
                return result;
            }
            else
            {
                throw new NotFoundException("Roles not found");
            }
        }

        public static bool IsSuperAdmin(HttpContext context)
        {
            List<string> roles = GetUserRoleFromContext(context);
            if (roles != null)
            {
                bool result = roles.Contains("SuperAdmin");
                return result;
            }
            else
            {
                throw new NotFoundException("Roles not found");
            }
        }

        public static bool IsUser(HttpContext context)
        {
            List<string> roles = GetUserRoleFromContext(context);
            if (roles != null)
            {
                bool result = roles.Contains("User");
                return result;
            }
            else
            {
                throw new NotFoundException("Roles not found");
            }
        }
    }
}

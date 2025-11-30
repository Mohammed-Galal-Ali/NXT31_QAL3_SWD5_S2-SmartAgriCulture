using Base.DAL.Models.BaseModels;
using Base.Repo.Interfaces;
using RepositoryProject.Specifications;

namespace Base.API.MiddleWare
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var accessToken = authHeader.Substring("Bearer ".Length).Trim();

                var repo = unitOfWork.Repository<BlacklistedToken>();
                var spec = new BaseSpecification<BlacklistedToken>(t => t.Token == accessToken);
                var isBlacklisted = await repo.ListAsync(spec);

                if (isBlacklisted.Count() > 0)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Access token has been revoked");
                    return;
                }
            }

            await _next(context);
        }
    }

}

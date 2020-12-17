using IdentityServerTemplate.Shared.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServerTemplate.Core.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        public Guid Id { get; }

        public string UserName { get; }

        public IEnumerable<string> Roles { get; }

        private readonly IHttpContextAccessor _accessor;
        
        public IdentityUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            if (!IsAuthenticated()) return;

            UserName = _accessor.HttpContext?.User?.Identity?.Name;
            var id = _accessor.HttpContext?.User?.Claims?.FirstOrDefault(w => w.Type == "sub")?.Value;
            var roles = _accessor.HttpContext?.User?.Claims?.Where(w => w.Type == "role")?.Select(s => s.Value);

            Id = Guid.Parse(id);
            Roles = roles;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}

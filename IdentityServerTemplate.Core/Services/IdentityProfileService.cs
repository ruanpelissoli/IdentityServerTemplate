using IdentityServerTemplate.Core.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<Account> _claimsFactory;
        private readonly UserManager<Account> _accountManager;        

        public IdentityProfileService(IUserClaimsPrincipalFactory<Account> claimsFactory,
            UserManager<Account> accountManager)
        {
            _claimsFactory = claimsFactory;
            _accountManager = accountManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _accountManager.FindByIdAsync(sub);

            if (user == null || !user.IsActive || user.IsBanned || !user.EmailConfirmed)
                throw new System.Exception("User does not exist"); //TODO: message            

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _accountManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}

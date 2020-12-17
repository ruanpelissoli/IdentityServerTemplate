using IdentityServerTemplate.Core.Entities;
using IdentityServerTemplate.Core.Repositories;
using IdentityServerTemplate.Shared.Consts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> _accountManager;

        public AccountRepository(UserManager<Account> accountManager)
        {
            _accountManager = accountManager;
        }

        public async Task<IdentityResult> CreateUser(Account account, string password, bool isDeveloper, bool isPublisher)
        {
            var result = await _accountManager.CreateAsync(account, password);

            if (!result.Succeeded) return result;

            result = await _accountManager.AddToRoleAsync(account, AccountProfiles.COMMON_USER);

            return result;
        }

        public async Task<IdentityResult> ConfirmEmail(Guid id, string token)
        {
            var user = await _accountManager.FindByIdAsync(id.ToString());

            if (user == null || !user.IsValid())
                return IdentityResult.Failed();

            var result = await _accountManager.ConfirmEmailAsync(user, token);

            return result;
        }

        public async Task<string> GenerateResetPasswordToken(string email)
        {
            var user = await _accountManager.FindByEmailAsync(email);

            if (user == null || !user.IsValid() || !(await _accountManager.IsEmailConfirmedAsync(user)))
                return null;

            var resetPasswordToken = await _accountManager.GeneratePasswordResetTokenAsync(user);
            return resetPasswordToken;
        }

        public async Task<IdentityResult> ResetPassword(string email, string password, string token)
        {
            var user = await _accountManager.FindByEmailAsync(email);
            if (user == null || !user.IsValid())
                return IdentityResult.Failed();

            var result = await _accountManager.ResetPasswordAsync(user, token, password);

            return result;
        }

        public async Task<Account> FindAsync(Guid id)
        {
            return await _accountManager.FindByIdAsync(id.ToString());
        }

        public async Task<Account> FindByEmailAsync(string email)
        {
            return await _accountManager.FindByEmailAsync(email);
        }

        public async Task UpdateAsync(Account user)
        {
            await _accountManager.UpdateAsync(user);
        }
    }
}

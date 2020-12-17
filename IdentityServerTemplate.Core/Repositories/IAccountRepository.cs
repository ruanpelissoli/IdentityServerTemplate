using IdentityServerTemplate.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUser(Account account, string password, bool isDeveloper, bool isPublisher);
        Task<Account> FindAsync(Guid id);
        Task<Account> FindByEmailAsync(string email);
        Task<IdentityResult> ConfirmEmail(Guid id, string token);
        Task<string> GenerateResetPasswordToken(string email);
        Task<IdentityResult> ResetPassword(string email, string password, string token);
        Task UpdateAsync(Account account);
    }
}

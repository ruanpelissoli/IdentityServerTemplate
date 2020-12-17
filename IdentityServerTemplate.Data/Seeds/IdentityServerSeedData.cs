using IdentityServerTemplate.Core.Entities;
using IdentityServerTemplate.Shared.Consts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace IdentityServerTemplate.Data.Seeds
{
    public static class IdentityServerSeedData
    {
        public static void EnsureSeedData(this IdentityServerContext context)
        {
            var adminRoleId = Guid.NewGuid();
            if (!context.Profiles.Any())
            {
                context.Profiles.AddRange(
                    new Profile[] {
                        new Profile
                        {
                            Name = AccountProfiles.ADMIN,
                            NormalizedName = AccountProfiles.ADMIN,
                            Id = adminRoleId,
                            ConcurrencyStamp = adminRoleId.ToString()
                        },
                        new Profile
                        {
                            Name = AccountProfiles.COMMON_USER,
                            NormalizedName = AccountProfiles.COMMON_USER,
                            Id = Guid.NewGuid(),
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        },
                    }
                );
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var user = new Account("admin", "admin@identityserver.com");

                var pwHasher = new PasswordHasher<Account>();
                user.PasswordHash = pwHasher.HashPassword(user, "4adm1ns3rver");
                user.EmailConfirmed = true;

                context.Accounts.Add(user);

                context.UserRoles.Add(
                    new IdentityUserRole<Guid>
                    {
                        RoleId = adminRoleId,
                        UserId = user.Id
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

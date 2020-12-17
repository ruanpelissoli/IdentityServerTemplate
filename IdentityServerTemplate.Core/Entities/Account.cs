using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServerTemplate.Core.Entities
{
    public class Account : IdentityUser<Guid>
    {
        public bool IsActive { get; private set; }
        public bool IsBanned { get; private set; }
        public DateTime? LastBanAt { get; private set; }
        public int BanCount { get; private set; }

        public Account(string userName, string email)
        {
            UserName = userName;
            NormalizedUserName = userName;
            Email = email;
            NormalizedEmail = email;
            IsActive = true;
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Ban()
        {
            IsBanned = true;
            LastBanAt = DateTime.UtcNow;
            BanCount += 1;
        }

        public void UnBan()
        {
            IsBanned = false;
        }

        public bool IsValid() => IsActive && !IsBanned;
    }
}

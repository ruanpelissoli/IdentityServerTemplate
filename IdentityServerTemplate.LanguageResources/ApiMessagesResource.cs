using IdentityServerTemplate.LanguageResources.Interfaces;
using Microsoft.Extensions.Localization;

namespace IdentityServerTemplate.LanguageResources
{
    public class ApiMessagesResource : IApiMessagesResource
    {
        private readonly IStringLocalizer<ApiMessagesResource> _localizer;

        public ApiMessagesResource(IStringLocalizer<ApiMessagesResource> localizer) =>
            _localizer = localizer;

        public string this[string PropertyName]
        {
            get
            {
                return GetString(PropertyName);
            }
        }

        public string Success => GetString(nameof(Success));

        public string PasswordTooShort => GetString(nameof(PasswordTooShort));

        public string PasswordRequiresLower => GetString(nameof(PasswordRequiresLower));

        public string DuplicateUserName => GetString(nameof(DuplicateUserName));

        public string DuplicateEmail => GetString(nameof(DuplicateEmail));

        private string GetString(string name) =>
            _localizer[name];
    }
}
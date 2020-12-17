using IdentityServerTemplate.Core.Enums;

namespace IdentityServerTemplate.Core.DTOs
{
    public class ForgotPasswordEmailDTO
    {
        public EmailTemplates EmailTemplate { get; set; }
        public string ConfirmationLink { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}

using IdentityServerTemplate.Core.Enums;

namespace IdentityServerTemplate.Core.DTOs
{
    public class ConfirmationEmailDTO
    {
        public EmailTemplates EmailTemplate { get; set; }
        public string ConfirmationLink { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class ForgotPasswordDTO
    {
        public EmailTemplates EmailTemplate { get; set; }
        public string ConfirmationLink { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}

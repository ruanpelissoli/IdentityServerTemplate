namespace IdentityServerTemplate.LanguageResources.Interfaces
{
    public interface IApiMessagesResource
    {
        string this[string PropertyName] { get; }

        string PasswordTooShort { get; }
        string PasswordRequiresLower { get; }
        string DuplicateUserName { get; }
        string DuplicateEmail { get; }
    }
}
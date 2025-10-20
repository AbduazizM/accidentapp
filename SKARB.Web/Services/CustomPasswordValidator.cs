using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace LogApp.Web.Services;

public class CustomPasswordValidator : IPasswordValidator<IdentityUser>
{
    private const int RequiredLength = 6;

    public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
    {
        var errors = new List<IdentityError>();

        if (string.IsNullOrEmpty(password) || password.Length < RequiredLength)
        {
            errors.Add(new IdentityError
            {
                Description = $"Пароль должен содержать не менее {RequiredLength} символов."
            });
        }

        if (!password.Any(char.IsLower))
        {
            errors.Add(new IdentityError
            {
                Description = "Пароль должен содержать хотя бы одну строчную букву."
            });
        }


        return Task.FromResult(
            errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray())
        );
    }
}
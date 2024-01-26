using CoffieShop.Identity.Identity;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace CoffieShop.Identity.Validators
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public CustomResourceOwnerPasswordValidator(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await _signInManager.PasswordSignInAsync(context.UserName,
                          context.Password, true, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(context.UserName);

                context.Result = new GrantValidationResult(subject: user.Id, authenticationMethod: "password");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid password");
            }
        }
    }
}

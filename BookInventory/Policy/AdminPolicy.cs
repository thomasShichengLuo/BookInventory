using BookInventory.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BookInventory.Police
{
    public class AdminRequirement
            : IAuthorizationRequirement
    {
    }


    public class AdminHandler
        : AuthorizationHandler<AdminRequirement>
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;


        public AdminHandler(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AdminRequirement requirement)
        {
            var user = context.User;
            var email = user.Identity?.Name;

            if (string.IsNullOrEmpty(email))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Is the user an admin from book.co.nz
            if (email.ToLower().EndsWith($"@book.co.nz"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}

﻿using Microsoft.AspNetCore.Authorization;

namespace Marvin.Authorization
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy CanGetWathers()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("country", "BR")
                .RequireRole("Admin")
                .Build();
        }
    }
}

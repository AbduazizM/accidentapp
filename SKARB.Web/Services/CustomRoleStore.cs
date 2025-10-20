using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SKARB.Web.Data;
using System.Security.Claims;

public class CustomRoleStore : RoleStore<IdentityRole, IdentityContext, string>
{
    public CustomRoleStore(IdentityContext context, IdentityErrorDescriber describer = null)
        : base(context, describer)
    {
    }

    public override Task<IList<Claim>> GetClaimsAsync(IdentityRole role, CancellationToken cancellationToken = default)
        => Task.FromResult<IList<Claim>>(new List<Claim>());

    public override Task AddClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public override Task RemoveClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
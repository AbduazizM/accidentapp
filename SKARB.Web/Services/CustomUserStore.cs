using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using System.Security.Claims;

namespace LogApp.Web.Services
{
    public class CustomUserStore : UserStore<IdentityUser, IdentityRole, IdentityContext, string>
    {
        public CustomUserStore(IdentityContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        // Отключение Claims
        public override Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken = default)
            => Task.FromResult<IList<Claim>>(new List<Claim>());

        public override Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public override Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public override Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public override Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
            => Task.FromResult<IList<IdentityUser>>(new List<IdentityUser>());

        // Отключение внешних логинов
        public override Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken = default)
            => Task.FromResult<IList<UserLoginInfo>>(new List<UserLoginInfo>());

        public override Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public override Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public override Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
            => Task.FromResult<IdentityUser>(null);
    }
}
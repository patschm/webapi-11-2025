using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsReviews.DAL.EntityFramework;

public class AuthenticationContext : IdentityDbContext<IdentityUser>
{
    public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
}
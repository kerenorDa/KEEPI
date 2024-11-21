using Duende.IdentityServer.EntityFramework.Options;
using Keepi.Shared;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Keepi.Server
{
    public class Db_Context : ApiAuthorizationDbContext<IdentityUser>
    {
        public Db_Context(
            DbContextOptions<Db_Context> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    } 
    
    
    //public class Db_Context : DbContext
    //{
    //    public Db_Context(DbContextOptions<Db_Context> options) : base(options) { }

    //    public DbSet<User> Users { get; set; }

    //    public Db_Context()
    //    {
            
    //    }
    //}

}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortal2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JobPortal2.Models
{
    public class JobPortalContext : IdentityDbContext
    {
        public JobPortalContext()
        {
        }

        public JobPortalContext(DbContextOptions<JobPortalContext> options)
            : base(options)
        {
        }

        public DbSet<ParticularDetails> ParticularDetails { get; set; }
        public DbSet<UserPersonalDetailsTable> UserPersonalDetailsTable { get; set; }
        public DbSet<UserGetisterAndLoginTable> UserGetisterAndLoginTable { get; set; }
        public DbSet<UserJobDetails> UserJobDetails { get; set; }
        public DbSet<UserFollowerandFollowedBy> UserFollowerandFollowedBy { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<JobPortal2.Models.SoretedProfiles> SoretedProfiles { get; set; }

    }
}

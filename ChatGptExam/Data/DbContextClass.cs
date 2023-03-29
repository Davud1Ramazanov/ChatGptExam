using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChatGptExam.Models;

namespace ChatGptExam.Data
{
    public class DbContextClass : IdentityDbContext<IdentityUser>
    {
        protected readonly IConfiguration Configuration;
        public DbContextClass(DbContextOptions configuration) : base(configuration)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Subscription> Subscriptions { get; set; }
        
    }
}

using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KVVKWeb.Models.Dto;


namespace KVVKWeb.Data
{
    public class CustomerDbContext:IdentityDbContext<CustomerInfo>
    {
        public CustomerDbContext()
        {

        }
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("tbl");
            builder.Entity<CustomerInfo>(b =>
            {
                b.ToTable("CustomersInfo");
                b.Property(i => i.CustomerID).UseIdentityColumn(100000, 1).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);//bu kısım oldukça önemli
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokens");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });
        }
        public DbSet<KVVKWeb.Models.Dto.RegisterModel> RegisterModel { get; set; }
        public DbSet<KVVKWeb.Models.Entities.Management> Management { get; set; }
        public DbSet<KVVKWeb.Models.Entities.KeyInfo> KeyInfo { get; set; }
        public DbSet<KVVKWeb.Models.Entities.Packages> Packages { get; set; }
    }
}

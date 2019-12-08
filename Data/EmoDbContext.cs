using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
// project dependencies
using emo_back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace emo_back.Data
{
    public class EmoDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>,
            int,
            IdentityUserClaim<int>,
            IdentityUserRole<int>,
            IdentityUserLogin<int>,
            IdentityRoleClaim<int>,
            IdentityUserToken<int>>
    {
        public EmoDbContext(DbContextOptions<EmoDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Channel>().HasMany(c => c.Messages).WithOne(m => m.Channel).HasForeignKey(m => m.ChannelId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Message>().HasOne(m => m.User).WithMany(u => u.Messages).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            
            // set global query filter for all entities.
            builder.Entity<BaseEntity>().HasQueryFilter(e => !e.IsDeleted);
        }

    }

    public static class BaseExtensions
    {
        public static void SoftDelete<T>(this EmoDbContext db, List<T> rangeToDelete) where T : BaseEntity
        {
            foreach (var item in rangeToDelete)
            {
                item.SoftDelete();
            }
        }

        public static void SoftDelete<T>(this EmoDbContext db, T itemToDelete) where T : BaseEntity
        {
            itemToDelete.SoftDelete();
        }

        public static void SoftDeleteRange<T>(this EmoDbContext db, List<T> rangeToDelete) where T : BaseEntity
        {
            foreach (var item in rangeToDelete)
            {
                item.SoftDelete();
            }
        }

        public static void SoftDelete(this BaseEntity item)
        {
            item.IsDeleted = true;
            item.UpdatedAt = DateTime.Now;
        }

        public static void SoftDelete<T>(this DbSet<T> entity, Func<T, bool> predicate) where T : BaseEntity
        {
            entity.Where(predicate).ToList().ForEach(e => e.SoftDelete());
        }

        public static void SoftDelete<T>(this DbSet<T> entity, Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            entity.Where(predicate).ToList().ForEach(e => e.SoftDelete());
        }
    }

}
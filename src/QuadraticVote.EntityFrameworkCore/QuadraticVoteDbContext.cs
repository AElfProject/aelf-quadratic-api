using Microsoft.EntityFrameworkCore;
using QuadraticVote.Domain;
using QuadraticVote.Domain.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace QuadraticVote.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class QuadraticVoteDbContext : AbpDbContext<QuadraticVoteDbContext>
    {
        public DbSet<Project> ProjectRoundInfos { get; set; }
        public DbSet<Round> RoundInfos { get; set; }
        public DbSet<UserProjectInfo> UserProjectInfos { get; set; }

        public QuadraticVoteDbContext(DbContextOptions<QuadraticVoteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Project>(b =>
            {
                b.ToTable(QuadraticVoteConsts.DbTablePrefix + "Project", QuadraticVoteConsts.DbSchema);
                b.HasIndex(x => new { x.RoundNumber });
                b.ApplyObjectExtensionMappings();
                b.ConfigureByConvention();
            });

            builder.Entity<Round>(b =>
            {
                b.ToTable(QuadraticVoteConsts.DbTablePrefix + "Round", QuadraticVoteConsts.DbSchema);
                b.ApplyObjectExtensionMappings();
                b.ConfigureByConvention();
            });

            builder.Entity<UserProjectInfo>(b =>
            {
                b.ToTable(QuadraticVoteConsts.DbTablePrefix + "UserProjectInfo", QuadraticVoteConsts.DbSchema);
                b.Property(x => x.ProjectId).IsRequired();
                b.Property(x => x.User).IsRequired();
                b.HasIndex(x => new { x.RoundNumber });
                b.ConfigureByConvention();
            });
        }
    }
}
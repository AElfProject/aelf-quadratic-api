using System;
using Microsoft.Extensions.DependencyInjection;
using QuadraticVote.Domain;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace QuadraticVote.EntityFrameworkCore
{
    [DependsOn(
        typeof(QuadraticVoteDomainModule),
        typeof(AbpEntityFrameworkCoreMySQLModule)
    )]
    public class QuadraticVoteEntityFrameworkModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddAbpDbContext<QuadraticVoteDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also AElfWalletServerMigrationsDbContextFactory for EF Core tooling. */
                options.UseMySQL();
            });
        }
        
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            using var serviceScope = context.ServiceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
            if (serviceScope == null) return;
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<QuadraticVoteDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}
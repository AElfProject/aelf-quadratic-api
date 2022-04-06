using Microsoft.Extensions.DependencyInjection.Extensions;
using QuadraticVote.Application;
using QuadraticVote.Application.Services;
using QuadraticVote.Application.Services.Impl;
using Volo.Abp.Modularity;

namespace QuadraticVote
{
    [DependsOn(
        typeof(QuadraticVoteDomainTestModule),
        typeof(QuadraticVoteApplicationModule)
    )]
    public class QuadraticVoteApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.RemoveAll<IQuadraticVoteAppService>();
            services.TryAddTransient<IQuadraticVoteAppService, QuadraticVoteAppService>();
        }
    }
}
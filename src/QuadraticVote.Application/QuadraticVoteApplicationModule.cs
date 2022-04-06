using Microsoft.Extensions.DependencyInjection;
using QuadraticVote.Application.Options;
using QuadraticVote.Domain;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace QuadraticVote.Application
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(QuadraticVoteDomainModule))]
    public class QuadraticVoteApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<ApiOption>(configuration.GetSection("ApiOption"));
        }
    }
}
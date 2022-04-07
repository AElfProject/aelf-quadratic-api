using AElf.AElfNode.EventHandler.BackgroundJob;
using Microsoft.Extensions.DependencyInjection;
using QuadraticVote.Domain;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace QuadraticVote.ContractEventHandler
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(QuadraticVoteDomainModule),
        typeof(AbpAutoMapperModule),
        typeof(AElfEventHandlerBackgroundJobModule)
    )]
    public class QuadraticVoteEventHandlerCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
        }
    }
}
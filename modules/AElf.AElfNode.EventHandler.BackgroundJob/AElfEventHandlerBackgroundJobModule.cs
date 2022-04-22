using AElf.AElfNode.EventHandler.BackgroundJob.BackgroundWorker;
using AElf.AElfNode.EventHandler.BackgroundJob.Options;
using AElf.AElfNode.EventHandler.BackgroundJob.Provider;
using AElf.AElfNode.EventHandler.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundWorkers;

namespace AElf.AElfNode.EventHandler.BackgroundJob
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AElfNodeEventHandlerCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpBackgroundWorkersModule)
    )]
    public class AElfEventHandlerBackgroundJobModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IAElfEventProcessorProvider, AElfEventProcessorProvider>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AElfEventHandlerBackgroundJobModule>();
            });
        }
        
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AElfProcessorOption>>().Value;
            if (!options.IsCheckFork) return;
            var backgroundWorkerManager = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
            var worker = context.ServiceProvider.GetRequiredService<ForkTransactionCheckWorker>();
            backgroundWorkerManager.AddAsync(worker);
        }
    }
}
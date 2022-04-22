using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.TestBase.Providers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.AElfNode.EventHandler.TestBase
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AElfEventHandlerBackgroundJobModule)
    )]
    public class AElfEventHandlerTestBaseModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var serviceProvider = context.Services;
            serviceProvider.AddTransient(typeof(IEventHandlerTestProcessor<>), typeof(EventHandlerTestProcessor<>));
            base.ConfigureServices(context);
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var processorsActionProvider = context.ServiceProvider.GetService<IProcessorsActionProvider>();
            processorsActionProvider?.RegisterProcessor();
        }
    }
}
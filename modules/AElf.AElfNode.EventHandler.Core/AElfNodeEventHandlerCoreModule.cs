using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.AElfNode.EventHandler.Core
{
    [DependsOn(
        typeof(AbpAutofacModule))]
    public class AElfNodeEventHandlerCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
using AElf.AElfNode.EventHandler.Core;
using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.AElfNode.EventHandler.EntityFrameworkCore.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AElf.AElfNode.EventHandler.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AElfNodeEventHandlerCoreModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AElfNodeEventHandlerEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddAbpDbContext<AElfNodeDbContext>(options =>
            {
                options.AddRepository<TransactionWithLogsInfo, EfCoreTransactionWithLogsInfoRepository>();
                options.AddRepository<SaveData, EfCoreSaveDataRepository>();
            });
        }
    }
}
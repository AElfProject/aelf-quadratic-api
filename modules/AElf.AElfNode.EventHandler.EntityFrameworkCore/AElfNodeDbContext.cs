using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.AElfNode.EventHandler.EntityFrameworkCore.Constants;
using AElf.AElfNode.EventHandler.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AElf.AElfNode.EventHandler.EntityFrameworkCore
{
    [ConnectionStringName(AElfNodeScanDbProperties.ConnectionStringName)]
    public class AElfNodeDbContext: AbpDbContext<AElfNodeDbContext>, IAElfNodeDbContext
    {
        public AElfNodeDbContext(DbContextOptions<AElfNodeDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAElfLibTransactionManagement();
        }

        public DbSet<SaveData> SaveData { get; }
        public DbSet<TransactionWithLogsInfo> TransactionWithLogsInfos { get; }
    }
}
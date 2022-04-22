using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AElf.AElfNode.EventHandler.EntityFrameworkCore
{
    public interface IAElfNodeDbContext: IEfCoreDbContext
    {
        public DbSet<SaveData> SaveData { get;}
        public DbSet<TransactionWithLogsInfo> TransactionWithLogsInfos { get;}
    }
}
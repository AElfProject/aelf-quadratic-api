using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.AElfNode.EventHandler.Core.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AElf.AElfNode.EventHandler.EntityFrameworkCore.Repositories
{
    public class
        EfCoreTransactionWithLogsInfoRepository : EfCoreRepository<IAElfNodeDbContext, TransactionWithLogsInfo,
            long>, ITransactionWithLogsInfoRepository
    {
        public EfCoreTransactionWithLogsInfoRepository(IDbContextProvider<IAElfNodeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
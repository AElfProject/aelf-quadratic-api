using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.AElfNode.EventHandler.Core.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AElf.AElfNode.EventHandler.EntityFrameworkCore.Repositories
{
    public class EfCoreSaveDataRepository : EfCoreRepository<IAElfNodeDbContext,
        SaveData,
        int>, ISaveDataRepository
    {
        public EfCoreSaveDataRepository(
            IDbContextProvider<IAElfNodeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
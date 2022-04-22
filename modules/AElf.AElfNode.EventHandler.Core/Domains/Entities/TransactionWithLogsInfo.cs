using System;
using Volo.Abp.Domain.Entities;

namespace AElf.AElfNode.EventHandler.Core.Domains.Entities
{
    public abstract class TransactionResultBase
    {
        public string TransactionId { get; set; }
        public long BlockNumber { get; set; }
        public string BlockHash { get; set; }
        public DateTime BlockTime { get; set; }
    }

    public class TransactionWithLogsInfo : TransactionResultBase, IEntity<long>
    {
        public long Id { get; }
        public int ChainId { get; set; }
        public long SaveTicks { get; set; }
        public bool IsFork { get; set; }
        public string Category { get; set; }
        public object[] GetKeys()
        {
            return new object[] {Id};
        }
    }
}
using Volo.Abp.Domain.Entities;

namespace AElf.AElfNode.EventHandler.Core.Domains.Entities
{
    public class SaveData : Entity<int>
    {
        public string Key { get; set; }
        public string Data { get; set; }
    }
}
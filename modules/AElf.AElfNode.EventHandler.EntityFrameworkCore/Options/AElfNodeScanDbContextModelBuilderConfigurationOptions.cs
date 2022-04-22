using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AElf.AElfNode.EventHandler.EntityFrameworkCore.Options
{
    public class AElfNodeScanDbContextModelBuilderConfigurationOptions: 
        AbpModelBuilderConfigurationOptions
    {
        public AElfNodeScanDbContextModelBuilderConfigurationOptions(
            string tablePrefix,
            string schema)
            : base(tablePrefix, schema)
        {
        }
    }
}
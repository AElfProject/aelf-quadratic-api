namespace AElf.AElfNode.EventHandler.EntityFrameworkCore.Constants
{
    public static class AElfNodeScanDbProperties
    {
        public static string DbTablePrefix { get; set; } = "AELF";

        public static string DbSchema { get; set; } = (string) null;
        public const string ConnectionStringName = "AElfNodeScan";
    }
}
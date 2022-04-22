namespace AElf.AElfNode.EventHandler.EntityFrameworkCore.Constants
{
    public class AElfNodeProcessorConsts
    {
        public static int MaxStatusLength { get; set; } = 32;
        public static int MaxTransactionIdLength { get; set; } = 64;
        public static int MaxMethodNameLength { get; set; } = 64;
        public static int MaxDatetimeLength { get; set; } = 16;
        public static int MaxAddressLength { get; set; } = 64;
        public static int MaxBlockHashLength { get; set; } = 128;
        public static int  MaxReturnValueLength { get; set; } = 256;
        public static int  MaxLogsLength { get; set; } = 256;
        public static int  MaxArgumentsLength { get; set; } = 256;
        public static int  MaxDataKeyLength { get; set; } = 64;
        public static int  MaxDataValueLength { get; set; } = 64;
    }
}
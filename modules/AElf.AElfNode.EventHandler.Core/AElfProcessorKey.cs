namespace AElf.AElfNode.EventHandler.Core
{
    public class AElfProcessorKey
    {
        public int ChainId { get; set; }
        public string ContractAddress { get; set; }
        public string EventName { get; set; }
        public string ProcessorName { get; set; }
    }
}
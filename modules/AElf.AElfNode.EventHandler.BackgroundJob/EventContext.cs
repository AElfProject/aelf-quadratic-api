using System.Collections.Generic;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;

namespace AElf.AElfNode.EventHandler.BackgroundJob
{
    public class EventContext: TransactionResultEto
    { 
        public int ChainId { get; set; }
        public string EventName { get; set; }
        public string EventAddress { get; set; }
        public Dictionary<string, string> Arguments { get; } = new();
    }
}
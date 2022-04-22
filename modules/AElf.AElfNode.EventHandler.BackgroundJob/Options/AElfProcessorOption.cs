using System.Collections.Generic;
using AElf.AElfNode.EventHandler.Core;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Options
{
    public class AElfProcessorOption
    {
        public bool IsCheckFork { get; set; }
        public bool IsDeleteTx { get; set; } = true;
        public int WorkerRepeatInternal { get; set; }
        public int TxCount { get; set; }
        public string JobCategory { get; set; }
        public string EventBusIsolationLevel { get; set; }
        public string BackgroundJobIsolationLevel { get; set; }
        public Dictionary<string, string> NodeUrlDic { get;} = new();
        public List<AElfProcessorKey> ProcessorKeyList { get;} = new();
    }
}
using System.Collections.Generic;
using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using Volo.Abp.EventBus;

namespace AElf.AElfNode.EventHandler.BackgroundJob.ETO
{
    [EventName("AElf.WebApp.MessageQueue.TransactionResultListEto")]
    public class TransactionResultListEto
    {
        public Dictionary<string, List<TransactionResultEto>> TransactionResults { get; set; }
        public long StartBlockNumber { get; set; }
        public long EndBlockNumber { get; set; }

        public int ChainId { get; set; }
    }

    public class TransactionResultEto: TransactionResultBase
    {
        public string Status { get; set; }
        public string MethodName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ReturnValue { get; set; }
        public string Error { get; set; }
        public LogEventEto[] Logs { get; set; }
    }

    public class LogEventEto
    {
        public string Address { get; set; }

        public string Name { get; set; }

        public string[] Indexed { get; set; }

        public string NonIndexed { get; set; }
    }
}
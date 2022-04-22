using AElf.AElfNode.EventHandler.BackgroundJob.ETO;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Services
{
    public interface ITransactionFilter
    {
        bool IsValidTransaction(TransactionResultEto txEto);
    }
}
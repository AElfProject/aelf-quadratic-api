using System.Collections.Generic;
using System.Linq;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using Volo.Abp.DependencyInjection;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Services
{
    public interface IFilterTransactionService
    {
        IEnumerable<TransactionResultEto> FilterTransaction(IEnumerable<TransactionResultEto> txEtos);
    }

    public class FilterTransactionService : IFilterTransactionService, ITransientDependency
    {
        private readonly List<ITransactionFilter> _filters;

        public FilterTransactionService(IEnumerable<ITransactionFilter> filters)
        {
            _filters = filters.ToList();
        }

        public IEnumerable<TransactionResultEto> FilterTransaction(IEnumerable<TransactionResultEto> txEtos)
        {
            return !_filters.Any() ? txEtos : txEtos.Where(t => _filters.All(f => f.IsValidTransaction(t))).ToList();
        }
    }
}
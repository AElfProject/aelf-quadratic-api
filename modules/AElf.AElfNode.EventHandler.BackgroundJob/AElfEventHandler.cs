using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using AElf.AElfNode.EventHandler.BackgroundJob.Options;
using AElf.AElfNode.EventHandler.BackgroundJob.Services;
using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.AElfNode.EventHandler.Core.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace AElf.AElfNode.EventHandler.BackgroundJob
{
    public class AElfEventHandler : IAsyncBackgroundJob<TransactionResultListEto>,
        IDistributedEventHandler<TransactionResultListEto>, ITransientDependency
    {
        private readonly ILogger<AElfEventHandler> _logger;
        private readonly bool _isCheckFork;
        private readonly string _jobCategory;
        private readonly IAbpLazyServiceProvider _lazyServiceProvider;
        private readonly ILogEventProcessor _logEventProcessor;
        private readonly IFilterTransactionService _filterTransactionService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IsolationLevel _eventBusIsolationLevel;
        private readonly IsolationLevel _backgroundJobIsolationLevel;
        private IObjectMapper ObjectMapper => _lazyServiceProvider.LazyGetService<IObjectMapper>();
        private INodeService LibService => _lazyServiceProvider.LazyGetService<INodeService>();

        private ITransactionWithLogsInfoRepository TransactionWithLogsInfoRepository =>
            _lazyServiceProvider.LazyGetService<ITransactionWithLogsInfoRepository>();

        public AElfEventHandler(
            ILogger<AElfEventHandler> logger,
            IOptionsSnapshot<AElfProcessorOption> options, IAbpLazyServiceProvider lazyServiceProvider,
            ILogEventProcessor logEventProcessor, IFilterTransactionService filterTransactionService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _logger = logger;
            _lazyServiceProvider = lazyServiceProvider;
            _logEventProcessor = logEventProcessor;
            _filterTransactionService = filterTransactionService;
            _unitOfWorkManager = unitOfWorkManager;
            _isCheckFork = options.Value.IsCheckFork;
            _jobCategory = options.Value.JobCategory;
            _eventBusIsolationLevel = GetIsolationLevel(options.Value.EventBusIsolationLevel, false);
            _backgroundJobIsolationLevel = GetIsolationLevel(options.Value.BackgroundJobIsolationLevel, true);
        }

        public async Task ExecuteAsync(TransactionResultListEto eventData)
        {
            using var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: true, isolationLevel: _backgroundJobIsolationLevel
            );
            await HandleAsync(eventData);
            await uow.CompleteAsync();
        }

        public async Task HandleEventAsync(TransactionResultListEto eventData)
        {
            using var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: true, isolationLevel: _eventBusIsolationLevel
            );
            await HandleAsync(eventData);
            await uow.CompleteAsync();
        }

        private async Task HandleAsync(TransactionResultListEto eventData)
        {
            var lib = 0L;
            if (_isCheckFork)
            {
                lib = await LibService.GetIrreversibleChainBlockHeightAsync(eventData.ChainId);
            }

            foreach (var txEto in _filterTransactionService.FilterTransaction(
                eventData.TransactionResults.Values.SelectMany(x => x)))
            {
                if (_isCheckFork && txEto.BlockNumber > lib)
                {
                    _logger.LogInformation($"Tx: {txEto.TransactionId} saved into db");
                    await TrySaveTransactionInformationAsync(txEto, eventData.ChainId, _jobCategory);
                    continue;
                }

                await _logEventProcessor.ProcessTransactionAsync(eventData.ChainId, txEto);
            }
        }

        private async Task TrySaveTransactionInformationAsync(TransactionResultEto txEto, int chainId, string category)
        {
            var tx = ObjectMapper.Map<TransactionResultEto, TransactionWithLogsInfo>(txEto);
            tx.ChainId = chainId;
            tx.SaveTicks = DateTime.UtcNow.Ticks;
            tx.Category = category;
            await TransactionWithLogsInfoRepository.InsertAsync(tx);
        }

        private IsolationLevel GetIsolationLevel(string level, bool isBackgroundJob)
        {
            if (!string.IsNullOrEmpty(level))
                return level switch
                {
                    "Unspecified" => IsolationLevel.Unspecified,
                    "Chaos" => IsolationLevel.Chaos,
                    "ReadUncommitted" => IsolationLevel.ReadUncommitted,
                    "ReadCommitted" => IsolationLevel.ReadCommitted,
                    "RepeatableRead" => IsolationLevel.RepeatableRead,
                    "Serializable" => IsolationLevel.Serializable,
                    "Snapshot" => IsolationLevel.Snapshot,
                    _ => throw new Exception($"Invalid isolation type : {level}")
                };
            return isBackgroundJob ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted;
        }
    }
}
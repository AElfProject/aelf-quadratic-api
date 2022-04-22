using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using AElf.AElfNode.EventHandler.BackgroundJob.Options;
using AElf.AElfNode.EventHandler.BackgroundJob.Services;
using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.AElfNode.EventHandler.Core.Repositories;
using AElf.Client.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace AElf.AElfNode.EventHandler.BackgroundJob.BackgroundWorker
{
    public class ForkTransactionCheckWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IAbpLazyServiceProvider _lazyServiceProvider;
        private readonly INodeService _nodeService;
        private readonly ILogEventProcessor _logEventProcessor;
        private readonly List<int> _chainIds;
        private readonly ILogger<ForkTransactionCheckWorker> _logger;
        private readonly int _txCount;
        private readonly bool _isDeleteTx;
        private readonly string _jobName;
        private IObjectMapper ObjectMapper => _lazyServiceProvider.LazyGetService<IObjectMapper>();
        private const string LatestCheckTickKey = "LatestCheckTickKey";
        private const int DefaultTxCount = 50;
        private const int DefaultInternal = 60000; //1 minutes

        private ITransactionWithLogsInfoRepository TransactionWithLogsInfoRepository =>
            _lazyServiceProvider.LazyGetService<ITransactionWithLogsInfoRepository>();

        private ISaveDataRepository SaveDataRepository => _lazyServiceProvider.LazyGetService<ISaveDataRepository>();

        public ForkTransactionCheckWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory, IAbpLazyServiceProvider lazyServiceProvider,
            INodeService nodeService,
            ILogger<ForkTransactionCheckWorker> logger, IOptionsSnapshot<AElfProcessorOption> options,
            ILogEventProcessor logEventProcessor
        ) : base(
            timer,
            serviceScopeFactory)
        {
            Timer.Period = options.Value.WorkerRepeatInternal > 0
                ? options.Value.WorkerRepeatInternal
                : DefaultInternal;
            _lazyServiceProvider = lazyServiceProvider;
            _nodeService = nodeService;
            _logger = logger;
            _logEventProcessor = logEventProcessor;
            _chainIds = options.Value.NodeUrlDic.Keys.Select(x => int.Parse(x)).ToList();
            _txCount = options.Value.TxCount > 0 ? options.Value.TxCount : DefaultTxCount;
            _isDeleteTx = options.Value.IsDeleteTx;
            _jobName = options.Value.JobCategory;
            if (string.IsNullOrEmpty(_jobName))
            {
                throw new Exception("Lack of fork job name");
            }
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            foreach (var chainId in _chainIds)
            {
                await HandleByChainAsync(chainId, _jobName);
            }
        }

        [UnitOfWork(true, IsolationLevel.ReadCommitted)]
        protected virtual async Task HandleByChainAsync(int chainId, string jobName)
        {
            var dataKey = $"{chainId}-{jobName}-{LatestCheckTickKey}";
            var lastUpdateTimeData = await SaveDataRepository.FindAsync(x => x.Key == dataKey);
            var lastUpdateTime = lastUpdateTimeData != null ? long.Parse(lastUpdateTimeData.Data) : 0L;
            var bestHeight = await _nodeService.GetIrreversibleChainBlockHeightAsync(chainId);
            _logger.LogInformation($"prepared to check fork transaction, current Lib height: {bestHeight}");
            var transactionList = (await TransactionWithLogsInfoRepository.GetQueryableAsync())
                .Where(TransactionWithLogFilter(lastUpdateTime,
                    chainId, bestHeight, jobName))
                .OrderBy(x => x.SaveTicks)
                .Take(_txCount).ToList();
            _logger.LogInformation($"{transactionList.Count} transactions to be check");
            if (!transactionList.Any())
                return;
            var forkTransactionList = new List<TransactionWithLogsInfo>();
            foreach (var tx in transactionList)
            {
                if (await HandleTransactionInfoSuccessfullyAsync(tx)) continue;
                tx.IsFork = true;
                forkTransactionList.Add(tx);
            }

            if (_isDeleteTx)
            {
                await TransactionWithLogsInfoRepository.DeleteManyAsync(transactionList);
            }
            else
            {
                await TransactionWithLogsInfoRepository.UpdateManyAsync(forkTransactionList);
            }

            await UpdateSaveDataAsync(lastUpdateTimeData, dataKey, transactionList.Last().SaveTicks);
        }

        private async Task<bool> HandleTransactionInfoSuccessfullyAsync(TransactionWithLogsInfo txInfo)
        {
            var txResult = await _nodeService.GetTransactionResultAsync(txInfo.ChainId, txInfo.TransactionId);
            if (txResult.BlockNumber != txInfo.BlockNumber || txInfo.BlockHash != txResult.BlockHash)
            {
                _logger.LogInformation(
                    $"find fork tx, tx id: {txInfo.TransactionId}");
                return false;
            }

            var txEto = ObjectMapper.Map<TransactionResultDto, TransactionResultEto>(txResult);
            txEto.BlockTime = txInfo.BlockTime;
            await _logEventProcessor.ProcessTransactionAsync(txInfo.ChainId, txEto);
            return true;
        }

        private async Task UpdateSaveDataAsync(SaveData saveData, string key, long newTime)
        {
            if (saveData != null)
            {
                saveData.Data = newTime.ToString();
                await SaveDataRepository.UpdateAsync(saveData);
                return;
            }

            await SaveDataRepository.InsertAsync(new SaveData
            {
                Key = key,
                Data = newTime.ToString()
            });
        }

        private Expression<Func<TransactionWithLogsInfo, bool>> TransactionWithLogFilter(long lastUpdateTime,
            int chainId, long bestHeight, string jobName)
        {
            return transactionWithLogsInfo => transactionWithLogsInfo.SaveTicks > lastUpdateTime &&
                                              transactionWithLogsInfo.ChainId == chainId &&
                                              transactionWithLogsInfo.BlockNumber <= bestHeight &&
                                              transactionWithLogsInfo.Category == jobName;
        }
    }
}
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob.Provider;
using AElf.Client.Dto;
using Volo.Abp.DependencyInjection;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Services
{
    public interface INodeService
    {
        Task<long> GetIrreversibleChainBlockHeightAsync(int chainId);
        Task<TransactionResultDto> GetTransactionResultAsync(int chainId, string txId);
        Task<BlockDto> GetBlockInfoAsync(int chainId, long blockHeight);
    }

    public class DefaultNodeService : INodeService, ITransientDependency
    {
        private readonly IAElfClientProvider _clientProvider;
        public DefaultNodeService(IAElfClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public async Task<long> GetIrreversibleChainBlockHeightAsync(int chainId)
        {
            var client = _clientProvider.GetClient(chainId);
            return (await client.GetChainStatusAsync()).LastIrreversibleBlockHeight;
        }

        public async Task<TransactionResultDto> GetTransactionResultAsync(int chainId, string txId)
        {
            var client = _clientProvider.GetClient(chainId);
            return await client.GetTransactionResultAsync(txId);
        }

        public async Task<BlockDto> GetBlockInfoAsync(int chainId, long blockHeight)
        {
            var client = _clientProvider.GetClient(chainId);
            return await client.GetBlockByHeightAsync(blockHeight);
        }
    }
}
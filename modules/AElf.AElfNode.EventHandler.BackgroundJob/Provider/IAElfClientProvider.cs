using System;
using System.Collections.Generic;
using System.Linq;
using AElf.AElfNode.EventHandler.BackgroundJob.Options;
using AElf.Client.Service;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Provider
{
    public interface IAElfClientProvider
    {
        AElfClient GetClient(int chainId);
    }

    public class DefaultAElfClientProvider : IAElfClientProvider, ISingletonDependency
    {
        private readonly Dictionary<int, AElfClient> _clientDic;
        private readonly Dictionary<int, string> _nodeUrlDic;

        public DefaultAElfClientProvider(IOptionsSnapshot<AElfProcessorOption> options)
        {
            _clientDic = new Dictionary<int, AElfClient>();
            _nodeUrlDic = options.Value.NodeUrlDic.ToDictionary(x => int.Parse(x.Key), x => x.Value);
        }

        public AElfClient GetClient(int chainId)
        {
            if (_clientDic.TryGetValue(chainId, out var client))
            {
                return client;
            }

            if (!_nodeUrlDic.ContainsKey(chainId))
            {
                throw new Exception($"ChainId: {chainId} api is not found");
            }

            client = new AElfClient(_nodeUrlDic[chainId]);
            _clientDic.TryAdd(chainId, client);
            return client;
        }
    }
}
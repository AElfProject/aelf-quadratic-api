using System;
using AElf.Client.Service;
using Microsoft.Extensions.Options;
using QuadraticVote.Application.Options;
using Volo.Abp.DependencyInjection;

namespace QuadraticVote.Application.Provider
{
    public interface IAElfClientProvider
    {
        AElfClient GetAElfClient();
    }

    public class DefaultAElfClientProvider : IAElfClientProvider, ISingletonDependency
    {
        private AElfClient _aelfClient = null;
        private readonly object _locker = new object();
        private readonly string _nodeApiUrl;

        public DefaultAElfClientProvider(IOptionsSnapshot<ApiOption> apiOption)
        {
            _nodeApiUrl = apiOption.Value.NodeApiUrl;
        }

        public AElfClient GetAElfClient()
        {
            if (_aelfClient != null)
            {
                return _aelfClient;
            }

            if (string.IsNullOrEmpty(_nodeApiUrl))
            {
                throw new Exception("Lack AELF node api URL");
            }

            lock (_locker)
            {
                if (_aelfClient != null)
                {
                    return _aelfClient;
                }

                _aelfClient = new AElfClient(_nodeApiUrl);
            }

            return _aelfClient;
        }
    }
}
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.Contracts.QuadraticFunding;
using QuadraticVote.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace QuadraticVote.ContractEventHandler.Processors
{
    public class RoundStartedProcessor : AElfEventProcessorBase<RoundStarted>
    {
        private readonly IRepository<Round> _roundInfosRepository;

        public RoundStartedProcessor(IRepository<Round> roundInfosRepository)
        {
            _roundInfosRepository = roundInfosRepository;
        }

        protected override async Task HandleEventAsync(RoundStarted eventDetailsEto, EventContext txInfoDto)
        {
            var round = await _roundInfosRepository.FindAsync(x => x.RoundNumber == eventDetailsEto.Round);
            if (round != null)
            {
                return;
            }
            
            await _roundInfosRepository.InsertAsync(new Round
            {
                RoundNumber = eventDetailsEto.Round
            });
        }
    }
}
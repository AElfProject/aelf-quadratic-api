using System;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.Contracts.QuadraticFunding;
using QuadraticVote.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace QuadraticVote.ContractEventHandler.Processors
{
    public class SupportUpdatedProcessor : AElfEventProcessorBase<SupportUpdated>
    {
        private readonly IRepository<Round> _roundInfosRepository;

        public SupportUpdatedProcessor(IRepository<Round> roundInfosRepository)
        {
            _roundInfosRepository = roundInfosRepository;
        }

        protected override async Task HandleEventAsync(SupportUpdated eventDetailsEto, EventContext txInfoDto)
        {
            var roundInfo = await _roundInfosRepository.FindAsync(x => x.RoundNumber == eventDetailsEto.Round);
            if (roundInfo == null)
            {
                throw new Exception($"Lack round info in db,  round: {eventDetailsEto.Round}");
            }
            
            roundInfo.TotalSupport += eventDetailsEto.Support;
            await _roundInfosRepository.UpdateAsync(roundInfo);
        }
    }
}
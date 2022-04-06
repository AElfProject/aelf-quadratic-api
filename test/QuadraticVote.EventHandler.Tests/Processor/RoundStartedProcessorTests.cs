using System;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.TestBase;
using AElf.Contracts.QuadraticFunding;
using Shouldly;
using Xunit;

namespace QuadraticVote.Processor
{
    public partial class QuadraticVoteProcessorTests
    {
        [Fact]
        public async Task RoundStarted_Should_Add_Project()
        {
            var roundNumber = 1;
            await RoundStartedAsync(roundNumber);
            var round = await _roundInfosRepository.GetAsync(x => x.RoundNumber == roundNumber);
            round.RoundNumber.ShouldBe(roundNumber);
            round.TotalSupport.ShouldBe(0);
        }

        [Fact]
        public async Task RoundStarted_Add_Round_Repeatedly_Should_Add_One_Round()
        {
            var roundNumber = 1;
            await RoundStartedAsync(roundNumber);
            var round = await _roundInfosRepository.GetAsync(x => x.RoundNumber == roundNumber);
            var support = 1000;
            round.TotalSupport = support;
            await _roundInfosRepository.UpdateAsync(round);
            
            await RoundStartedAsync(roundNumber);
            round = await _roundInfosRepository.GetAsync(x => x.RoundNumber == roundNumber);
            round.TotalSupport.ShouldBe(support);
        }
        
        private async Task RoundStartedAsync(int round)
        {
            var projectUploadedProcessor = GetRequiredService<IEventHandlerTestProcessor<RoundStarted>>();
            await projectUploadedProcessor.HandleEventAsync(new RoundStarted
            {
                Round = round
            }, new EventContext());
        }
    }
}
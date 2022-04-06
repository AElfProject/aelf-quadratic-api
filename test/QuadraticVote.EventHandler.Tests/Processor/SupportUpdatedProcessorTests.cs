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
        public async Task SupportUpdated_Should_Modify_Round_Support()
        {
            var roundNumber = 1;
            await RoundStartedAsync(roundNumber);
            var supportOne = 10012313;
            await SupportUpdatedAsync(roundNumber, supportOne);
            var round = await _roundInfosRepository.GetAsync(x => x.RoundNumber == roundNumber);
            round.TotalSupport.ShouldBe(supportOne);

            var supportTwo = 10012313;
            await SupportUpdatedAsync(roundNumber, supportTwo);
            round = await _roundInfosRepository.GetAsync(x => x.RoundNumber == roundNumber);
            round.TotalSupport.ShouldBe(supportOne + supportTwo);
        }

        [Fact]
        public async Task SupportUpdated_Without_Existed_Round_Should_Throw_Exception()
        {
            var round = 1;
            var support = 1000;
            var exception =
                await Assert.ThrowsAsync<Exception>(async () => await SupportUpdatedAsync(round, support));
            exception.Message.ShouldBe($"Lack round info in db,  round: {round}");
        }

        private async Task SupportUpdatedAsync(long round, long support)
        {
            var projectUploadedProcessor = GetRequiredService<IEventHandlerTestProcessor<SupportUpdated>>();
            await projectUploadedProcessor.HandleEventAsync(new SupportUpdated
            {
                Round = round,
                Support = support
            }, new EventContext());
        }
    }
}
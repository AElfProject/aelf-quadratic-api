using System;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.TestBase;
using AElf.Contracts.QuadraticFunding;
using AElf.Types;
using Shouldly;
using Xunit;

namespace QuadraticVote.Processor
{
    public partial class QuadraticVoteProcessorTests
    {
        [Fact]
        public async Task Vote_Should_Modify_Project_And_User()
        {
            var round = 1;
            var projectId = QuadraticVoteConstants.ProjectOneId;
            await ProjectUploadedAsync(projectId, round);
            var userAddress = QuadraticVoteConstants.Radahn;
            var user = userAddress.ToBase58();
            var vote = 12304;
            var cost = 321312;
            var grants = 32123;
            var supportArea = 21313;

            var userBeforeVote =
                await _userProjectInfosRepository.FindAsync(u => u.RoundNumber == round && u.User == user);
            userBeforeVote.ShouldBe(null);
            await VoteAsync(round, userAddress, projectId, vote, grants, supportArea, cost);
            var userAfterVote =
                await _userProjectInfosRepository.FindAsync(u => u.RoundNumber == round && u.User == user);
            var projectInfo =
                await _projectRoundInfosRepository.FindAsync(p => p.RoundNumber == round && p.ProjectId == projectId);
            userAfterVote.Vote.ShouldBe(vote);
            projectInfo.SupportArea.ShouldBe(supportArea);
            projectInfo.Grant.ShouldBe(grants);
        }

        [Fact]
        public async Task Vote_Not_Existed_Project_Should_Throw_Exception()
        {
            var round = 1;
            var projectId = QuadraticVoteConstants.ProjectOneId;
            var userAddress = QuadraticVoteConstants.Radahn;
            var vote = 12304;
            var grants = 32123;
            var supportArea = 21313;
            var cost = 321_00000000L;
            var exception =
                await Assert.ThrowsAsync<Exception>(async () =>
                    await VoteAsync(round, userAddress, projectId, vote, grants, supportArea, cost));
            exception.Message.ShouldBe($"Lack project Info in db, project: {projectId}, round: {round}");
        }

        private async Task VoteAsync(long round, Address account, string projectId, long vote, long grants,
            long supportArea, long cost)
        {
            var projectUploadedProcessor = GetRequiredService<IEventHandlerTestProcessor<Voted>>();
            await projectUploadedProcessor.HandleEventAsync(new Voted
            {
                Round = round,
                Account = account,
                Project = projectId,
                Vote = vote,
                Grants = grants,
                SupportArea = supportArea,
                Cost = cost
            }, new EventContext());
        }
    }
}
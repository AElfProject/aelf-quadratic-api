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
        public async Task ProjectBanned_Should_Modify_Ban_Status()
        {
            var round = 1;
            var projectId = QuadraticVoteConstants.ProjectOneId;
            await ProjectUploadedAsync(projectId, round);
            var ban = true;
            await ProjectBannedAsync(projectId, round, ban);
            var project = await _projectRoundInfosRepository.GetAsync(x => x.ProjectId == projectId);
            project.IsBanned.ShouldBe(ban);

            ban = false;
            await ProjectBannedAsync(projectId, round, ban);
            project = await _projectRoundInfosRepository.GetAsync(x => x.ProjectId == projectId);
            project.IsBanned.ShouldBe(ban);
        }

        [Fact]
        public async Task ProjectBanned_Modify_Ban_Of_Not_Existed_Project_Should_Throw_Exception()
        {
            var round = 1;
            var projectId = QuadraticVoteConstants.ProjectOneId;
            var exception =
                await Assert.ThrowsAsync<Exception>(async () => await ProjectBannedAsync(projectId, round, true));
            exception.Message.ShouldBe($"Lack project Info in db, project: {projectId}, round: {round}");
        }
        
        private async Task ProjectBannedAsync(string projectId, long round, bool isBanned)
        {
            var projectUploadedProcessor = GetRequiredService<IEventHandlerTestProcessor<ProjectBanned>>();
            await projectUploadedProcessor.HandleEventAsync(new ProjectBanned
            {
               Project = projectId,
               Round = round,
               Ban = isBanned
            }, new EventContext());
        }
    }
}
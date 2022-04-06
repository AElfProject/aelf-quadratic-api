using System;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.TestBase;
using AElf.Contracts.QuadraticFunding;
using AElf.Types;
using QuadraticVote.Domain.Entities;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;
using Project = QuadraticVote.Domain.Entities.Project;

namespace QuadraticVote.Processor
{
    public partial class QuadraticVoteProcessorTests : QuadraticVoteEventHandlerTestBase
    {
        private readonly IRepository<Project> _projectRoundInfosRepository;
        private readonly IRepository<UserProjectInfo> _userProjectInfosRepository;
        private readonly IRepository<Round> _roundInfosRepository;

        public QuadraticVoteProcessorTests()
        {
            _projectRoundInfosRepository = GetRequiredService<IRepository<Project>>();
            _userProjectInfosRepository = GetRequiredService<IRepository<UserProjectInfo>>();
            _roundInfosRepository = GetRequiredService<IRepository<Round>>();
        }

        [Fact]
        public async Task ProjectUpload_Should_Add_Project()
        {
            var round = 1;
            var projectId = QuadraticVoteConstants.ProjectOneId;
            await ProjectUploadedAsync(projectId, round);
            var project = await _projectRoundInfosRepository.GetAsync(x => x.ProjectId == projectId);
            project.RoundNumber.ShouldBe(round);
            project.Bid.ShouldBe(QuadraticVoteConstants.ProjectOneBid);
            project.ProjectNumber.ShouldBe(QuadraticVoteConstants.ProjectOneNum);
            project.IsBanned.ShouldBe(false);
        }

        [Fact]
        public async Task ProjectUpload_Add_Project_Repeatedly_Should_Add_One_Project()
        {
            var round = 1;
            var projectId = QuadraticVoteConstants.ProjectOneId;
            await ProjectUploadedAsync(projectId, round);
            var project = await _projectRoundInfosRepository.GetAsync(x => x.ProjectId == projectId);
            var vote = 1000;
            project.Vote = vote;
            await _projectRoundInfosRepository.UpdateAsync(project);

            await ProjectUploadedAsync(projectId, round);
            project = await _projectRoundInfosRepository.GetAsync(x => x.ProjectId == projectId);
            project.Vote.ShouldBe(vote);
        }

        private async Task ProjectUploadedAsync(string projectId, int round)
        {
            var projectUploadedProcessor = GetRequiredService<IEventHandlerTestProcessor<ProjectUploaded>>();
            await projectUploadedProcessor.HandleEventAsync(new ProjectUploaded
            {
                Uploader = new Address(),
                ProjectId = projectId,
                Round = round
            }, new EventContext());
        }
    }
}
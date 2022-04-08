using System;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.Contracts.QuadraticFunding;
using QuadraticVote.ContractEventHandler.Helpers;
using Volo.Abp.Domain.Repositories;
using Project = QuadraticVote.Domain.Entities.Project;

namespace QuadraticVote.ContractEventHandler.Processors
{
    public class ProjectBannedProcessor : AElfEventProcessorBase<ProjectBanned>
    {
        private readonly IRepository<Project> _projectRoundInfosRepository;

        public ProjectBannedProcessor(IRepository<Project> projectRoundInfosRepository)
        {
            _projectRoundInfosRepository = projectRoundInfosRepository;
        }

        protected override async Task HandleEventAsync(ProjectBanned eventDetailsEto, EventContext txInfoDto)
        {
            var projectId = ProjectHelper.ModifyProjectId(eventDetailsEto.Project);
            var projectInfo = await _projectRoundInfosRepository.FindAsync(x =>
                x.RoundNumber == eventDetailsEto.Round && x.ProjectId == projectId);
            if (projectInfo == null)
            {
                throw new Exception(
                    $"Lack project Info in db, project: {projectId}, round: {eventDetailsEto.Round}");
            }

            projectInfo.IsBanned = eventDetailsEto.Ban;
            await _projectRoundInfosRepository.UpdateAsync(projectInfo);
        }
    }
}
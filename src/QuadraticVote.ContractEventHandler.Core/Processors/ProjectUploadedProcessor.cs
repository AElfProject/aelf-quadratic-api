using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.Contracts.QuadraticFunding;
using Volo.Abp.Domain.Repositories;
using Project = QuadraticVote.Domain.Entities.Project;

namespace QuadraticVote.ContractEventHandler.Processors
{
    public class ProjectUploadedProcessor : AElfEventProcessorBase<ProjectUploaded>
    {
        private readonly IRepository<Project> _projectRoundInfosRepository;

        public ProjectUploadedProcessor(IRepository<Project> projectRoundInfosRepository)
        {
            _projectRoundInfosRepository = projectRoundInfosRepository;
        }

        protected override async Task HandleEventAsync(ProjectUploaded eventDetailsEto, EventContext txInfoDto)
        {
            var projectInfo = await _projectRoundInfosRepository.FindAsync(x =>
                x.RoundNumber == eventDetailsEto.Round && x.ProjectId == eventDetailsEto.ProjectId);
            if (projectInfo != null)
            {
                return;
            }
            
            await _projectRoundInfosRepository.InsertAsync(
                new Project(eventDetailsEto.ProjectId, eventDetailsEto.Round), true);
        }
    }
}
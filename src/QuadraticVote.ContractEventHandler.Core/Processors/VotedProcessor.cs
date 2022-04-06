using System;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.Contracts.QuadraticFunding;
using QuadraticVote.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Project = QuadraticVote.Domain.Entities.Project;

namespace QuadraticVote.ContractEventHandler.Processors
{
    public class VotedProcessor : AElfEventProcessorBase<Voted>
    {
        private readonly IRepository<Project> _projectRoundInfosRepository;
        private readonly IRepository<UserProjectInfo> _userProjectInfosRepository;

        public VotedProcessor(IRepository<Project> projectRoundInfosRepository,
            IRepository<UserProjectInfo> userProjectInfosRepository)
        {
            _projectRoundInfosRepository = projectRoundInfosRepository;
            _userProjectInfosRepository = userProjectInfosRepository;
        }

        protected override async Task HandleEventAsync(Voted eventDetailsEto, EventContext txInfoDto)
        {
            var projectInfo = await _projectRoundInfosRepository.FindAsync(x =>
                eventDetailsEto.Round == x.RoundNumber && eventDetailsEto.Project == x.ProjectId);
            if (projectInfo == null)
            {
                throw new Exception(
                    $"Lack project Info in db, project: {eventDetailsEto.Project}, round: {eventDetailsEto.Round}");
            }

            projectInfo.Grant += eventDetailsEto.Grants;
            projectInfo.Vote += eventDetailsEto.Vote;
            projectInfo.SupportArea += eventDetailsEto.SupportArea;
            await _projectRoundInfosRepository.UpdateAsync(projectInfo);

            var user = eventDetailsEto.Account.ToBase58();
            var userInfo = await _userProjectInfosRepository.FindAsync(x =>
                eventDetailsEto.Round == x.RoundNumber && user == x.User &&
                eventDetailsEto.Project == x.ProjectId);
            if (userInfo != null)
            {
                userInfo.Vote += eventDetailsEto.Vote;
                userInfo.Cost += eventDetailsEto.Cost;
                await _userProjectInfosRepository.UpdateAsync(userInfo);
                return;
            }

            await _userProjectInfosRepository.InsertAsync(new UserProjectInfo
            {
                User = user,
                RoundNumber = eventDetailsEto.Round,
                ProjectId = eventDetailsEto.Project,
                Vote = eventDetailsEto.Vote,
                Cost = eventDetailsEto.Cost
            });
        }
    }
}
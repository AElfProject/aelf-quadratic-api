namespace QuadraticVote.ContractEventHandler.Helpers
{
    public static class ProjectHelper
    {
        public static string ModifyProjectId(string projectId)
        {
            return projectId.PadLeft(15, '0');
        }
        
        public static string ModifyProjectId(long projectId)
        {
            return ModifyProjectId(projectId.ToString());
        }
    }
}
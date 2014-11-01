namespace TeamCityBuildMonitor.Models
{
    public class FailedBuild
    {
        public string ProjectName { get; set; }
        public string BuildTypeId { get; set; }
        public string BuildLogUrl { get; set; }

        public bool BeenInvestigated { get; set; }
        public string Assignee { get; set; }
    }
}
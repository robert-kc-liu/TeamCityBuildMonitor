namespace TeamCityBuildMonitor.BuildMonitor
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
        public BuildTypeList BuildTypes { get; set; }

        public bool BuildFailed { get; set; }
    }
}
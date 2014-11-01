namespace TeamCityBuildMonitor.BuildMonitor
{
    public class Build
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string WebUrl { get; set; }
    }
}
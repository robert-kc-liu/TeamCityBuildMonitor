using System.Collections.Generic;

namespace TeamCityBuildMonitor.Models
{
    public class BuildModel
    {
        public BuildState BuildState { get; set; }

        public int ProjectTotal { get; set; }
        public int ProjectFailed { get; set; }

        public int BuildTypeTotal { get; set; }
        public int BuildTypeFailed { get; set; }

        public IEnumerable<FailedBuild> FailedBuilds { get; set; }
    }
}
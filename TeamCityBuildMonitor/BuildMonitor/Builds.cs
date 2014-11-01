using System.Collections.Generic;

namespace TeamCityBuildMonitor.BuildMonitor
{
    public class Builds
    {
        public int Count { get; set; }
        public List<Build> Build { get; set; }
    }
}
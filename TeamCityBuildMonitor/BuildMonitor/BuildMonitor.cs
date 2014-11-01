using System.Collections.Generic;
using System.Linq;
using RestSharp;
using TeamCityBuildMonitor.Models;

namespace TeamCityBuildMonitor.BuildMonitor
{
    public class BuildMonitor
    {
        private readonly RestClient _client;
        private const string Root = "http://[teamcity_server]/guestAuth/app/rest";
        private const string BuildTypeBuildsFormatString = "/buildTypes/id:{0}/builds/";
        private const string InvestigationByBuildTypeFormatString = "investigations/buildType:(id:{0})";

        public BuildMonitor()
        {
            _client = new RestClient(Root);
        }

        public BuildModel GetCurrentBuildModel()
        {
            var request = new RestRequest("projects", Method.GET);
            var response = _client.Execute<ProjectList>(request);

            var data = response.Data;
            var projectResourceRoot = request.Resource;

            var buildModel = new BuildModel();
            var allBuildsOk = true;
            var failedBuilds = new List<FailedBuild>();
            var buildTypeTotal = 0;
            var buildTypeFailed = 0;

            foreach (var projectData in data.Project)
            {
                request.Resource = projectResourceRoot + "/" + projectData.Id;
                var projectResponse = _client.Execute<Project>(request);

                foreach (var buildType in projectResponse.Data.BuildTypes.BuildType)
                {
                    buildTypeTotal++;

                    request.Resource = string.Format(BuildTypeBuildsFormatString, buildType.Id);
                    var buildsResponse = _client.Execute<Builds>(request);
                    
                    var builds = buildsResponse.Data.Build;
                    if (builds != null && builds.Any())
                    {
                        var latestBuild = builds.First();

                        if (latestBuild.Status != "SUCCESS")
                        {
                            allBuildsOk = false;
                            projectData.BuildFailed = true;
                            buildTypeFailed++;

                            request.Resource = string.Format(InvestigationByBuildTypeFormatString, buildType.Id);

                            var investigationResponse = _client.Execute<Investigation>(request);

                            var failedBuild = new FailedBuild
                            {
                                ProjectName = projectData.Name,
                                BuildTypeId = buildType.Id,
                                BuildLogUrl = latestBuild.WebUrl
                            };

                            if (investigationResponse.Data != null)
                            {
                                failedBuild.BeenInvestigated = (investigationResponse.Data.State == "TAKEN");
                                failedBuild.Assignee = investigationResponse.Data.Assignee.Name;
                            }

                            failedBuilds.Add(failedBuild);
                        }
                    }
                }
            }

            buildModel.BuildState = GetBuildState(allBuildsOk, failedBuilds);
            buildModel.FailedBuilds = failedBuilds;
            buildModel.ProjectTotal = data.Project.Count;
            buildModel.BuildTypeTotal = buildTypeTotal;
            buildModel.BuildTypeFailed = buildTypeFailed;
            buildModel.ProjectFailed = data.Project.Count(p => p.BuildFailed);

            return buildModel;
        }

        private BuildState GetBuildState(bool allBuildsOk, IEnumerable<FailedBuild> failedBuilds)
        {
            if (allBuildsOk)
            {
                return BuildState.Ok;
            }

            if (failedBuilds.All(fb => fb.BeenInvestigated))
            {
                return BuildState.FailedWithAllBeenInvestigated;
            }

            return BuildState.Failed;
        }
    }
}
using System.Web.Mvc;

namespace TeamCityBuildMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly BuildMonitor.BuildMonitor _buildMonitor;

        public HomeController(BuildMonitor.BuildMonitor buildMonitor)
        {
            _buildMonitor = buildMonitor;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult GetBuildStatus()
        {
            var buildModel = _buildMonitor.GetCurrentBuildModel();

            return Json(buildModel);
        }
    }
}
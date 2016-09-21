using Newtonsoft.Json;
using paas_demo.Models;
using System;
using System.Configuration;
using System.Net;
using System.Web.Mvc;

namespace paas_demo.Controllers
{
    public class DashboardsController : Controller
    {
        private readonly string baseUri;

        public DashboardsController()
        {
            this.baseUri = ConfigurationManager.AppSettings["baseUri"];
        }

        public ActionResult Index()
        {
            if (Session["Accesstoken"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else return GetDashboards();
        }

        public ActionResult GetDashboards()
        {
            Dashboards dashboards;

            //Configure dashboards request
            WebRequest request = WebRequest.Create(String.Format("{0}Dashboards", baseUri)) as HttpWebRequest;

            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", Session["AccessToken"].ToString()));

            //Get dashboards response from request.GetResponse()
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                //Get reader from response stream
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    //Deserialize JSON string
                    dashboards = JsonConvert.DeserializeObject<Dashboards>(reader.ReadToEnd());
                }
            }

            var viewModel = new DashboardsViewModel
            {
                Dashboards = dashboards.value
            };

            return View(viewModel);
        }

        public ActionResult Tiles(string dashboardId, string dashboardName)
        {
            if (Session["Accesstoken"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else return GetTiles(dashboardId, dashboardName);
        }

        protected ActionResult GetTiles(string dashboardId, string dashboardName)
        {
            Tiles tiles;
            //Configure tiles request
            WebRequest request = WebRequest.Create(
                String.Format("{0}Dashboards/{1}/Tiles",
                baseUri,
                dashboardId)) as HttpWebRequest;

            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", Session["AccessToken"].ToString()));

            //Get tiles response from request.GetResponse()
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                //Get reader from response stream
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    //Deserialize JSON string
                    tiles = JsonConvert.DeserializeObject<Tiles>(reader.ReadToEnd());
                }
            }

            var viewModel = new TilesViewModel
            {
                Tiles = tiles.value,
                DashboardName = dashboardName,
                DashboardId = dashboardId,
                AccessToken = Session["AccessToken"].ToString()
            };

            return View(viewModel);
        }
    }
}
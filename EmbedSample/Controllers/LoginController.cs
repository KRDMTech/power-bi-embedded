using Microsoft.IdentityModel.Clients.ActiveDirectory;
using paas_demo.Models;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace paas_demo.Controllers
{
    public class LoginController : Controller
    {
        public LoginController()
        {
        }

        public ActionResult Index()
        {
            var code = Request.QueryString["code"];

            if (!string.IsNullOrEmpty(code))
            {
                // Get auth token from auth code       
                TokenCache TC = new TokenCache();

                //Values are hard-coded for sample purposes
                string authority = ConfigurationManager.AppSettings["authUri"];
                AuthenticationContext AC = new AuthenticationContext(authority, TC);
                ClientCredential cc = new ClientCredential(ConfigurationManager.AppSettings["clientId"], ConfigurationManager.AppSettings["clientSecret"]);
                var AR = AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri(ConfigurationManager.AppSettings["redirectUri"]), cc);

                Session["AccessToken"] = AR.Result.AccessToken;
            }
            else if (Session["Accesstoken"] == null)
            {
                var viewModel = new LoginPageViewModel();

                return View(viewModel);
            }

            return RedirectToAction("Index", "Dashboards");
        }

        public ActionResult Logout()
        {
            Session["AccessToken"] = null;
            Session["Role"] = null;
            Session["User"] = null;
            Session["Email"] = null;

            return RedirectToAction("Index", "Login");
        }

        public ActionResult AppLogin(string UserName)
        {
            switch (UserName)
            {
                case "Carl":
                    Session["Role"] = "Carl";
                    Session["User"] = "Carl McDonald";
                    Session["Email"] = "carl.mcdonald@imail.org";
                    break;
                case "Dale":
                    Session["Role"] = "Dale";
                    Session["User"] = "Dale Forbes";
                    Session["Email"] = "dale.forbes@imail.org";
                    break;
            }

            return RedirectToAction("Index", "Report");
        }

        [HttpPost]
        public void GetAuthorizationCode()
        {
            var @params = new NameValueCollection
            {
                //Azure AD will return an authorization code.
                {"response_type", ConfigurationManager.AppSettings["responseType"]},

                //Client ID is used by the application to identify themselves to the users that they are requesting permissions from.
                //You get the client id when you register your Azure app.
                {"client_id", ConfigurationManager.AppSettings["clientId"]},

                //Resource uri to the Power BI resource to be authorized
                //The resource uri is hard-coded for sample purposes
                {"resource", ConfigurationManager.AppSettings["resource"]},

                //After app authenticates, Azure AD will redirect back to the web app. In this sample, Azure AD redirects back
                //to Default page (Default.aspx).
                { "redirect_uri", ConfigurationManager.AppSettings["redirectUri"]},
            };
            //Create sign-in query string
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(@params);

            //Redirect to Azure AD to get an authorization code
            Response.Redirect(String.Format(ConfigurationManager.AppSettings["authUri"] + "?{0}", queryString));
        }
    }
}
using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using PatientPortal.Data.EF;

namespace PatientPortal.Web.AuthHelpers 
{
    public static class AuthHelpers 
    {

        public static bool IsBhdKey
        {
            get
            {
                var apiCookie = HttpContext.Current.Request.Cookies["apikey"];
                var compareApiKey = WebConfigurationManager.AppSettings["BhdApiKey"];
                if (apiCookie != null && apiCookie.Value == compareApiKey)
                {
                    return true;
                }
                return false;
            }

        } 


        public static bool isAuthenticated 
        {
            get {
                HttpCookie myCookie = HttpContext.Current.Request.Cookies["BHD-COOKIE"];
                if (myCookie != null) 
                {
                    string clientTokenID = myCookie["token"];
                    if (!string.IsNullOrEmpty(clientTokenID)) 
                    {
                        using (var db = new BHDPortalsEntities()) 
                        {
                            if (db.usp_VerifyPatientByTokenID_prr(clientTokenID).FirstOrDefault() != null)
                            {
                                myCookie.Expires = DateTime.Now.AddDays(-1);
                                HttpContext.Current.Response.Cookies.Add(myCookie);
                               myCookie.Expires = DateTime.Now.AddMinutes(10.0);
                                HttpContext.Current.Response.Cookies.Add(myCookie);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }
    }



    public class BhdApiAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!AuthHelpers.IsBhdKey)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult("Default",
                    new System.Web.Routing.RouteValueDictionary {
                        {"controller", "Account"},
                        {"action", "Login"},
                        {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                    });
            }
        }
    }

  

    public class CookieAuthAttribute : FilterAttribute, IAuthenticationFilter 
    {
        public void OnAuthentication(AuthenticationContext filterContext) 
        {
            if (!AuthHelpers.isAuthenticated) 
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

      

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) 
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult) 
            {
                filterContext.Result = new RedirectToRouteResult("Default", 
                    new System.Web.Routing.RouteValueDictionary {
                        {"controller", "Account"},
                        {"action", "Login"},
                        {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                    });
            }
        }
    }
}
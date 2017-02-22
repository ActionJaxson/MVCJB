using System;
using System.Web;
using System.Web.Configuration;

namespace PatientPortal.Web.AuthHelpers
{
    public class SessionHelper
    {
        
        public static bool IsMobile()
        {

            return HttpContext.Current.Session["isMobileApp"] != null ? true : false;
        }


        public static string IsMedHelp()
        {
            var apiKeySess = "";
            try
            {
                apiKeySess = HttpContext.Current.Session["apikey"].ToString();
            }
            catch (NullReferenceException)
            {

                return "false";
            }
          
            if (string.IsNullOrEmpty(apiKeySess))
            {
                return "false";
            }
            var apiKey = WebConfigurationManager.AppSettings["ApiKey"];
            var bhdApiKey = WebConfigurationManager.AppSettings["BhdReportKey"];
            var bhdMobKey = WebConfigurationManager.AppSettings["BhdApiKey"];
           
            if (apiKeySess == apiKey)
            {
                return "mh";
            }
            if (apiKeySess == bhdMobKey)
            {
                return "bmk";
            }
           
           
            return apiKeySess == bhdApiKey ? "bhd" : "false";
        }

        public static bool IsPatientOverride()
        {
            return HttpContext.Current.Session["patientOverride"] != null ? true : false;
        }
    }
}
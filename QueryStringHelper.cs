using System;
using System.Linq;
using System.Text;
using System.Web;

namespace PatientPortal.Web.AuthHelpers
{
    public class QueryStringHelper
    {

        public static bool HasEmptyKey(string key)
        {
            return HttpContext.Current.Request.QueryString.GetValues(null) != null && HttpContext.Current.Request.QueryString.GetValues(null).Contains(key);
        }

        public static string GetApiKey(string key)
        {
            var apiKey = HttpContext.Current.Request.QueryString.GetValues(key);
            return apiKey != null ? apiKey[0] : null;
        }


        public static bool IsPatientOverride(string key)
        {
            if (HttpContext.Current != null)
            {
                if( HttpContext.Current.Request.QueryString.GetValues(key) != null )
                {
                    return Convert.ToBoolean(HttpContext.Current.Request.QueryString[key]);
                }
            }

            return false;
        }


        public static string ProcessReturnUrl(string qs, string controll, ref string action)
        {
            if (!String.IsNullOrEmpty(qs))
            {
                string[] myArray = qs.Split('/');
                var sb = new StringBuilder();
                var flag = true;
                for (var i = 0; i < myArray.Length; i++)
                {
                    var indexPos = myArray.Length - 3;

                    if (indexPos < i)
                    {
                        sb.Append(myArray[i]);
                        if (flag)
                        {
                            sb.Append('/');
                        }
                        flag = false;
                    }
                }

                string myString = sb.ToString();
                string[] queryArray = myString.Split('/');
                controll = queryArray[0];
                var actionquery = queryArray[1];
                string[] q = actionquery.Split('?');
                 action = q[0];

            }
            return controll;
        }

        public static string GetPatientID(string key)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.QueryString.GetValues(key) != null)
                {
                    return HttpContext.Current.Request.QueryString[key];
                }
            }

            return "";
        }


    }
}
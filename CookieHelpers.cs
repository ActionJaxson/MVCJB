using System;
using System.Web;

namespace PatientPortal.Web.AuthHelpers {
    public static class CookieHelpers 
    {
        //private const string aesPassword = "37899888-8B9B-4E45-9961-702A8D6DF438";
        //private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public static void SetAuthCookie(string token, bool rememberMe) 
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["BHD-COOKIE"] ?? new HttpCookie("BHD-COOKIE");
            myCookie.Values["token"] = token;
           
           // myCookie.Expires = DateTime.Now.AddDays(1 + (rememberMe ? 6 : 0));
            myCookie.Expires = DateTime.Now.AddMinutes(4.00);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }



        public static void ClearAuthCookie() 
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["BHD-COOKIE"] ?? new HttpCookie("BHD-COOKIE");
            myCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public static string GetAuthToken() 
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["BHD-COOKIE"];
            if (myCookie != null) 
            {
                return myCookie["token"];
            }
            return null;
        }

        public static bool GetLockoutValue()
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["regLockout"];
            if (myCookie != null)
            {
                return Convert.ToBoolean(myCookie.Value);
            }
            return false;
        }

        public static int GetRegisterAttemptValue()
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["attempt"];
            if(myCookie != null)
            {
                return Convert.ToInt32(myCookie.Value);
            }
            return 0;
        }

        public static void LockoutCheck()
        {

            if (System.Web.HttpContext.Current.Request.Cookies["attempt"] == null)
            {
                HttpCookie myCookie = new HttpCookie("attempt");
                myCookie.Value = 1.ToString();
                myCookie.Expires = DateTime.Now.AddDays(12);
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else if (Convert.ToInt32(System.Web.HttpContext.Current.Request.Cookies["attempt"].Value) <= 2)
            {
                int count = Convert.ToInt32(HttpContext.Current.Request.Cookies["attempt"].Value);
                count = count + 1;
                System.Web.HttpContext.Current.Response.Cookies["attempt"].Value = count.ToString();
                if (count == 3)
                {
                    // Is this an iPad from the pilot program or an internal employee?
                    if (System.Web.HttpContext.Current.Request.Cookies["iPadSettings"] != null ||
                        System.Web.HttpContext.Current.Request.UserHostAddress.Contains("192.168."))
                    {
                        DateTime expire = DateTime.Now.AddDays(-1);
                        HttpCookie regLockout = new HttpCookie("regLockout");
                        regLockout.Value = "false";
                        regLockout.Expires = expire;
                        System.Web.HttpContext.Current.Response.Cookies.Add(regLockout);
                        System.Web.HttpContext.Current.Response.Cookies["attempt"].Expires = expire;
                    }
                    else
                    {
                        DateTime expire = DateTime.Now.AddHours(24);
                        HttpCookie regLockout = new HttpCookie("regLockout");
                        regLockout.Value = "true";
                        regLockout.Expires = expire;
                        System.Web.HttpContext.Current.Response.Cookies.Add(regLockout);
                        System.Web.HttpContext.Current.Response.Cookies["attempt"].Expires = expire;
                    }
                }
            }
        }

        //public static CookieModel TokenData {
        //    get {
        //        try {
        //            HttpCookie myCookie = HttpContext.Current.Request.Cookies["c"];
        //            if (myCookie != null) {
        //                byte[] encVal = Convert.FromBase64String(myCookie.Values["in"]);
        //                byte[] tmpPass = Encoding.ASCII.GetBytes(aesPassword);

        //                byte[] decryptedImpersonatedName = AESUtils.AES_Decrypt(encVal, tmpPass);

        //                string decryptedData = Encoding.ASCII.GetString(decryptedImpersonatedName);
        //                string[] split = decryptedData.Split('~');

        //                if (split.Length != 3) {
        //                    return null;
        //                }
        //                if (split[0] != HttpContext.Current.User.Identity.Name) {
        //                    return null;
        //                }

        //                return split[1];
        //            }
        //        }
        //        catch {
        //            return string.Empty;
        //        }

        //        return string.Empty;
        //    }
        //    set {
        //        // Get a random 64-bit number
        //        byte[] randomBytes = new byte[8];
        //        rngCsp.GetBytes(randomBytes);

        //        // Create the cookie value
        //        string cookieVal = HttpContext.Current.User.Identity.Name + "~" + value + "~" + BitConverter.ToInt64(randomBytes, 0);
        //        byte[] cookieArr = Encoding.ASCII.GetBytes(cookieVal);
        //        byte[] tmpPass = Encoding.ASCII.GetBytes(aesPassword);

        //        HttpCookie myCookie = HttpContext.Current.Request.Cookies["c"] ?? new HttpCookie("c");
        //        // Encrypt the value and convert to base64
        //        myCookie.Values["in"] = Convert.ToBase64String(AESUtils.AES_Encrypt(cookieArr, tmpPass));
        //        myCookie.Expires = DateTime.Now.AddDays(1);
        //        HttpContext.Current.Response.Cookies.Add(myCookie);
        //    }
        //}
    }
}
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PatientPortal.Web.AuthHelpers {
    public class SessionData {
        const string ClientIdKey = "ClientId";
        private const string ImpersonatedUserName = "ImpersonatedUserName";
        private const string PasswordTries = "ForgotPasswordAttempts";
        
        private const string aesPassword = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public static int ClientId {
            get {
                try {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies["c"];
                    if (myCookie != null) {
                        byte[] encVal = Convert.FromBase64String(myCookie.Values["cc"]);
                        byte[] tmpPass = Encoding.ASCII.GetBytes(aesPassword);

                        byte[] decryptedClientID = AESUtils.AES_Decrypt(encVal, tmpPass);

                        string decryptedData = Encoding.ASCII.GetString(decryptedClientID);
                        string[] split = decryptedData.Split('~');

                        if (split.Length != 3) {
                            return 0;
                        }
                        if (split[0] != HttpContext.Current.User.Identity.Name) {
                            return 0;
                        }

                        return Convert.ToInt32(split[1]);
                    }
                }
                catch {
                    return 0;
                }

                return 0;
            }
            set {
                // Get a random 64-bit number
                byte[] randomBytes = new byte[8];
                rngCsp.GetBytes(randomBytes);

                // Create the cookie value
                string cookieVal = HttpContext.Current.User.Identity.Name + "~" + value + "~" + BitConverter.ToInt64(randomBytes, 0);
                byte[] cookieArr = Encoding.ASCII.GetBytes(cookieVal);
                byte[] tmpPass = Encoding.ASCII.GetBytes(aesPassword);

                HttpCookie myCookie = HttpContext.Current.Request.Cookies["c"] ?? new HttpCookie("c");
                // Encrypt the value and convert to base64
                myCookie.Values["cc"] = Convert.ToBase64String(AESUtils.AES_Encrypt(cookieArr, tmpPass));
                myCookie.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public static string UserName {
            get {
                try {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies["c"];
                    if (myCookie != null) {
                        byte[] encVal = Convert.FromBase64String(myCookie.Values["in"]);
                        byte[] tmpPass = Encoding.ASCII.GetBytes(aesPassword);

                        byte[] decryptedImpersonatedName = AESUtils.AES_Decrypt(encVal, tmpPass);

                        string decryptedData = Encoding.ASCII.GetString(decryptedImpersonatedName);
                        string[] split = decryptedData.Split('~');

                        if (split.Length != 3) {
                            return string.Empty;
                        }
                        if (split[0] != HttpContext.Current.User.Identity.Name) {
                            return string.Empty;
                        }

                        return split[1];
                    }
                }
                catch {
                    return string.Empty;
                }

                return string.Empty;
            }
            set {
                // Get a random 64-bit number
                byte[] randomBytes = new byte[8];
                rngCsp.GetBytes(randomBytes);

                // Create the cookie value
                string cookieVal = HttpContext.Current.User.Identity.Name + "~" + value + "~" + BitConverter.ToInt64(randomBytes, 0);
                byte[] cookieArr = Encoding.ASCII.GetBytes(cookieVal);
                byte[] tmpPass = Encoding.ASCII.GetBytes(aesPassword);

                HttpCookie myCookie = HttpContext.Current.Request.Cookies["c"] ?? new HttpCookie("c");
                // Encrypt the value and convert to base64
                myCookie.Values["in"] = Convert.ToBase64String(AESUtils.AES_Encrypt(cookieArr, tmpPass));
                myCookie.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public static int forgotPasswordAttempts {
            get { return HttpContext.Current.Session[PasswordTries] != null ? (int)HttpContext.Current.Session[PasswordTries] : 0; }
            set {
                HttpContext.Current.Session[PasswordTries] = value;
            }
        }
    }

    //public class UserInfo {
    //    //private static string _fullname = "";

    //    public static string fullname {
    //        get {
    //            using (var context = new HCPPortalsEntities()) {
    //                return context.GetHCPName(HttpContext.Current.User.Identity.Name).FirstOrDefault();
    //            }
    //        }

    //    }
    //}


}

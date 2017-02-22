//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using System.Web.Configuration;
//using System.Web.SessionState;

//namespace BHDPortalsMaterializeMVC.Helpers
//{
//    public static class TamperProofQs
//    {
//        private const string QUERYSTRING_VALIDATION_NAME = "qtpval";
//        private const string QUERYSTRING_VALIDATION_NAME_WITH_SEP = "&" + QUERYSTRING_VALIDATION_NAME + "=";

//        private static string ComputeHash(string data)
//        {
//            //Add some "entropy" by also using the ASP Session Id
//            HttpSessionState oSession = HttpContext.Current.Session;
//            data += oSession.SessionID;
//            oSession["A"] = 5;  //Set a variable or the session id will change...

//            //Get bytes from plaintext
//            byte[] plaintextBytes = Encoding.UTF8.GetBytes(data);

//            //Read the key to use from the web.config
//            HMACSHA1 hashAlg = new HMACSHA1();

//            //Don't want to just use hashing because attacker can guess the alorithm, modify the data, and 
//            // create own hash that will match
//            //SHA1 hashAlg = new SHA1Managed();

//            byte[] hash = hashAlg.ComputeHash(plaintextBytes);

//            return conversions.ByteArrayToHex(hash);
//        }


//        public static string HashQueryString(string queryString)
//        {
//            // Adds the &qtpval=<hash value> to the querystring
//            return queryString + QUERYSTRING_VALIDATION_NAME_WITH_SEP + ComputeHash(queryString);
//        }


//        public static void ValidateQueryString()
//        {
//            HttpRequest request = HttpContext.Current.Request;

//            //If no querystring is present, nothing to validate
//            if (0 == request.QueryString.Count)
//            {
//                return;
//            }

//            //Get the entire querystring (minus initial ?)
//            string queryString = request.Url.Query.TrimStart(new char[] { '?' });

//            //Get just our hash value from the querystring collection, if none present throw exception
//            string submittedHash = request.QueryString[QUERYSTRING_VALIDATION_NAME];
//            if (null == submittedHash)
//            {
//                throw new ApplicationException("Querystring validation hash was not sent!");
//            }

//            //Take the original querystring and get all of it except our hash (we need to recompute the hash
//            // just like it was done on the original querystring)
//            int hashPos = queryString.IndexOf(QUERYSTRING_VALIDATION_NAME_WITH_SEP);
//            queryString = queryString.Substring(0, hashPos);

//            //If the hash that was sent on the querystring does not match our compute of that hash given the 
//            // current data in the querystring, then throw an exception
//            if (ComputeHash(queryString) != submittedHash)
//            {
//                throw new ApplicationException("Querystring hash values don't match");
//            }
//        }

//    }
//}
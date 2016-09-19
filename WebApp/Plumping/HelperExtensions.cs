using System;
using System.Configuration;
using System.Web;
namespace WebApp.Plumping {
    public static class SSHelper {
        /****************************************************************************************
         * All Razor helper functions that supports the web pages
        /* 
        /* Author: Kelvin   Update: 2013/07/15 
         * ***************************************************************************************/
        //gets absolute base url for a web page, so all the js and css can be referenced properly
        public static string AbsoluteBaseUrl() {
            var a = HttpContext.Current.Request.Url.Scheme;
            var b = HttpContext.Current.Request.Url.Authority; //with port number
            var c = HttpRuntime.AppDomainAppVirtualPath;
            var d = string.Format("{0}://{1}{2}", a, b, c);
            return d.EndsWith("/") ? d : d + "/";
        }

        public static string AbsoluteBaseUrlNoPort() {
            var a = HttpContext.Current.Request.Url.Scheme;
            var b = HttpContext.Current.Request.Url.Host; //with port number
            var c = HttpRuntime.AppDomainAppVirtualPath;
            var d = string.Format("{0}://{1}{2}", a, b, c);
            return d.EndsWith("/") ? d : d + "/";
        }

        public static bool IsEarlyBird() {
            return DateTime.Compare(DateTime.Now, DateTime.Parse(ConfigurationManager.AppSettings["earlybird_endtime"])) < 0;
        }
    }
}
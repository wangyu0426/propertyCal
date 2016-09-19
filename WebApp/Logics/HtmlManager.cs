using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ServiceStack.Text;

namespace WebApp.Logics {
    public interface IHtmlManager {
        string GetHtml(string url);
    }
    public class HtmlManager : LogicalBase, IHtmlManager {
        public string GetHtml(string url) { 
            var request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/plain";
            var bs = request.GetResponse().GetResponseStream().ReadFully();
            var str = System.Text.Encoding.ASCII.GetString(bs);
            return str;
        }
    }
}
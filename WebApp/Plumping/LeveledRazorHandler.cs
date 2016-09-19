/****************************************************************************************
/* change the behaviour when 401, 403, 404, 500...
/* instead of going to root 'Page Not Found' page, it checks the current folder first.
/* then it checks the parent folder level by level, until it hits the root directory.
/* 
/* Author: Kelvin   Update: 2013/07/15
/* Author:          Update:
/* Author:          Update:
 * ***************************************************************************************/
using System.IO;
using System.Net;
using ServiceStack;
using ServiceStack.Common.Web;
using ServiceStack.Razor;
using ServiceStack.Razor.Managers;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints.Support;
namespace WebApp.Plumping {
    public class LeveledRazorHandler : EndpointHandlerBase {
        public RazorFormat RazorFormat { get; set; }
        public RazorPage RazorPage { get; set; }
        public object Model { get; set; }
        public string PathInfo { get; set; }
        public LeveledRazorHandler(string pathInfo) {
            PathInfo = pathInfo;
        }
        public override void ProcessRequest(IHttpRequest httpReq, IHttpResponse httpRes, string operationName) {
            httpRes.ContentType = ContentType.Html;
            if (RazorFormat == null)
                RazorFormat = RazorFormat.Instance;
            //RazorFormat.FindByPathInfo(path); 
            var contentPage = RazorPage ?? FindHandlerPage(httpReq.PathInfo);
            if (contentPage == null) {
                httpRes.StatusCode = (int)HttpStatusCode.NotFound;
                httpRes.EndHttpHandlerRequest();
                return;
            }
            var model = Model;
            if (model == null)
                httpReq.Items.TryGetValue("Model", out model);
            if (model == null) {
                var modelType = RazorPage != null ? RazorPage.ModelType : null;
                model = modelType == null || modelType == typeof(DynamicRequestObject)
                            ? null
                            : DeserializeHttpRequest(modelType, httpReq, httpReq.ContentType);
            }
            RazorFormat.ProcessRazorPage(httpReq, contentPage, model, httpRes);
        }
        private RazorPage FindHandlerPage(string path) {
            var dir = (Path.GetDirectoryName(path) ?? "\\").Replace("\\", "/");
            var rooted = (dir == "/");
            if (rooted) dir = "";
            var file = dir + PathInfo;
            var res = RazorFormat.FindByPathInfo(file);
            if (!rooted && res == null) {
                var parent = dir.Substring(0, dir.LastIndexOf("/") + 1);
                return FindHandlerPage(parent);
            }
            return res;
        }
        public override object CreateRequest(IHttpRequest request, string operationName) {
            return null;
        }
        public override object GetResponse(IHttpRequest httpReq, IHttpResponse httpRes, object request) {
            return null;
        }
    }
}
using System.Configuration;
using System.Linq;
using Funq;
using ServiceStack.Common;
using ServiceStack.Common.Utils;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack.ServiceHost;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using WebApp.Logics;
using WebApp.Services;
namespace WebApp {
    public class AppHost : AppHostBase {

        public ILogManager Log { get; set; }

        public AppHost() : base("", typeof(LogService).Assembly) { }
        private void PluginRazorFormat() {
            Plugins.Add(new RazorFormat());
        }
        private void RegisterInterfaces(Container container) {
            var logicalTypes = typeof(ILogicalBase).Assembly.GetTypes().Where(type =>
                type.IsClass && !type.IsAbstract && type.GetInterface("I" + type.Name) != null);
            foreach (var type in logicalTypes) { //register all interfaces
                container.RegisterAutoWiredType(type, type.GetInterface("I" + type.Name), ReuseScope.None);
            }
        }
        public override void Configure(Container container) {
            PluginRazorFormat();
            RegisterInterfaces(container);
            Log = container.Resolve<ILogManager>();
            ServiceExceptionHandler = ((httpReq, httpRes, ex) => {
                //log your exceptions here 
                dynamic error = new {Request = httpReq.FormData.ToString(), ErrorMsg = ex.Message};
                Log.Write("ServerError", error);
                return DtoUtils.CreateErrorResponse(httpReq, ex, ex.ToResponseStatus());
            });
            //handler
            SetConfig(new EndpointHostConfig {
                EnableFeatures = Feature.All.Remove(Feature.Jsv | Feature.Soap | Feature.Csv), // | Feature.Xml),
                DefaultContentType = ContentType.Json,
                AllowFileExtensions = { { "swf" },{ "ics" } },
                //MetadataOperationPageBodyHtml = "",
                //MetadataPageBodyHtml = "",
                //MetadataRedirectPath = "/default",
                //DefaultRedirectPath = "/default",
                //CustomHttpHandlers = {
                //    {HttpStatusCode.NotFound, new LeveledRazorHandler("/notfound")},
                //    {HttpStatusCode.Unauthorized, new LeveledRazorHandler("/unauthorized")},
                //    {HttpStatusCode.Forbidden, new LeveledRazorHandler("/forbidden")},
                //    {HttpStatusCode.InternalServerError, new LeveledRazorHandler("/servererror")}
                //}
            });
            JsConfig.ExcludeTypeInfo = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            JsConfig.EmitCamelCaseNames = true;
        }
    }
}
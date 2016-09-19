using ServiceStack.ServiceInterface;
using WebApp.Logics;
namespace WebApp.Services {
    public class ServiceBase : Service {
        public ILogManager Log { get; set; }
        public ILogManager LogManager { get; set; }
        public IHtmlManager HtmlManager { get; set; }
    }
}
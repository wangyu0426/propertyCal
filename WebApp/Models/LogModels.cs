using System.Collections.Generic;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
namespace WebApp.Models {
    [Route("/api/log")]
    [Route("/api/log/{Type}")]
    [Route("/api/log/{Type}/{Page}")]
    public class LogDto{
        public string Type { get; set; }
        public int? Page { get; set; }
    }

    public class LogDtoResponse : IHasResponseStatus {
        public IList<LogRecord> Logs { get; set; } 
        public ResponseStatus ResponseStatus { get; set; } //string error code and message if any, otherwise success
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace WebApp.Models {

    [Route("/api/html")]
    public class HtmlDto : IHasResponseStatus {
        public string Url { get; set; }
        public string Html { get; set; }
        public string StatusCode { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
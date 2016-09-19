using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Data.ModelExtensions;
using WebApp.Models;

namespace WebApp.Services {
    public class HtmlService : ServiceBase {
        public HtmlDto Any(HtmlDto dto) {
            dto.Html = HtmlManager.GetHtml(dto.Url);
            return dto;
        }
    }
}
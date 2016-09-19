using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models {
    public static class Business {
        public static string ModelName = "Model1";
        public static string DatabaseConnstr = "server=INTEL36;database=HLJ;Trusted_Connection=True;";
        public static string EntityFrameworkConnStr = "metadata=res://*/Models." + ModelName + ".csdl|res://*/Models." + ModelName + ".ssdl|res://*/Models." + ModelName + 
                                                      ".msl;provider=System.Data.SqlClient;provider connection string=\"" + DatabaseConnstr + ";MultipleActiveResultSets=True;App=EntityFramework\"";
    }
}
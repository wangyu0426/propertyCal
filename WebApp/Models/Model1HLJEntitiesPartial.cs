using System.Configuration;
using System.Data.Entity;

namespace WebApp.Models {
    public partial class HLJEntities {
        public HLJEntities(string connstr)
            : base(connstr) {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public HLJEntities(string connstr, bool create)
            : base(connstr) {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            if (create) Database.Create();
        }
    }
}
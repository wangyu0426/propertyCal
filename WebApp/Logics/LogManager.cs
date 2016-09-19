using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceStack;
using ServiceStack.Text;
using WebApp.Models;
using WebApp.Models;

namespace WebApp.Logics {
    public interface ILogManager {
        IList<LogRecord> GetList(string type, int? page); 
        void Write(string title, string msg);
        void Write(string title, object msg);
        void Write(string title, Exception msg);
    }
    public class LogManager : ILogManager {
        public IList<LogRecord> GetList(string type, int? page) {
            List<LogRecord> list = null;
            using (var conn = new HLJEntities(Business.EntityFrameworkConnStr)) {
                var data = conn.Set<LogRecord>().OrderByDescending(x=>x.Id).AsQueryable();
                if (type != null) 
                    data = data.Where(x => x.Title.Equals(type,StringComparison.OrdinalIgnoreCase));
                if (page != null) 
                    data = data.Skip((page.Value-1)*50);     
                list = data.Take(50).ToList();
                foreach (var item in list) {
                    conn.Entry(item).State = EntityState.Detached;
                }
            }
            return list;
        }
        public void Write(string title, string msg) {
            var rec = new LogRecord() {Title = title, Message = msg, Date = DateTime.UtcNow};
            Write(rec);
        }
        public void Write(string title, object msg) {
            var rec = new LogRecord() {Title = title, Message = msg.ToJson(), Date = DateTime.UtcNow};
            Write(rec);
        }
        public void Write(string title, Exception msg) {
            var rec = new LogRecord() {Title = title, Message = msg.Message, Date = DateTime.UtcNow};
            Write(rec);
        }
        private void Write(LogRecord rec) {
            using (var conn = new HLJEntities(Business.EntityFrameworkConnStr)) {
                conn.Set<LogRecord>().Add(rec);
                conn.SaveChanges();
            }
        }
    }
}
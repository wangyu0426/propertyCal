using System.Collections.Generic;
using WebApp.Models;
namespace WebApp.Data.ModelExtensions {
    public static class LogModelExtensions {
        public static LogDtoResponse ToDto(this IList<LogRecord> rec) {
            return new LogDtoResponse(){Logs = rec};
        }
    }
}
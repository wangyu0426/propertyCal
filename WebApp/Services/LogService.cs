using WebApp.Data.ModelExtensions;
using WebApp.Models;
namespace WebApp.Services {
    public class LogService : ServiceBase {
        public LogDtoResponse Any(LogDto dto) {
            return LogManager.GetList(dto.Type,dto.Page).ToDto();
        }
    }
}
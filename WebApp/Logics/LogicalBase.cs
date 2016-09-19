namespace WebApp.Logics {
    public interface ILogicalBase {
        ILogManager Log { get; set; }
    }
    public abstract class LogicalBase {
        public ILogManager Log { get; set; }
    }
}
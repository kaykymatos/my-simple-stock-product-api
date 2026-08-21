namespace MySimpleStockProduct.Logging
{
    public interface ICustomLogger<T>
    {
        void LogInfo(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
    }
}

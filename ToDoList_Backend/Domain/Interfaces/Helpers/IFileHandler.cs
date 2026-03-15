namespace Domain.Interfaces.Helpers
{
    public interface IFileHandler
    {
        void SaveData<T>(string filePath, T data);
        List<T> ReadData<T>(string filePath);
        void OverwriteData<T>(string filePath, List<T> data);
        Task SaveDataAsync<T>(string filePath, List<T> data);
        Task OverwriteDataAsync<T>(string filePath, List<T> data);
        Task<List<T>> ReadDataAsync<T>(string filePath);
    }
}
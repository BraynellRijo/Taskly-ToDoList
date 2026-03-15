using System.Text.Json;
using Domain.ENUM;
using DAL.PathHandler;
using Domain.Interfaces.Helpers;

namespace DAL.FileHandler
{
    public class JsonFileHandler : IFileHandler
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly PersistencePathHandler _pathManager;
        private const FileType FILE_TYPE = FileType.Json;

        public JsonFileHandler()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            _pathManager = new PersistencePathHandler();
        }

        private string GetAndEnsurePath(string fileName)
        {
            string filePath = _pathManager.GetPersistenceFilePath(fileName, FILE_TYPE);
            _pathManager.VerifyDirectoryExists(filePath);
            return filePath;
        }
        private List<T> DeserializeJson<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return [];

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? [];
            }
            catch (JsonException)
            {
                var single = JsonSerializer.Deserialize<T>(json, _jsonOptions);
                return single is not null ? [single] : [];
            }
        }


        public async Task<List<T>> ReadDataAsync<T>(string fileName)
        {
            string filePath = GetAndEnsurePath(fileName);
            if (!File.Exists(filePath)) return [];

            string json = await File.ReadAllTextAsync(filePath);
            return DeserializeJson<T>(json);
        }

        public async Task SaveDataAsync<T>(string fileName, List<T> data)
        {
            ArgumentNullException.ThrowIfNull(data);

            try
            {
                List<T> existing = await ReadDataAsync<T>(fileName);
                existing.AddRange(data);

                string filePath = GetAndEnsurePath(fileName);
                string jsonString = JsonSerializer.Serialize(existing, _jsonOptions);
                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex) when (ex is not ArgumentNullException)
            {
                throw new IOException($"Error saving file '{fileName}': {ex.Message}", ex);
            }
        }

        public async Task OverwriteDataAsync<T>(string fileName, List<T> data)
        {
            ArgumentNullException.ThrowIfNull(data);
            string filePath = GetAndEnsurePath(fileName);
            string json = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }

        public List<T> ReadData<T>(string fileName)
        {
            string filePath = GetAndEnsurePath(fileName);
            if (!File.Exists(filePath)) return [];

            string json = File.ReadAllText(filePath);
            return DeserializeJson<T>(json);
        }

        public void SaveData<T>(string fileName, T data)
        {
            ArgumentNullException.ThrowIfNull(data);

            try
            {
                List<T> existing = ReadData<T>(fileName);
                existing.Add(data);

                string filePath = GetAndEnsurePath(fileName);
                File.WriteAllText(filePath, JsonSerializer.Serialize(existing, _jsonOptions));
            }
            catch (Exception ex) when (ex is not ArgumentNullException)
            {
                throw new IOException($"Error saving file '{fileName}': {ex.Message}", ex);
            }
        }

        public void OverwriteData<T>(string fileName, List<T> data)
        {
            ArgumentNullException.ThrowIfNull(data);
            string filePath = GetAndEnsurePath(fileName);
            File.WriteAllText(filePath, JsonSerializer.Serialize(data, _jsonOptions));
        }
    }
}
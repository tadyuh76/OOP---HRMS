using System.Text.Json;

namespace HRManagementSystem
{
    public class JsonFileStorage : IFileStorage
    {
        private readonly JsonSerializerOptions _options;

        public JsonFileStorage()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public bool SaveData<T>(string filename, T data)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            try
            {
                string jsonString = JsonSerializer.Serialize(data, _options);
                File.WriteAllText(filename, jsonString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public T LoadData<T>(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            if (!File.Exists(filename))
            {
                return default;
            }

            string jsonString = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<T>(jsonString, _options);
        }
    }
}
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
                string directory = Path.GetDirectoryName(filename);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string jsonString = JsonSerializer.Serialize(data, _options);
                File.WriteAllText(filename, jsonString);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lưu dữ liệu: {ex.Message}");
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
            try
            {
                string jsonString = File.ReadAllText(filename);
                return JsonSerializer.Deserialize<T>(jsonString, _options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc dữ liệu: {ex.Message}");
                return default;
            }

        }
        public void SavePayrollData(List<Payroll> payrolls, string payrollFilePath)
        {
            if (payrolls == null)
            {
                throw new ArgumentNullException(nameof(payrolls));
            }
            SaveData<List<Payroll>>(payrollFilePath, payrolls);
        }

        // Phương thức bổ sung để đọc danh sách Payroll
        public List<Payroll> LoadPayrollData(string payrollFilePath)
        {
            var result = LoadData<List<Payroll>>(payrollFilePath);
            return result ?? new List<Payroll>();
        }

        // Phương thức bổ sung để lưu danh sách Employee
        public void SaveEmployeeData(List<Employee> employees, string employeeFilePath)
        {
            if (employees == null)
            {
                throw new ArgumentNullException(nameof(employees));
            }
            SaveData<List<Employee>>(employeeFilePath, employees);
        }

        // Phương thức bổ sung để đọc danh sách Employee
        public List<Employee> LoadEmployeeData(string employeeFilePath)
        {
            var result = LoadData<List<Employee>>(employeeFilePath);
            return result ?? new List<Employee>();
        }
        
    }
}
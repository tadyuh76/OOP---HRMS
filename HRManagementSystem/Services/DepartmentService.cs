using System.Text.Json;

namespace HRManagementSystem
{
    public class DepartmentService
    {
        private readonly string _filePath;

        public DepartmentService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Departments.json");
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Department>();
            }

            string json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Department>>(json) ?? new List<Department>();
        }

        public async Task<Department> GetDepartmentByIdAsync(string id)
        {
            var departments = await GetAllDepartmentsAsync();
            return departments.FirstOrDefault(d => d.DepartmentId == id);
        }

        public async Task<bool> AddDepartmentAsync(Department department)
        {
            var departments = await GetAllDepartmentsAsync();

            // Check if department with same ID already exists
            if (departments.Any(d => d.DepartmentId == department.DepartmentId))
            {
                return false;
            }

            departments.Add(department);
            await SaveDepartmentsAsync(departments);
            return true;
        }

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            var departments = await GetAllDepartmentsAsync();
            int index = departments.FindIndex(d => d.DepartmentId == department.DepartmentId);

            if (index == -1)
            {
                return false;
            }

            departments[index] = department;
            await SaveDepartmentsAsync(departments);
            return true;
        }

        public async Task<bool> DeleteDepartmentAsync(string departmentId)
        {
            var departments = await GetAllDepartmentsAsync();
            int count = departments.RemoveAll(d => d.DepartmentId == departmentId);

            if (count == 0)
            {
                return false;
            }

            await SaveDepartmentsAsync(departments);
            return true;
        }

        private async Task SaveDepartmentsAsync(List<Department> departments)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(departments, options);

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<string> GenerateNewDepartmentId()
        {
            var departments = await GetAllDepartmentsAsync();

            if (!departments.Any())
            {
                return "DEP001";
            }

            var lastId = departments.Max(d => d.DepartmentId);
            if (int.TryParse(lastId.Substring(3), out int id))
            {
                return $"DEP{(id + 1):D3}";
            }

            return "DEP001";
        }
    }
}

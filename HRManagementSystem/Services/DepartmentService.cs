using System.Text.Json;

namespace HRManagementSystem
{
    public class DepartmentService : IService<Department>
    {
        private readonly FileManager _fileManager;
        private List<Department> _departments;

        // Singleton instance
        private static DepartmentService _instance;

        // Default constructor that doesn't require FileManager
        public DepartmentService()
        {
            _fileManager = null;

            try
            {
                // Try to load departments directly from the JSON file
                if (File.Exists(FileManager.departmentDataPath))
                {
                    JsonFileStorage storage = new JsonFileStorage();
                    _departments = storage.LoadData<List<Department>>(FileManager.departmentDataPath) ?? new List<Department>();
                }
                else
                {
                    _departments = new List<Department>();
                }
            }
            catch
            {
                // If anything goes wrong, initialize with an empty list
                _departments = new List<Department>();
            }
        }

        public DepartmentService(FileManager fileManager)
        {
            _fileManager = fileManager;
            _departments = _fileManager?.LoadDepartments() ?? new List<Department>();
        }

        // Singleton pattern
        public static DepartmentService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DepartmentService();
            }
            return _instance;
        }

        public List<Department> GetAll()
        {
            return _departments;
        }

        public Department GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return _departments.FirstOrDefault(d => d.DepartmentId == id);
        }

        public bool Add(Department entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _departments.Add(entity);
            return SaveChanges();
        }

        public bool Update(Department entity)
        {
            if (entity == null || string.IsNullOrEmpty(entity.DepartmentId))
                throw new ArgumentNullException(nameof(entity));

            int index = _departments.FindIndex(d => d.DepartmentId == entity.DepartmentId);
            if (index == -1)
                return false;

            _departments[index] = entity;
            return SaveChanges();
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            int index = _departments.FindIndex(d => d.DepartmentId == id);
            if (index == -1)
                return false;

            _departments.RemoveAt(index);
            return SaveChanges();
        }

        private bool SaveChanges()
        {
            // If FileManager is not available, just return success without saving
            if (_fileManager == null)
                return true;

            return _fileManager.SaveDepartments(_departments);
        }

        public string GenerateNewDepartmentId()
        {
            if (!_departments.Any())
            {
                return "DEP001";
            }

            string? lastId = _departments.Max(d => d.DepartmentId);
            if (int.TryParse(lastId.Substring(3), out int id))
            {
                return $"DEP{(id + 1):D3}";
            }

            return "DEP001";
        }
    }
}

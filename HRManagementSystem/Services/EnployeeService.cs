namespace HRManagementSystem
{
    public class EmployeeService : IService<Employee>
    {
        private readonly FileManager _fileManager;
        private List<Employee> _employees;

        // Singleton instance for cases where FileManager isn't available
        private static EmployeeService _instance;

        // Default constructor that doesn't require FileManager
        public EmployeeService()
        {
            _fileManager = null;

            try
            {
                // Try to load employees directly from the JSON file
                if (File.Exists(FileManager.employeeDataPath))
                {
                    JsonFileStorage storage = new JsonFileStorage();
                    _employees = storage.LoadData<List<Employee>>(FileManager.employeeDataPath) ?? new List<Employee>();
                }
                else
                {
                    _employees = new List<Employee>();
                }
            }
            catch
            {
                // If anything goes wrong, initialize with an empty list
                _employees = new List<Employee>();
            }
        }

        public EmployeeService(FileManager fileManager)
        {
            _fileManager = fileManager;
            _employees = _fileManager?.LoadEmployees() ?? new List<Employee>();
        }

        // Singleton pattern to ensure there's always at least one instance available
        public static EmployeeService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EmployeeService();
            }
            return _instance;
        }

        // Method to manually set employees (useful for testing or when FileManager not available)
        public void SetEmployees(List<Employee> employees)
        {
            _employees = employees ?? new List<Employee>();
        }

        public List<Employee> GetAll()
        {
            // Load department data to populate department names
            DepartmentService departmentService = DepartmentService.GetInstance();
            List<Department> departments = departmentService.GetAll();

            // Associate department names with employees
            foreach (Employee employee in _employees)
            {
                foreach (Department department in departments)
                {
                    if (department.DepartmentId == employee.DepartmentId)
                    {
                        employee.DepartmentName = department.Name;
                        break;
                    }
                }
            }

            return _employees;
        }

        public Employee GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            foreach (Employee e in _employees)
            {
                if (e.Id == id)
                {
                    return e;
                }
            }

            return null;
        }

        public bool Add(Employee entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            _employees.Add(entity);
            return SaveChanges();
        }

        public bool Update(Employee entity)
        {
            if (entity == null || string.IsNullOrEmpty(entity.Id))
                throw new ArgumentNullException(nameof(entity));

            int index = -1;
            for (int i = 0; i < _employees.Count; i++)
            {
                if (_employees[i].Id == entity.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                return false;

            _employees[index] = entity;
            return SaveChanges();
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            int index = -1;
            for (int i = 0; i < _employees.Count; i++)
            {
                if (_employees[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                return false;

            _employees.RemoveAt(index);
            return SaveChanges();
        }

        private bool SaveChanges()
        {
            // If FileManager is not available, just return success without saving
            if (_fileManager == null)
                return true;

            return _fileManager.SaveEmployees(_employees);
        }
    }
}

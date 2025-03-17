namespace HRManagementSystem
{
    public class FileManager
    {
        private readonly IFileStorage _fileStorage;

        public FileManager(IFileStorage storage)
        {
            _fileStorage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public List<Employee> LoadEmployees()
        {
            return _fileStorage.LoadData<List<Employee>>("employees.json") ?? new List<Employee>();
        }

        public bool SaveEmployees(List<Employee> employees)
        {
            if (employees == null)
            {
                throw new ArgumentNullException(nameof(employees));
            }
            return _fileStorage.SaveData("employees.json", employees);
        }

        public List<Department> LoadDepartments()
        {
            return _fileStorage.LoadData<List<Department>>("departments.json") ?? new List<Department>();
        }

        public bool SaveDepartments(List<Department> departments)
        {
            if (departments == null)
            {
                throw new ArgumentNullException(nameof(departments));
            }
            return _fileStorage.SaveData("departments.json", departments);
        }

        public List<Attendance> LoadAttendances()
        {
            return _fileStorage.LoadData<List<Attendance>>("attendances.json") ?? new List<Attendance>();
        }

        public bool SaveAttendances(List<Attendance> attendances)
        {
            if (attendances == null)
            {
                throw new ArgumentNullException(nameof(attendances));
            }
            return _fileStorage.SaveData("attendances.json", attendances);
        }

        public List<Leave> LoadLeaves()
        {
            return _fileStorage.LoadData<List<Leave>>("leaves.json") ?? new List<Leave>();
        }

        public bool SaveLeaves(List<Leave> leaves)
        {
            if (leaves == null)
            {
                throw new ArgumentNullException(nameof(leaves));
            }
            return _fileStorage.SaveData("leaves.json", leaves);
        }

        public List<Payroll> LoadPayrolls()
        {
            return _fileStorage.LoadData<List<Payroll>>("payrolls.json") ?? new List<Payroll>();
        }

        public bool SavePayrolls(List<Payroll> payrolls)
        {
            if (payrolls == null)
            {
                throw new ArgumentNullException(nameof(payrolls));
            }
            return _fileStorage.SaveData("payrolls.json", payrolls);
        }
    }

}
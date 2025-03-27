namespace HRManagementSystem
{
    public class FileManager
    {
        private readonly IFileStorage _fileStorage;
        public static string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
        public static string dataDirectory = Path.Combine(projectDirectory, "Data");
        public static string employeeDataPath = Path.Combine(dataDirectory, "Employees.json");
        public static string departmentDataPath = Path.Combine(dataDirectory, "Departments.json");
        public static string attendanceDataPath = Path.Combine(dataDirectory, "Attendances.json");
        public static string leaveDataPath = Path.Combine(dataDirectory, "Leaves.json");
        public static string payrollDataPath = Path.Combine(dataDirectory, "Payroll.json");
        public FileManager(IFileStorage storage)
        {
            _fileStorage = storage ?? throw new ArgumentNullException(nameof(storage));
            // Đảm bảo thư mục Data tồn tại
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
        }

        public List<Employee> LoadEmployees()
        {
            return _fileStorage.LoadData<List<Employee>>(employeeDataPath) ?? new List<Employee>();
        }

        public bool SaveEmployees(List<Employee> employees)
        {
            if (employees == null)
            {
                throw new ArgumentNullException(nameof(employees));
            }
            return _fileStorage.SaveData(employeeDataPath, employees);
        }

        public List<Department> LoadDepartments()
        {
            return _fileStorage.LoadData<List<Department>>(departmentDataPath) ?? new List<Department>();
        }

        public bool SaveDepartments(List<Department> departments)
        {
            if (departments == null)
            {
                throw new ArgumentNullException(nameof(departments));
            }
            return _fileStorage.SaveData(departmentDataPath, departments);
        }

        public List<Attendance> LoadAttendances()
        {
            return _fileStorage.LoadData<List<Attendance>>(attendanceDataPath) ?? new List<Attendance>();
        }

        public bool SaveAttendances(List<Attendance> attendances)
        {
            if (attendances == null)
            {
                throw new ArgumentNullException(nameof(attendances));
            }
            return _fileStorage.SaveData(attendanceDataPath, attendances);
        }

        public List<Leave> LoadLeaves()
        {
            return _fileStorage.LoadData<List<Leave>>(leaveDataPath) ?? new List<Leave>();
        }

        public bool SaveLeaves(List<Leave> leaves)
        {
            if (leaves == null)
            {
                throw new ArgumentNullException(nameof(leaves));
            }
            return _fileStorage.SaveData(leaveDataPath, leaves);
        }

        public List<Payroll> LoadPayrolls()
        {
            return _fileStorage.LoadData<List<Payroll>>(payrollDataPath) ?? new List<Payroll>();
        }

        public bool SavePayrolls(List<Payroll> payrolls)
        {
            if (payrolls == null)
            {
                throw new ArgumentNullException(nameof(payrolls));
            }
            return _fileStorage.SaveData(payrollDataPath, payrolls);
        }

        public List<Payroll> LoadPayrollsByEmployeeId(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            List<Payroll> allPayrolls = LoadPayrolls();
            return allPayrolls.FindAll(p => p.EmployeeId == employeeId);
        }
    }

}
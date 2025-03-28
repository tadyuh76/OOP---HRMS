namespace HRManagementSystem.Services
{
    public class AttendanceService
    {
        private readonly FileManager _fileManager;
        private List<Attendance> attendances;

        // Singleton instance for cases where FileManager isn't available
        private static AttendanceService _instance;

        // Default constructor that doesn't require FileManager
        public AttendanceService()
        {
            _fileManager = null;

            try
            {
                // Try to load attendances directly from the JSON file
                if (File.Exists(FileManager.attendanceDataPath))
                {
                    JsonFileStorage storage = new JsonFileStorage();
                    attendances = storage.LoadData<List<Attendance>>(FileManager.attendanceDataPath) ?? new List<Attendance>();
                }
                else
                {
                    attendances = new List<Attendance>();
                }
            }
            catch
            {
                // If anything goes wrong, initialize with an empty list
                attendances = new List<Attendance>();
            }
        }

        public AttendanceService(FileManager fileManager)
        {
            _fileManager = fileManager;
            attendances = _fileManager?.LoadAttendances() ?? new List<Attendance>();
        }

        // Singleton pattern to ensure there's always at least one instance available
        public static AttendanceService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AttendanceService();
            }
            return _instance;
        }

        public Attendance RecordAttendance(string employeeId, string employeeName, AttendanceStatus status)
        {
            // Remove employee validation since we're using employeeName directly
            // var employeeService = EmployeeService.GetInstance();
            // var employee = employeeService.GetById(employeeId);
            // if (employee == null)
            // {
            //     throw new EntityNotFoundException("Employee not found.");
            // }

            // Check if attendance for today already exists
            var existingAttendance = attendances
                .FirstOrDefault(a => a.EmployeeId == employeeId && a.Date.Date == DateTime.Today);

            if (existingAttendance != null)
            {
                throw new HRSystemException("Attendance already recorded for today.");
            }

            // Create new attendance record
            var attendance = new Attendance
            {
                AttendanceId = $"ATT{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}",
                EmployeeId = employeeId,
                EmployeeName = employeeName, // Use the provided employee name
                Date = DateTime.Today,
                ClockInTime = DateTime.Now,
                ClockOutTime = DateTime.MinValue, // Will be updated when clocking out
                Status = status,
                Employee = null // No longer setting the Employee object
            };

            attendances.Add(attendance);
            SaveChanges();

            return attendance;
        }

        public void UpdateClockOut(string attendanceId)
        {
            var attendance = attendances.FirstOrDefault(a => a.AttendanceId == attendanceId);
            if (attendance == null)
            {
                throw new EntityNotFoundException("Attendance record not found.");
            }

            attendance.ClockOutTime = DateTime.Now;
            SaveChanges();
        }

        public List<Attendance> GetMonthlyAttendance(int year, int month)
        {
            return attendances
                .Where(a => a.Date.Year == year && a.Date.Month == month)
                .ToList();
        }

        public Dictionary<string, int> GetAttendanceSummary(int year, int month)
        {
            var monthlyAttendance = GetMonthlyAttendance(year, month);

            return monthlyAttendance
                .GroupBy(a => a.Status)
                .ToDictionary(
                    group => group.Key.ToString(),
                    group => group.Count()
                );
        }

        public List<Attendance> GetEmployeeAttendance(string employeeId, int year, int month)
        {
            return attendances
                .Where(a => a.EmployeeId == employeeId &&
                            a.Date.Year == year &&
                            a.Date.Month == month)
                .ToList();
        }

        private bool SaveChanges()
        {
            // If FileManager is not available, just return success without saving
            if (_fileManager == null)
            {
                // Fallback to saving directly with JsonFileStorage
                try
                {
                    JsonFileStorage storage = new JsonFileStorage();
                    return storage.SaveData(FileManager.attendanceDataPath, attendances);
                }
                catch
                {
                    return false;
                }
            }

            return _fileManager.SaveAttendances(attendances);
        }
    }
}
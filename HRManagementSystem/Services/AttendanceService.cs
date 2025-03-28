namespace HRManagementSystem.Services
{
    public class AttendanceService
    {
        private readonly FileManager _fileManager;
        private List<Attendance> attendances;

        // Singleton instance for cases where FileManager isn't available
        private static AttendanceService _instance;

        // Company working hours
        private static readonly TimeSpan workStartTime = new TimeSpan(9, 0, 0); // 9:00 AM
        private static readonly TimeSpan workEndTime = new TimeSpan(17, 30, 0); // 5:30 PM

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
            // Check if attendance for today already exists
            var existingAttendance = attendances
                .FirstOrDefault(a => a.EmployeeId == employeeId && a.Date.Date == DateTime.Today);

            if (existingAttendance != null)
            {
                throw new HRSystemException("Attendance already recorded for today.");
            }

            // Automatically set status to Late if clocking in after official start time
            var currentTime = DateTime.Now;
            if (status == AttendanceStatus.Present && currentTime.TimeOfDay > workStartTime)
            {
                status = AttendanceStatus.Late;
            }

            // Create new attendance record with consistent ID format: ATT followed by 3 digits
            // First determine highest existing ID number
            int maxId = 0;
            foreach (var atd in attendances)
            {
                if (atd.AttendanceId.StartsWith("ATT") && atd.AttendanceId.Length >= 5)
                {
                    if (int.TryParse(atd.AttendanceId.Substring(3), out int idNum))
                    {
                        maxId = Math.Max(maxId, idNum);
                    }
                }
            }

            // Create a consistent ID format
            string newId = $"ATT{(maxId + 1):D3}";

            // Create new attendance record
            var attendance = new Attendance
            {
                AttendanceId = newId,
                EmployeeId = employeeId,
                EmployeeName = employeeName,
                Date = DateTime.Today,
                ClockInTime = currentTime,
                ClockOutTime = DateTime.MinValue,
                Status = status,
                Employee = null
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

        public List<Attendance> GetDailyAttendance(DateTime date)
        {
            return attendances
                .Where(a => a.Date.Date == date.Date)
                .ToList();
        }

        public List<Attendance> GetEmployeeDailyAttendance(string employeeId, DateTime date)
        {
            return attendances
                .Where(a => a.EmployeeId == employeeId && a.Date.Date == date.Date)
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

        public static TimeSpan GetWorkStartTime()
        {
            return workStartTime;
        }

        public static TimeSpan GetWorkEndTime()
        {
            return workEndTime;
        }
    }
}
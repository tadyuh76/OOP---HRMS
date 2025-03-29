namespace HRManagementSystem
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
            // Replace lambda with explicit loop
            Attendance? existingAttendance = null;
            foreach (Attendance a in attendances)
            {
                if (a.EmployeeId == employeeId && a.Date.Date == DateTime.Today)
                {
                    existingAttendance = a;
                    break;
                }
            }

            if (existingAttendance != null)
            {
                throw new HRSystemException("Attendance already recorded for today.");
            }

            // Automatically set status to Late if clocking in after official start time
            DateTime currentTime = DateTime.Now;
            if (status == AttendanceStatus.Present && currentTime.TimeOfDay > workStartTime)
            {
                status = AttendanceStatus.Late;
            }

            // Create new attendance record with consistent ID format: ATT followed by 3 digits
            // First determine highest existing ID number
            int maxId = 0;
            foreach (Attendance atd in attendances)
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
            Attendance attendance = new Attendance
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
            Attendance? attendance = null;
            foreach (Attendance a in attendances)
            {
                if (a.AttendanceId == attendanceId)
                {
                    attendance = a;
                    break;
                }
            }

            if (attendance == null)
            {
                throw new EntityNotFoundException("Attendance record not found.");
            }

            attendance.ClockOutTime = DateTime.Now;
            SaveChanges();

            // After updating clock out time, check if this is a contract employee
            // and update their hours worked
            UpdateContractEmployeeHours(attendance);
        }

        private void UpdateContractEmployeeHours(Attendance attendance)
        {
            if (attendance == null || attendance.ClockInTime == DateTime.MinValue ||
                attendance.ClockOutTime == DateTime.MinValue)
                return;

            try
            {
                // Get the employee service
                EmployeeService employeeService = EmployeeService.GetInstance();
                List<Employee>? employees = employeeService.GetAll();

                if (employees == null)
                    return;

                // Find the employee by their ID
                Employee? employee = null;
                foreach (Employee e in employees)
                {
                    if (e.EmployeeId == attendance.EmployeeId)
                    {
                        employee = e;
                        break;
                    }
                }

                if (employee == null)
                    return;

                // Check if this is a contract employee by checking the EmployeeType property
                if (employee.EmployeeType == "Contract")
                {
                    // Calculate the hours worked
                    TimeSpan hoursWorked = attendance.ClockOutTime - attendance.ClockInTime;

                    // Convert to decimal hours
                    decimal hours = (decimal)hoursWorked.TotalHours;

                    // Update the employee's hours worked if they have the ContractEmployee properties
                    if (employee is ContractEmployee contractEmployee)
                    {
                        contractEmployee.HoursWorked += hours;
                        employeeService.Update(contractEmployee);
                    }
                    else
                    {
                        // If the employee is stored as a base class but marked as Contract type,
                        // try to access the property through reflection or dynamic
                        Type employeeType = employee.GetType();
                        System.Reflection.PropertyInfo? hoursWorkedProperty = employeeType.GetProperty("HoursWorked");

                        if (hoursWorkedProperty != null)
                        {
                            decimal currentHours = (decimal)hoursWorkedProperty.GetValue(employee, null);
                            hoursWorkedProperty.SetValue(employee, currentHours + hours);
                            employeeService.Update(employee);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Log the exception but don't rethrow - this is a secondary operation
                // that shouldn't prevent clock-out if it fails
            }
        }

        public List<Attendance> GetMonthlyAttendance(int year, int month)
        {
            List<Attendance> result = new List<Attendance>();
            foreach (Attendance a in attendances)
            {
                if (a.Date.Year == year && a.Date.Month == month)
                {
                    result.Add(a);
                }
            }
            return result;
        }

        public Dictionary<string, int> GetAttendanceSummary(int year, int month)
        {
            List<Attendance> monthlyAttendance = GetMonthlyAttendance(year, month);
            Dictionary<string, int> summary = new Dictionary<string, int>();

            foreach (Attendance a in monthlyAttendance)
            {
                string key = a.Status.ToString();
                if (!summary.ContainsKey(key))
                {
                    summary[key] = 0;
                }
                summary[key]++;
            }

            return summary;
        }

        public List<Attendance> GetEmployeeAttendance(string employeeId, int year, int month)
        {
            List<Attendance> result = new List<Attendance>();
            foreach (Attendance a in attendances)
            {
                if (a.EmployeeId == employeeId && a.Date.Year == year && a.Date.Month == month)
                {
                    result.Add(a);
                }
            }
            return result;
        }

        public List<Attendance> GetDailyAttendance(DateTime date)
        {
            List<Attendance> result = new List<Attendance>();
            foreach (Attendance a in attendances)
            {
                if (a.Date.Date == date.Date)
                {
                    result.Add(a);
                }
            }
            return result;
        }

        public List<Attendance> GetEmployeeDailyAttendance(string employeeId, DateTime date)
        {
            List<Attendance> result = new List<Attendance>();
            foreach (Attendance a in attendances)
            {
                if (a.EmployeeId == employeeId && a.Date.Date == date.Date)
                {
                    result.Add(a);
                }
            }
            return result;
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
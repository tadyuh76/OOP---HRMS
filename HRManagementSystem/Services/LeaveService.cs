namespace HRManagementSystem
{
    public class LeaveService
    {
        private readonly FileManager _fileManager;
        private List<LeaveRequest> leaveRequests;

        // Singleton instance for cases where FileManager isn't available
        private static LeaveService _instance;

        // Default constructor that doesn't require FileManager
        public LeaveService()
        {
            _fileManager = null;

            try
            {
                // Try to load leave data directly from the JSON files
                JsonFileStorage storage = new JsonFileStorage();

                if (File.Exists(FileManager.leaveDataPath))
                {
                    leaveRequests = storage.LoadData<List<LeaveRequest>>(FileManager.leaveDataPath) ?? new List<LeaveRequest>();
                }
                else
                {
                    leaveRequests = new List<LeaveRequest>();
                }
            }
            catch
            {
                // If anything goes wrong, initialize with an empty list
                leaveRequests = new List<LeaveRequest>();
            }
        }

        public LeaveService(FileManager fileManager)
        {
            _fileManager = fileManager;
            leaveRequests = _fileManager?.LoadLeaveRequests() ?? new List<LeaveRequest>();
        }

        // Singleton pattern to ensure there's always at least one instance available
        public static LeaveService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LeaveService();
            }
            return _instance;
        }

        public LeaveRequest SubmitLeaveRequest(
            string employeeId,
            DateTime startDate,
            DateTime endDate,
            LeaveType leaveType,
            string remarks)
        {
            // Validate employee by EmployeeId field instead of Id field
            EmployeeService employeeService = EmployeeService.GetInstance();
            List<Employee> employees = employeeService.GetAll();
            Employee? employee = null;
            foreach (Employee e in employees)
            {
                if (e.EmployeeId == employeeId)
                {
                    employee = e;
                    break;
                }
            }

            if (employee == null)
            {
                throw new EntityNotFoundException($"Employee with ID {employeeId} not found.");
            }

            // Validate date range
            if (startDate > endDate)
            {
                throw new ValidationException("Start date must be before or equal to end date.");
            }

            // Create Leave Request with a consistent ID format
            string requestId = GenerateLeaveRequestId();

            LeaveRequest leaveRequest = new LeaveRequest
            {
                RequestId = requestId,
                EmployeeId = employeeId,
                EmployeeName = employee.Name,
                RequestDate = DateTime.Now,
                StartDate = startDate,
                EndDate = endDate,
                Type = leaveType,
                Status = LeaveStatus.Pending,
                Remarks = remarks,
                Employee = employee,
                ApproverId = null
            };

            leaveRequests.Add(leaveRequest);
            SaveChanges();

            return leaveRequest;
        }

        // Helper method to generate consistent leave request IDs
        private string GenerateLeaveRequestId()
        {
            // Find the highest existing ID number
            int maxId = 0;
            foreach (LeaveRequest request in leaveRequests)
            {
                if (request.RequestId != null && request.RequestId.StartsWith("LVE") && request.RequestId.Length > 3)
                {
                    if (int.TryParse(request.RequestId.Substring(3), out int idNum))
                    {
                        maxId = Math.Max(maxId, idNum);
                    }
                }
            }

            // Create a new ID with the next sequential number, formatted as 3 digits
            return $"LVE{(maxId + 1):D3}";
        }

        public List<LeaveRequest> GetPendingLeaveRequests()
        {
            List<LeaveRequest> result = new List<LeaveRequest>();
            foreach (LeaveRequest lr in leaveRequests)
            {
                if (IsPendingLeaveRequest(lr))
                {
                    result.Add(lr);
                }
            }
            return result;
        }

        private bool IsPendingLeaveRequest(LeaveRequest lr)
        {
            return lr.Status == LeaveStatus.Pending;
        }

        public LeaveRequest ApproveLeaveRequest(string requestId, string approverId)
        {
            LeaveRequest? leaveRequest = null;
            foreach (LeaveRequest lr in leaveRequests)
            {
                if (IsMatchingRequestId(lr, requestId))
                {
                    leaveRequest = lr;
                    break;
                }
            }

            if (leaveRequest == null)
            {
                throw new EntityNotFoundException("Leave request not found.");
            }

            leaveRequest.Approve(approverId);
            SaveChanges();

            return leaveRequest;
        }

        private bool IsMatchingRequestId(LeaveRequest lr, string requestId)
        {
            return lr.RequestId == requestId;
        }

        public LeaveRequest RejectLeaveRequest(string requestId, string approverId, string rejectionReason)
        {
            LeaveRequest? leaveRequest = null;
            foreach (LeaveRequest lr in leaveRequests)
            {
                if (IsMatchingRequestId(lr, requestId))
                {
                    leaveRequest = lr;
                    break;
                }
            }

            if (leaveRequest == null)
            {
                throw new EntityNotFoundException("Leave request not found.");
            }

            leaveRequest.Reject(approverId, rejectionReason);
            SaveChanges();

            return leaveRequest;
        }

        public List<LeaveRequest> GetEmployeeLeaves(string employeeId)
        {
            List<LeaveRequest> result = new List<LeaveRequest>();
            foreach (LeaveRequest l in leaveRequests)
            {
                if (IsMatchingEmployeeId(l, employeeId))
                {
                    result.Add(l);
                }
            }
            return result;
        }

        private bool IsMatchingEmployeeId(LeaveRequest l, string employeeId)
        {
            return l.EmployeeId == employeeId;
        }

        public Dictionary<LeaveType, int> GetLeaveTypesSummary(string employeeId)
        {
            Dictionary<LeaveType, int> leaveSummary = new Dictionary<LeaveType, int>();

            foreach (LeaveRequest leaveRequest in leaveRequests)
            {
                if (leaveRequest.EmployeeId == employeeId && leaveRequest.Status == LeaveStatus.Approved)
                {
                    if (!leaveSummary.ContainsKey(leaveRequest.Type))
                    {
                        leaveSummary[leaveRequest.Type] = 0;
                    }

                    leaveSummary[leaveRequest.Type] += leaveRequest.CalculateDays();
                }
            }

            return leaveSummary;
        }

        public List<LeaveRequest> GetMonthlyLeaves(int year, int month)
        {
            List<LeaveRequest> result = new List<LeaveRequest>();
            foreach (LeaveRequest l in leaveRequests)
            {
                if (IsLeaveInMonth(l, year, month))
                {
                    result.Add(l);
                }
            }
            return result;
        }

        private bool IsLeaveInMonth(LeaveRequest l, int year, int month)
        {
            return (l.StartDate.Year == year && l.StartDate.Month == month) ||  // Leave starts in target month
                   (l.EndDate.Year == year && l.EndDate.Month == month) ||      // Leave ends in target month
                   (l.StartDate < new DateTime(year, month, 1) &&
                    l.EndDate >= new DateTime(year, month, 1));                 // Leave spans over target month
        }

        public List<LeaveRequest> GetDailyLeaves(DateTime date)
        {
            List<LeaveRequest> result = new List<LeaveRequest>();
            foreach (LeaveRequest l in leaveRequests)
            {
                if (IsLeaveOnDate(l, date))
                {
                    result.Add(l);
                }
            }
            return result;
        }

        private bool IsLeaveOnDate(LeaveRequest l, DateTime date)
        {
            return date.Date >= l.StartDate.Date &&
                   date.Date <= l.EndDate.Date;
        }

        public List<LeaveRequest> GetEmployeeMonthlyLeaves(string employeeId, int year, int month)
        {
            List<LeaveRequest> result = new List<LeaveRequest>();
            foreach (LeaveRequest l in leaveRequests)
            {
                if (IsEmployeeLeaveInMonth(l, employeeId, year, month))
                {
                    result.Add(l);
                }
            }
            return result;
        }

        private bool IsEmployeeLeaveInMonth(LeaveRequest l, string employeeId, int year, int month)
        {
            return l.EmployeeId == employeeId &&
                   ((l.StartDate.Year == year && l.StartDate.Month == month) ||  // Leave starts in target month
                    (l.EndDate.Year == year && l.EndDate.Month == month) ||      // Leave ends in target month
                    (l.StartDate < new DateTime(year, month, 1) &&
                     l.EndDate >= new DateTime(year, month, 1)));                // Leave spans over target month
        }
        public List<LeaveRequest> GetEmployeeDailyLeaves(string employeeId, DateTime date)
        {
            List<LeaveRequest> result = new List<LeaveRequest>();
            foreach (LeaveRequest l in leaveRequests)
            {
                if (IsEmployeeLeaveOnDate(l, employeeId, date))
                {
                    result.Add(l);
                }
            }
            return result;
        }

        private bool IsEmployeeLeaveOnDate(LeaveRequest l, string employeeId, DateTime date)
        {
            return l.EmployeeId == employeeId &&
                   date.Date >= l.StartDate.Date &&
                   date.Date <= l.EndDate.Date;
        }

        private bool SaveChanges()
        {
            bool success = true;

            // If FileManager is available, use it for saving
            if (_fileManager != null)
            {
                success = _fileManager.SaveLeaveRequests(leaveRequests);
                return success;
            }

            // Fallback to direct file saving when FileManager is not available
            try
            {
                JsonFileStorage storage = new JsonFileStorage();
                success = storage.SaveData(FileManager.leaveDataPath, leaveRequests);
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
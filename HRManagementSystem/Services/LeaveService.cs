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
            Employee? employee = employees.FirstOrDefault(e => e.EmployeeId == employeeId);

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
            return leaveRequests
                .Where(lr => lr.Status == LeaveStatus.Pending)
                .ToList();
        }

        public LeaveRequest ApproveLeaveRequest(string requestId, string approverId)
        {
            LeaveRequest? leaveRequest = leaveRequests
                .FirstOrDefault(lr => lr.RequestId == requestId);

            if (leaveRequest == null)
            {
                throw new EntityNotFoundException("Leave request not found.");
            }

            leaveRequest.Approve(approverId);
            SaveChanges();

            return leaveRequest;
        }

        public LeaveRequest RejectLeaveRequest(string requestId, string approverId, string rejectionReason)
        {
            LeaveRequest? leaveRequest = leaveRequests
                .FirstOrDefault(lr => lr.RequestId == requestId);

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
            return leaveRequests
                .Where(l => l.EmployeeId == employeeId)
                .ToList();
        }

        public Dictionary<LeaveType, int> GetLeaveTypesSummary(string employeeId)
        {
            return leaveRequests
                .Where(l => l.EmployeeId == employeeId && l.Status == LeaveStatus.Approved)
                .GroupBy(l => l.Type)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(l => l.CalculateDays())
                );
        }

        public List<LeaveRequest> GetAllLeaves()
        {
            return leaveRequests.ToList();
        }

        public List<LeaveRequest> GetMonthlyLeaves(int year, int month)
        {
            // Returns all leaves that overlap with the specified month
            return leaveRequests
                .Where(l =>
                    (l.StartDate.Year == year && l.StartDate.Month == month) ||  // Leave starts in target month
                    (l.EndDate.Year == year && l.EndDate.Month == month) ||      // Leave ends in target month
                    (l.StartDate < new DateTime(year, month, 1) &&
                     l.EndDate >= new DateTime(year, month, 1))                  // Leave spans over target month
                )
                .ToList();
        }

        public List<LeaveRequest> GetDailyLeaves(DateTime date)
        {
            // Returns all leaves that include the specified date
            return leaveRequests
                .Where(l =>
                    date.Date >= l.StartDate.Date &&
                    date.Date <= l.EndDate.Date
                )
                .ToList();
        }

        public List<LeaveRequest> GetEmployeeMonthlyLeaves(string employeeId, int year, int month)
        {
            // Returns all leaves for an employee that overlap with the specified month
            return leaveRequests
                .Where(l => l.EmployeeId == employeeId &&
                    ((l.StartDate.Year == year && l.StartDate.Month == month) ||  // Leave starts in target month
                     (l.EndDate.Year == year && l.EndDate.Month == month) ||      // Leave ends in target month
                     (l.StartDate < new DateTime(year, month, 1) &&
                      l.EndDate >= new DateTime(year, month, 1)))                 // Leave spans over target month
                )
                .ToList();
        }

        public List<LeaveRequest> GetEmployeeDailyLeaves(string employeeId, DateTime date)
        {
            // Returns all leaves for an employee that include the specified date
            return leaveRequests
                .Where(l => l.EmployeeId == employeeId &&
                    date.Date >= l.StartDate.Date &&
                    date.Date <= l.EndDate.Date
                )
                .ToList();
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
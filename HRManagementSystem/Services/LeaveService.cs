namespace HRManagementSystem.Services
{
    public class LeaveService
    {
        private readonly FileManager _fileManager;
        private List<LeaveRequest> leaveRequests;
        private List<Leave> leaves;

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
                    leaves = storage.LoadData<List<Leave>>(FileManager.leaveDataPath) ?? new List<Leave>();
                }
                else
                {
                    leaves = new List<Leave>();
                }

                string requestsPath = Path.Combine(FileManager.dataDirectory, "LeaveRequests.json");
                if (File.Exists(requestsPath))
                {
                    leaveRequests = storage.LoadData<List<LeaveRequest>>(requestsPath) ?? new List<LeaveRequest>();
                }
                else
                {
                    leaveRequests = new List<LeaveRequest>();
                }
            }
            catch
            {
                // If anything goes wrong, initialize with empty lists
                leaves = new List<Leave>();
                leaveRequests = new List<LeaveRequest>();
            }
        }

        public LeaveService(FileManager fileManager)
        {
            _fileManager = fileManager;
            leaves = _fileManager?.LoadLeaves() ?? new List<Leave>();
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
            // Validate employee
            var employeeService = EmployeeService.GetInstance();
            var employee = employeeService.GetById(employeeId);
            if (employee == null)
            {
                throw new EntityNotFoundException("Employee not found.");
            }

            // Validate date range
            if (startDate > endDate)
            {
                throw new ValidationException("Start date must be before or equal to end date.");
            }

            // Create Leave
            Leave leave = new Leave
            {
                LeaveId = $"LV{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}",
                EmployeeId = employeeId,
                EmployeeName = employee.Name, // Store employee name
                StartDate = startDate,
                EndDate = endDate,
                Type = leaveType,
                Status = LeaveStatus.Pending,
                Remarks = remarks,
                Employee = employee
            };

            // Create Leave Request
            LeaveRequest leaveRequest = new LeaveRequest
            {
                RequestId = $"LR{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}",
                EmployeeId = employeeId,
                RequestDate = DateTime.Now,
                LeaveDetails = leave,
                ApproverId = null // Will be set by admin
            };

            leaveRequests.Add(leaveRequest);
            leaves.Add(leave);
            SaveChanges();

            return leaveRequest;
        }

        public List<LeaveRequest> GetPendingLeaveRequests()
        {
            return leaveRequests
                .Where(lr => lr.LeaveDetails.Status == LeaveStatus.Pending)
                .ToList();
        }

        public LeaveRequest ApproveLeaveRequest(string requestId, string approverId)
        {
            var leaveRequest = leaveRequests
                .FirstOrDefault(lr => lr.RequestId == requestId);

            if (leaveRequest == null)
            {
                throw new EntityNotFoundException("Leave request not found.");
            }

            leaveRequest.ApproverId = approverId;
            leaveRequest.LeaveDetails.Status = LeaveStatus.Approved;
            SaveChanges();

            return leaveRequest;
        }

        public LeaveRequest RejectLeaveRequest(string requestId, string approverId, string rejectionReason)
        {
            var leaveRequest = leaveRequests
                .FirstOrDefault(lr => lr.RequestId == requestId);

            if (leaveRequest == null)
            {
                throw new EntityNotFoundException("Leave request not found.");
            }

            leaveRequest.ApproverId = approverId;
            leaveRequest.LeaveDetails.Status = LeaveStatus.Rejected;
            leaveRequest.LeaveDetails.Remarks += $" Rejection Reason: {rejectionReason}";
            SaveChanges();

            return leaveRequest;
        }

        public List<Leave> GetEmployeeLeaves(string employeeId)
        {
            return leaves
                .Where(l => l.EmployeeId == employeeId)
                .ToList();
        }

        public Dictionary<LeaveType, int> GetLeaveTypesSummary(string employeeId)
        {
            return leaves
                .Where(l => l.EmployeeId == employeeId && l.Status == LeaveStatus.Approved)
                .GroupBy(l => l.Type)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(l => l.CalculateDays())
                );
        }

        private bool SaveChanges()
        {
            bool success = true;

            // If FileManager is available, use it for both leaves and requests
            if (_fileManager != null)
            {
                success = _fileManager.SaveLeaves(leaves);
                if (success)
                {
                    success = _fileManager.SaveLeaveRequests(leaveRequests);
                }
                return success;
            }

            // Fallback to direct file saving when FileManager is not available
            try
            {
                JsonFileStorage storage = new JsonFileStorage();
                success = storage.SaveData(FileManager.leaveDataPath, leaves);
                
                if (success)
                {
                    success = storage.SaveData(FileManager.leaveRequestsDataPath, leaveRequests);
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
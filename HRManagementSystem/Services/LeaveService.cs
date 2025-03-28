using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HRManagementSystem.Services
{
    public class LeaveService
    {
        private const string LEAVE_REQUEST_FILE_PATH = @"..\..\Data\LeaveRequests.json";
        private const string LEAVE_FILE_PATH = @"..\..\Data\Leave.json";
        private List<LeaveRequest> leaveRequests;
        private List<Leave> leaves;
        private EmployeeService employeeService;

        public LeaveService(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
            LoadLeaveData();
        }

        private void LoadLeaveData()
        {
            // Load Leave Requests
            if (File.Exists(LEAVE_REQUEST_FILE_PATH))
            {
                string requestsJson = File.ReadAllText(LEAVE_REQUEST_FILE_PATH);
                leaveRequests = JsonSerializer.Deserialize<List<LeaveRequest>>(requestsJson) ?? new List<LeaveRequest>();
            }
            else
            {
                leaveRequests = new List<LeaveRequest>();
            }

            // Load Leaves
            if (File.Exists(LEAVE_FILE_PATH))
            {
                string leavesJson = File.ReadAllText(LEAVE_FILE_PATH);
                leaves = JsonSerializer.Deserialize<List<Leave>>(leavesJson) ?? new List<Leave>();
            }
            else
            {
                leaves = new List<Leave>();
            }
        }

        private void SaveLeaveData()
        {
            // Save Leave Requests
            string requestsJson = JsonSerializer.Serialize(leaveRequests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(LEAVE_REQUEST_FILE_PATH, requestsJson);

            // Save Leaves
            string leavesJson = JsonSerializer.Serialize(leaves, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(LEAVE_FILE_PATH, leavesJson);
        }

        public LeaveRequest SubmitLeaveRequest(
            string employeeId,
            DateTime startDate,
            DateTime endDate,
            LeaveType leaveType,
            string remarks)
        {
            // Validate employee
            var employee = employeeService.GetEmployeeById(employeeId);
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
                LeaveId = Guid.NewGuid().ToString(),
                EmployeeId = employeeId,
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
                RequestId = Guid.NewGuid().ToString(),
                EmployeeId = employeeId,
                RequestDate = DateTime.Now,
                LeaveDetails = leave,
                ApproverId = null // Will be set by admin
            };

            leaveRequests.Add(leaveRequest);
            leaves.Add(leave);
            SaveLeaveData();

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
            SaveLeaveData();

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
            SaveLeaveData();

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
    }
}
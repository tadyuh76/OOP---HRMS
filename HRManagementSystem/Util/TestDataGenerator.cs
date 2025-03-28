using System.Text.Json;

namespace HRManagementSystem.Util
{
    public static class TestDataGenerator
    {
        /// <summary>
        /// Generates sample leave requests for testing
        /// </summary>
        public static bool GenerateSampleLeaveRequests(string employeeId, string employeeName, int count = 5)
        {
            try
            {
                List<LeaveRequest> leaveRequests = new List<LeaveRequest>();
                Random random = new Random();
                Array leaveTypes = Enum.GetValues(typeof(LeaveType));
                LeaveStatus[] leaveStatuses = new[] { LeaveStatus.Pending, LeaveStatus.Approved, LeaveStatus.Rejected };
                string?[] approverIds = new[] { "ADMIN001", "ADMIN002", null };

                DateTime baseDate = DateTime.Today;

                for (int i = 0; i < count; i++)
                {
                    DateTime requestDate = baseDate.AddDays(-random.Next(1, 30));
                    DateTime startDate = baseDate.AddDays(random.Next(7, 60));
                    DateTime endDate = startDate.AddDays(random.Next(1, 5));
                    LeaveType leaveType = (LeaveType)leaveTypes.GetValue(random.Next(leaveTypes.Length));
                    LeaveStatus status = leaveStatuses[random.Next(leaveStatuses.Length)];
                    string? approverId = status == LeaveStatus.Pending ? null : approverIds[random.Next(2)];

                    string remarks = $"Test leave request {i + 1}";
                    if (status == LeaveStatus.Rejected)
                    {
                        remarks += " Rejection Reason: Test rejection reason";
                    }

                    LeaveRequest leaveRequest = new LeaveRequest
                    {
                        RequestId = $"TEST{random.Next(1000, 9999)}",
                        EmployeeId = employeeId,
                        EmployeeName = employeeName,
                        RequestDate = requestDate,
                        StartDate = startDate,
                        EndDate = endDate,
                        Type = leaveType,
                        Status = status,
                        Remarks = remarks,
                        ApproverId = approverId
                    };

                    leaveRequests.Add(leaveRequest);
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonData = JsonSerializer.Serialize(leaveRequests, options);
                File.WriteAllText(Path.Combine(FileManager.dataDirectory, "TestLeaveRequests.json"), jsonData);

                Console.WriteLine($"Generated {count} test leave requests for employee {employeeId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating test data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifies that a leave request was correctly saved to the leave data file
        /// </summary>
        public static bool VerifyLeaveRequestSaved(string requestId)
        {
            try
            {
                if (!File.Exists(FileManager.leaveDataPath))
                {
                    Console.WriteLine("Leave data file does not exist");
                    return false;
                }

                string jsonContent = File.ReadAllText(FileManager.leaveDataPath);
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Console.WriteLine("Leave data file is empty");
                    return false;
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<LeaveRequest>? leaveRequests = JsonSerializer.Deserialize<List<LeaveRequest>>(jsonContent, options);
                if (leaveRequests == null)
                {
                    Console.WriteLine("Failed to deserialize leave requests");
                    return false;
                }

                LeaveRequest? found = leaveRequests.Find(r => r.RequestId == requestId);
                if (found == null)
                {
                    Console.WriteLine($"Leave request with ID {requestId} not found");
                    return false;
                }

                Console.WriteLine($"Leave request verified: {found.RequestId} for {found.EmployeeName}, Status: {found.Status}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying leave request: {ex.Message}");
                return false;
            }
        }
    }
}

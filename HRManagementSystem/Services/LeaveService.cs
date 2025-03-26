// // Services/LeaveService.cs
// using System;
// using System.Collections.Generic;

// namespace HRManagementSystem
// {
//     public class LeaveService
//     {
//         private List<Leave> leaves;
//         private List<LeaveRequest> leaveRequests;
//         private JsonFileStorage<Leave> leaveStorage;
//         private JsonFileStorage<LeaveRequest> requestStorage;

//         public LeaveService()
//         {
//             leaveStorage = new JsonFileStorage<Leave>("Leave.json");
//             requestStorage = new JsonFileStorage<LeaveRequest>("LeaveRequests.json");
//             LoadData();
//         }

//         private void LoadData()
//         {
//             leaves = leaveStorage.LoadData() ?? new List<Leave>();
//             leaveRequests = requestStorage.LoadData() ?? new List<LeaveRequest>();
//         }

//         public List<Leave> GetAllLeaves()
//         {
//             return leaves;
//         }

//         public List<Leave> GetLeavesByEmployeeId(string employeeId)
//         {
//             return leaves.FindAll(l => l.EmployeeId == employeeId);
//         }

//         public Leave GetLeaveById(string leaveId)
//         {
//             return leaves.Find(l => l.LeaveId == leaveId);
//         }

//         public List<LeaveRequest> GetPendingLeaveRequests()
//         {
//             return leaveRequests.FindAll(lr => lr.LeaveDetails.Status == LeaveStatus.Pending);
//         }

//         public LeaveRequest SubmitLeaveRequest(string employeeId, DateTime startDate, DateTime endDate, 
//                                              LeaveType type, string remarks, string approverId)
//         {
//             string leaveId = Guid.NewGuid().ToString();
//             Leave leave = new Leave(
//                 leaveId,
//                 employeeId,
//                 startDate,
//                 endDate,
//                 type,
//                 LeaveStatus.Pending,
//                 remarks
//             );

//             string requestId = Guid.NewGuid().ToString();
//             LeaveRequest request = new LeaveRequest(
//                 requestId,
//                 employeeId,
//                 DateTime.Now,
//                 leave,
//                 approverId
//             );

//             request.Submit(); // Updates the status to Pending
//             leaveRequests.Add(request);
//             leaves.Add(leave);
//             SaveChanges();

//             return request;
//         }

//         public void ApproveLeaveRequest(string requestId)
//         {
//             LeaveRequest request = leaveRequests.Find(lr => lr.RequestId == requestId);
//             if (request != null && request.LeaveDetails != null)
//             {
//                 request.LeaveDetails.Status = LeaveStatus.Approved;
//                 Leave leave = leaves.Find(l => l.LeaveId == request.LeaveDetails.LeaveId);
//                 if (leave != null)
//                 {
//                     leave.Status = LeaveStatus.Approved;
//                 }
//                 SaveChanges();
//             }
//         }

//         public void RejectLeaveRequest(string requestId)
//         {
//             LeaveRequest request = leaveRequests.Find(lr => lr.RequestId == requestId);
//             if (request != null && request.LeaveDetails != null)
//             {
//                 request.LeaveDetails.Status = LeaveStatus.Rejected;
//                 Leave leave = leaves.Find(l => l.LeaveId == request.LeaveDetails.LeaveId);
//                 if (leave != null)
//                 {
//                     leave.Status = LeaveStatus.Rejected;
//                 }
//                 SaveChanges();
//             }
//         }

//         public void CancelLeaveRequest(string requestId)
//         {
//             LeaveRequest request = leaveRequests.Find(lr => lr.RequestId == requestId);
//             if (request != null)
//             {
//                 request.Cancel();
//                 Leave leave = leaves.Find(l => l.LeaveId == request.LeaveDetails.LeaveId);
//                 if (leave != null)
//                 {
//                     leave.Status = LeaveStatus.Cancelled;
//                 }
//                 SaveChanges();
//             }
//         }

//         private void SaveChanges()
//         {
//             leaveStorage.SaveData(leaves);
//             requestStorage.SaveData(leaveRequests);
//         }
//     }
// }
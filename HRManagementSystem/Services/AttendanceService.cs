// // Services/AttendanceService.cs
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace HRManagementSystem
// {
//     public class AttendanceService
//     {
//         private List<Attendance> attendances;
//         private JsonFileStorage<Attendance> storage;

//         public AttendanceService()
//         {
//             storage = new("Attendance.json");
//             LoadAttendances();
//         }

//         private void LoadAttendances()
//         {
//             attendances = storage.LoadData() ?? new List<Attendance>();
//         }

//         public List<Attendance> GetAllAttendances()
//         {
//             return attendances;
//         }

//         public List<Attendance> GetAttendancesByEmployeeId(string employeeId)
//         {
//             return attendances.FindAll(a => a.EmployeeId == employeeId);
//         }

//         public List<Attendance> GetAttendancesByDate(DateTime date)
//         {
//             return attendances.FindAll(a => a.Date.Date == date.Date);
//         }

//         public Attendance GetAttendanceById(string attendanceId)
//         {
//             return attendances.Find(a => a.AttendanceId == attendanceId);
//         }

//         public void RecordClockIn(string employeeId, DateTime clockInTime)
//         {
//             string attendanceId = Guid.NewGuid().ToString();
//             Attendance attendance = new Attendance(
//                 attendanceId,
//                 employeeId,
//                 clockInTime.Date,
//                 clockInTime,
//                 DateTime.MinValue, // ClockOut not yet recorded
//                 AttendanceStatus.Present
//             );

//             attendances.Add(attendance);
//             SaveChanges();
//         }

//         public void RecordClockOut(string attendanceId, DateTime clockOutTime)
//         {
//             Attendance attendance = GetAttendanceById(attendanceId);
//             if (attendance != null)
//             {
//                 attendance.ClockOutTime = clockOutTime;
//                 SaveChanges();
//             }
//         }

//         public void UpdateAttendanceStatus(string attendanceId, AttendanceStatus status)
//         {
//             Attendance attendance = GetAttendanceById(attendanceId);
//             if (attendance != null)
//             {
//                 attendance.Status = status;
//                 SaveChanges();
//             }
//         }

//         public void SaveChanges()
//         {
//             storage.SaveData(attendances);
//         }
//     }
// }
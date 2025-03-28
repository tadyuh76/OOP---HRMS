using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HRManagementSystem.Services
{
    public class AttendanceService
    {
        private const string ATTENDANCE_FILE_PATH = @"..\..\Data\Attendance.json";
        private List<Attendance> attendances;
        //private EmployeeService employeeService;

        /*public AttendanceService(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
            LoadAttendances();
        }*/

        private void LoadAttendances()
        {
            if (File.Exists(ATTENDANCE_FILE_PATH))
            {
                string jsonString = File.ReadAllText(ATTENDANCE_FILE_PATH);
                attendances = JsonSerializer.Deserialize<List<Attendance>>(jsonString) ?? new List<Attendance>();
            }
            else
            {
                attendances = new List<Attendance>();
            }
        }

        private void SaveAttendances()
        {
            string jsonString = JsonSerializer.Serialize(attendances, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ATTENDANCE_FILE_PATH, jsonString);
        }

        public Attendance RecordAttendance(string employeeId, string fullName, AttendanceStatus status)
        {
            // Validate employee exists
            var employee = employeeService.GetEmployeeById(employeeId);
            if (employee == null || employee.FullName != fullName)
            {
                throw new EntityNotFoundException("Employee not found or name mismatch.");
            }

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
                AttendanceId = Guid.NewGuid().ToString(),
                EmployeeId = employeeId,
                Date = DateTime.Today,
                ClockInTime = DateTime.Now,
                ClockOutTime = DateTime.MinValue, // Will be updated when clocking out
                Status = status,
                Employee = employee
            };

            attendances.Add(attendance);
            SaveAttendances();

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
            SaveAttendances();
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
    }
}
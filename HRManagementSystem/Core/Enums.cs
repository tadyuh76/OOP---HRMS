using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    // Enumerations
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmployeeStatus
    {
        Active,
        OnLeave,
        Terminated,
        Suspended
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LeaveType
    {
        Annual,
        Sick,
        Maternity,
        Paternity,
        Unpaid,
        Bereavement
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AttendanceStatus
    {
        Present,
        Absent,
        HalfDay,
        WorkFromHome
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        Administrator,
        Employee
    }
}
namespace HRManagementSystem
{
    // Enumerations
    public enum EmployeeStatus
    {
        Active,
        OnLeave,
        Terminated,
        Suspended
    }

    public enum LeaveType
    {
        Annual,
        Sick,
        Maternity,
        Paternity,
        Unpaid,
        Bereavement
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public enum AttendanceStatus
    {
        Present,
        Absent,
        HalfDay,
        WorkFromHome
    }

    public enum UserRole
    {
        Administrator,
        Manager,
        Employee
    }
}
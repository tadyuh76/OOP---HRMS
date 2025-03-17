using HRManagementSystem;

namespace hrms
{
    public class EmployeeFactory
    {
        public Employee CreateEmployee(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            switch (type.ToLower())
            {
                case "fulltime":
                    return CreateFullTimeEmployee();
                case "contract":
                    return CreateContractEmployee();
                case "manager":
                    return CreateManager();
                default:
                    throw new ArgumentException("Invalid employee type", nameof(type));
            }
        }

        public FullTimeEmployee CreateFullTimeEmployee()
        {
            return new FullTimeEmployee
            {
                Status = EmployeeStatus.Active
            };
        }

        public ContractEmployee CreateContractEmployee()
        {
            return new ContractEmployee
            {
                Status = EmployeeStatus.Active
            };
        }

        public Manager CreateManager()
        {
            return new Manager
            {
                Status = EmployeeStatus.Active,
                ManagedEmployeeIds = new System.Collections.Generic.List<string>()
            };
        }
    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class DataGenerator
    {
        private readonly EmployeeFactory _employeeFactory;
        private readonly Random _random;
        
        public DataGenerator()
        {
            _employeeFactory = new EmployeeFactory();
            _random = new Random();
        }
        
        public void GenerateAndSaveEmployees(int count, string filePath)
        {
            List<Employee> employees = GenerateEmployees(count);
            
            try
            {
                // Ensure directory exists
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                // Serialize employees with type information
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                options.Converters.Add(new JsonStringEnumConverter());
                options.Converters.Add(new EmployeeJsonConverter());
                
                string json = JsonSerializer.Serialize(employees, options);
                File.WriteAllText(filePath, json);
                
                Console.WriteLine($"Generated {count} employees and saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving employee data: {ex.Message}");
            }
        }
        
        public List<Employee> GenerateEmployees(int count)
        {
            List<Employee> employees = new List<Employee>();
            
            // Names for generation
            string[] firstNames = { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Jessica", "William", "Lisa", "Thomas", "Olivia", "Daniel", "Sophia", "James", "Emma", "Joseph", "Ava", "Charles", "Mia" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Davis", "Wilson", "Taylor", "Clark", "Lewis", "Walker", "Hall", "Young", "Allen", "King", "Wright", "Scott", "Green", "Baker", "Adams", "Nelson", "Carter" };
            string[] positions = { "Software Engineer", "UI/UX Designer", "Data Analyst", "Project Manager", "Marketing Specialist", "HR Specialist", "Financial Analyst", "System Administrator", "Content Creator", "Business Analyst" };
            string[] departments = { "DEP001", "DEP002", "DEP003", "DEP004", "DEP005" };
            
            for (int i = 0; i < count; i++)
            {
                // Determine employee type based on distribution
                // 70% FullTime, 20% Regular, 10% Contract
                string employeeType;
                int typeRoll = _random.Next(1, 101);
                
                if (typeRoll <= 70)
                {
                    employeeType = "FullTime";
                }
                else if (typeRoll <= 90)
                {
                    employeeType = "Regular";
                }
                else
                {
                    employeeType = "Contract";
                }
                
                // Create base employee
                Employee employee;
                
                switch (employeeType)
                {
                    case "FullTime":
                        employee = _employeeFactory.CreateFullTimeEmployee();
                        break;
                    case "Contract":
                        employee = _employeeFactory.CreateContractEmployee();
                        break;
                    default:
                        employee = new Employee();
                        break;
                }
                
                // Generate common properties
                string firstName = firstNames[_random.Next(firstNames.Length)];
                string lastName = lastNames[_random.Next(lastNames.Length)];
                
                employee.Id = Guid.NewGuid().ToString();
                employee.Name = $"{firstName} {lastName}";
                employee.Email = $"{firstName.ToLower()}.{lastName.ToLower()}@company.com";
                employee.Phone = $"555-{_random.Next(100, 1000)}-{_random.Next(1000, 10000)}";
                employee.DateOfBirth = DateTime.Now.AddYears(-_random.Next(22, 45)).AddDays(-_random.Next(1, 365));
                employee.Address = $"{_random.Next(100, 1000)} {firstNames[_random.Next(firstNames.Length)]} {(DateTime.Now.Millisecond % 2 == 0 ? "St" : "Ave")}, Boston, MA";
                employee.EmployeeId = $"EMP{(i + 1).ToString("000")}";
                employee.HireDate = DateTime.Now.AddYears(-_random.Next(0, 8)).AddMonths(-_random.Next(0, 12));
                employee.Position = positions[_random.Next(positions.Length)];
                employee.BaseSalary = (_random.Next(5000, 10000) / 100m) * 100m; // Round to nearest 100
                employee.DepartmentId = departments[_random.Next(departments.Length)];
                employee.Status = EmployeeStatus.Active;
                
                // Set type-specific properties
                if (employee is FullTimeEmployee fullTimeEmployee)
                {
                    fullTimeEmployee.AnnualBonus = (_random.Next(3000, 15000) / 500m) * 500m; // Round to nearest 500
                }
                else if (employee is ContractEmployee contractEmployee)
                {
                    contractEmployee.HourlyRate = (_random.Next(3000, 6000) / 100m); // $30-$60 per hour
                    contractEmployee.HoursWorked = _random.Next(80, 180); // Range of hours for the month
                }
                
                employees.Add(employee);
            }
            
            return employees;
        }
    }
}

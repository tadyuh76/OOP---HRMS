using System.Text.Json.Serialization;

namespace HRManagementSystem {
    // Base class for all entities
    public class Person
    {
        // Private fields
        private string id = "";
        private string name = "";
        private string email = "";
        private string phone = "";
        private DateTime dateOfBirth = new DateTime();
        private string address = "";

        public Person() { }

        public Person(string id, string name, string email, string phone, DateTime dateOfBirth, string address)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            DateOfBirth = dateOfBirth;
            Address = address;
        }

        // Public properties with JSON serialization
        [JsonPropertyName("id")]
        public string Id { get => id; set => id = value; }
        
        [JsonPropertyName("name")]
        public string Name { get => name; set => name = value; }
        
        [JsonPropertyName("email")]
        public string Email { get => email; set => email = value; }
        
        [JsonPropertyName("phone")]
        public string Phone { get => phone; set => phone = value; }
        
        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        
        [JsonPropertyName("address")]
        public string Address { get => address; set => address = value; }

        // Methods
        public int CalculateAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

}
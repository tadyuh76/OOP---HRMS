using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class RoleSelectionService
    {
        private UserRole currentRole;

        public RoleSelectionService()
        {
            // Default to Employee role
            currentRole = UserRole.Employee;
        }

        public RoleSelectionService(UserRole initialRole)
        {
            currentRole = initialRole;
        }

        [JsonPropertyName("currentRole")]
        public UserRole CurrentRole
        {
            get { return currentRole; }
            private set { currentRole = value; }
        }

        public void SwitchRole(UserRole newRole)
        {
            if (currentRole != newRole)
            {
                currentRole = newRole;
                OnRoleChanged(new RoleChangedEventArgs(newRole));
            }
        }

        public bool CanAccessFeature(string featureName)
        {
            if (string.IsNullOrEmpty(featureName))
            {
                return false;
            }

            // Implementation depends on feature permissions
            switch (currentRole)
            {
                case UserRole.Administrator:
                    return true; // Admin can access all features
                case UserRole.Employee:
                    return featureName == "ViewProfile" || featureName == "RequestLeave"; // Example permissions
                default:
                    return false;
            }
        }

        public event RoleChangedEventHandler RoleChanged;

        protected void OnRoleChanged(RoleChangedEventArgs e)
        {
            RoleChangedEventHandler handler = RoleChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    // Delegate for role changed event
    public delegate void RoleChangedEventHandler(object sender, RoleChangedEventArgs e);

}
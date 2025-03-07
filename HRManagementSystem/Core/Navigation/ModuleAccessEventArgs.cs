
namespace HRManagementSystem
{
    // Custom EventArgs for module access events
    public class ModuleAccessEventArgs : EventArgs
    {
        public string ModuleName { get; private set; }

        public ModuleAccessEventArgs(string moduleName)
        {
            ModuleName = moduleName;
        }
    }
}
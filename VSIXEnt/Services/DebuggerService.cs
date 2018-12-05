using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace VSIXEnt.Services
{
    internal class DebuggerService : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return service();
        }

        public object service()
        {
            
            var commandServices = Package.GetGlobalService((typeof(SVsShellDebugger)));
    

            return commandServices;
        }
    }
}
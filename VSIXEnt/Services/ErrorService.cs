using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace VSIXEnt.Services
{
    internal class ErrorService : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return service();
        }

        public object service()
        {
            IVsErrorList commandServices = Package.GetGlobalService((typeof(SVsErrorList))) as IVsErrorList;
            return commandServices;
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSIXEnt.Services;

namespace VSIXEnt.Helper
{
    class ExceptionFinder
    {
        public static bool isExceptionThrown()
        {
            DebuggerService debuggerService = new DebuggerService();
            DBGMODE dBGMODE = VsShellUtilities.GetDebugMode(debuggerService);
            if(DBGMODE.DBGMODE_Break==dBGMODE)
            {
                return true;
            }
            return false;
        }
    }
}

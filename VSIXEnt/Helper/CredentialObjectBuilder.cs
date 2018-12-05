using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Connected.CredentialStorage;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXEnt.Helper
{
    class CredentialObjectBuilder
    {
        public static IVsCredential getCredentailsObject()
        {
            IVsCredential credentialObject = Package.GetGlobalService((typeof(IVsCredential)))  as IVsCredential;
            return credentialObject;
          
        }
    }
}

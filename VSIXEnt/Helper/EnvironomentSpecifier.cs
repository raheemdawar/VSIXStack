using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXEnt.Helper
{
    class EnvironomentSpecifier
    {
      public  Dictionary<string, string> environoments;
       public EnvironomentSpecifier()
        {
            environoments = new Dictionary<string, string>();
            environoments.Add("cs", "C#");
            environoments.Add("py", "Python");
          

        }
            public  string getEnvNameByFileExtensionName(string name)
        {
            string env = "";
            environoments.TryGetValue(name, out env);
            return env;
        }
    }
}

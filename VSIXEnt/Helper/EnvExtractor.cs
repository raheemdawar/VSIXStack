using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXEnt.Helper
{
    class EnvExtractor
    {
        public string detectEnvironoment(string fileNmae)
        {
            if(!string.IsNullOrWhiteSpace(fileNmae))
            {
                string[] fileParts = fileNmae.Split('.');
                if(fileParts.Length==2)
                {
                    return fileParts[1];
                }
            }

            return null;

        }
    }
}

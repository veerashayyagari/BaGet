using System;
using System.Collections.Generic;
using System.Text;

namespace BaGet.Core.Configuration
{
    public class ConfigUtility
    {
        public static string ReadEnvironmentVariable(string envVariableName)
        {
            var value = Environment.GetEnvironmentVariable(envVariableName);

            if(string.IsNullOrEmpty(value))
            {
                value = Environment.GetEnvironmentVariable(envVariableName, EnvironmentVariableTarget.User);
            }

            return value;
        }
    }
}

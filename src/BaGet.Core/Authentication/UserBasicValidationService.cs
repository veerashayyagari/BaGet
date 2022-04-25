using System;
using System.Collections.Generic;
using BaGet.Core.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BaGet.Core.Authentication
{
    public class UserBasicValidationService : IUserService
    {
        private const string PatTokenConfigSetting = "NUGET_FEED_PAT_TOKENS";

        private readonly ILogger<IUserService> userSvcLogger;

        public UserBasicValidationService(ILogger<IUserService> userServiceLogger)
        {
            userSvcLogger = userServiceLogger;
        }

        public bool IsValidUser(string username, string password)
        {
            if(string.IsNullOrEmpty(ConfigUtility.ReadEnvironmentVariable(PatTokenConfigSetting)))
            {
                userSvcLogger.LogError($"Unable to find PAT token config setting: {PatTokenConfigSetting}");
                throw new ArgumentNullException();
            }

            try
            {
                var basicAuthDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(ConfigUtility.ReadEnvironmentVariable(PatTokenConfigSetting));
                username = username.Trim().ToLower();                

                if(basicAuthDictionary.ContainsKey(username) && basicAuthDictionary[username].Equals(password, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                userSvcLogger.LogError($"Unable to deserialize Token configuration {ex}");
                throw new UnauthorizedAccessException("Error validating user");
            }
        }
    }
}

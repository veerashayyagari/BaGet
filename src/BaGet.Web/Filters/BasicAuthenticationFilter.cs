using BaGet.Core.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace BaGet.Web.Filters
{
    public class BasicAuthenticationFilter : IAuthorizationFilter
    {
        private readonly string _realm;        

        public BasicAuthenticationFilter(string realm)
        {
            if(string.IsNullOrEmpty(realm))
            {
                throw new ArgumentNullException(nameof(realm));
            }

            _realm = realm;            
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];

                if (!string.IsNullOrEmpty(authHeader))
                {
                    var headerValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (headerValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.Parameter)).Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            if (userService.IsValidUser(credentials[0], credentials[1]))
                            {
                                return;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new UnauthorizedAccessException("Authorizing user failed");
            }

            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic real=\"{_realm}\"";
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;

namespace BaGet.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthenticationAttribute: TypeFilterAttribute
    {
        public BasicAuthenticationAttribute(string realm = "Slsec Nuget Feed Realm"): base(typeof(BasicAuthenticationFilter))
        {
            Arguments = new object[] { realm };
        }
    }
}

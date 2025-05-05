using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Principal;
namespace CwcExim.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string[] roles { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public string LoginId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string[] roles { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CwcExim.DAL;
namespace CwcExim.Models
{
    [MetadataType(typeof(LoginMD))]
    public class Login:UserBase
    {
        public string HdnLoginPassword { get; set; }
        public Role Role { get; set; }

        public int FirstLogin { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class ModelAddToRole
    {
       
        public string Email { get; set; }
        public List<string> roles { get; set; }
        public string selectedRole { get; set; }

    }
}
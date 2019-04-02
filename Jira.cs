using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HostJiraIntegretion.Models
{
    public class login
    {
        [Required(ErrorMessage = "user name is needed")]
        public string username { get; set; }

        [Required(ErrorMessage = "password is needed")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
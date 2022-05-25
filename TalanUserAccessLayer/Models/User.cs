using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanUserAccessLayer.Models
{
    public class User
    {
        [Description("UserId")]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string SaltedAndHashedPassword { get; set; }
        public bool Admin { get; set; }


    }
}

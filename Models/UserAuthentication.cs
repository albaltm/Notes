using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    public class UserAuthentication
    {
        public string User {  get; set; }
        public string Token { get; set; } = "";
    }
}

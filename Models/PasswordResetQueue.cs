using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    public class PasswordResetQueue
    {
        public string Id { get; set; }
        public string IdUser { get; set; }
        public int Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}

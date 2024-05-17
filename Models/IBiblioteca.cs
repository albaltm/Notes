using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    public interface IBiblioteca
    {
        string Id { get; set; }
        string User { get; set; }
        string Name { get; set; }
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
        bool Favorite { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MedEd.Models
{
    public partial class Drzava
    {
        public Drzava()
        {
            Mjesto = new HashSet<Mjesto>();
        }

        public int SifraDrzava { get; set; }
        public string NazivDrzava { get; set; }
        public string OznakaDrzava { get; set; }

        public virtual ICollection<Mjesto> Mjesto { get; set; }
    }
}

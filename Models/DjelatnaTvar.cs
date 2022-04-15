using System;
using System.Collections.Generic;

namespace MedEd.Models
{
    public partial class DjelatnaTvar
    {
        public DjelatnaTvar()
        {
            Lijek = new HashSet<Lijek>();
        }

        public int SifraDjelatnaTvar { get; set; }
        public string NazivDjelatnaTvar { get; set; }

        public virtual ICollection<Lijek> Lijek { get; set; }
    }
}

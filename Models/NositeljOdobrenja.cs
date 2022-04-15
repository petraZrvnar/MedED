using System;
using System.Collections.Generic;

namespace MedEd.Models
{
    public partial class NositeljOdobrenja
    {
        public NositeljOdobrenja()
        {
            Lijek = new HashSet<Lijek>();
        }

        public int SifraNositelj { get; set; }
        public string NazivNositelj { get; set; }

        public virtual ICollection<Lijek> Lijek { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MedEd.Models
{
    public partial class VrstaLijek
    {
        public VrstaLijek()
        {
            Lijek = new HashSet<Lijek>();
        }

        public int SifraVrstaLijek { get; set; }
        public string OpisVrstaLijek { get; set; }

        public virtual ICollection<Lijek> Lijek { get; set; }
    }
}

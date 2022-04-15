using System;
using System.Collections.Generic;

namespace MedEd.Models
{
    public partial class FarmaceutskiOblik
    {
        public FarmaceutskiOblik()
        {
            Lijek = new HashSet<Lijek>();
        }

        public int SifraFarmOblik { get; set; }
        public string OpisFarmOblik { get; set; }

        public virtual ICollection<Lijek> Lijek { get; set; }
    }
}

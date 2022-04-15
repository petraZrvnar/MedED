using System.Collections.Generic;

namespace MedEd.Models
{
    public partial class VrstaLjekarna
    {
        public VrstaLjekarna()
        {
            Ljekarna = new HashSet<Ljekarna>();
        }

        public int SifraVrstaLjekarna { get; set; }
        public string OpisVrstaLjekarna { get; set; }

        public virtual ICollection<Ljekarna> Ljekarna { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel;

namespace MedEd.Models
{
    public partial class KlasaRizik
    {
        public KlasaRizik()
        {
            MedicinskiProizvod = new HashSet<MedicinskiProizvod>();
        }

        public int SifraKlasa { get; set; }
        [DisplayName("Klasa rizika")]
        public string OznKlasaRizika { get; set; }

        public virtual ICollection<MedicinskiProizvod> MedicinskiProizvod { get; set; }
    }
}

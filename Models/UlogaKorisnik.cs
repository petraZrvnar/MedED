using System.Collections.Generic;

namespace MedEd.Models
{
    public partial class UlogaKorisnik
    {
        public UlogaKorisnik()
        {
            RegistriraniKorisnik = new HashSet<RegistriraniKorisnik>();
        }

        public int SifraUloga { get; set; }
        public string OpisUloga { get; set; }

        public virtual ICollection<RegistriraniKorisnik> RegistriraniKorisnik { get; set; }
    }
}

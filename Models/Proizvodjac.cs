using System.Collections.Generic;
using System.ComponentModel;

namespace MedEd.Models
{
    public partial class Proizvodjac
    {
        public Proizvodjac()
        {
            Lijek = new HashSet<Lijek>();
            MedicinskiProizvod = new HashSet<MedicinskiProizvod>();
        }

        public int SifraProizvodjac { get; set; }
        public string NazivProizvodjac { get; set; }
        [DisplayName("Sjedište proizvođača")]
        public string Sjediste { get; set; }
        [DisplayName("E-mail proizvođača")]
        public string EmailProizvodjac { get; set; }
        [DisplayName("Stranica proizvođača")]
        public string WebSiteProizvodjac { get; set; }
        public int SifMjesto { get; set; }

        public virtual Mjesto SifMjestoNavigation { get; set; }
        public virtual ICollection<Lijek> Lijek { get; set; }
        public virtual ICollection<MedicinskiProizvod> MedicinskiProizvod { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedEd.Models
{
    public partial class Mjesto
    {
        public Mjesto()
        {
            Ljekarna = new HashSet<Ljekarna>();
            Proizvodjac = new HashSet<Proizvodjac>();
        }

        [Key]
        public int SifraMjesto { get; set; }
        [DisplayName("Mjesto")]
        public string NazivMjesto { get; set; }
        [DisplayName("Poštanski broj")]
        public string PostanskiBroj { get; set; }
        [DisplayName("Država")]
        public int SifDrzava { get; set; }

        [DisplayName("Država")]
        public virtual Drzava SifDrzavaNavigation { get; set; }
        public virtual ICollection<Ljekarna> Ljekarna { get; set; }
        public virtual ICollection<Proizvodjac> Proizvodjac { get; set; }
    }
}

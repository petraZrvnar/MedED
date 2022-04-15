using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedEd.Models
{
    public partial class Lijek
    {
        public Lijek()
        {
            LijekLjekarna = new HashSet<LijekLjekarna>();
        }

        [Key]
        public int SifraLijek { get; set; }

        [Required]
        [DisplayName("Naziv lijeka")]
        [MaxLength(255)]
        public string NazivLijek { get; set; }

        [Required]
        [DisplayName("Broj odobrenja")]
        [MaxLength(14)]
        public string BrojOdobrenja { get; set; }

        [Required]
        [DisplayName("God. stavljanja na tržište")]
        [Range(1950, 2030)]
        public int GodStavljanjaTrziste { get; set; }

        [DisplayName("Slika lijeka")]
        public string SlikaLijek { get; set; }

        [Required]
        [DisplayName("Način izdavanja")]
        [MaxLength(255)]
        public string NacinIzdavanja { get; set; }

        [Required]
        [DisplayName("Na tržištu")]
        [MaxLength(50)]
        public string NaTrzistu { get; set; }

        [Required]
        [DisplayName("Količina djelatne tvari")]
        [Range(1, 10000)]
        public int Kolicina { get; set; }

        [Required]
        [DisplayName("Jedinica")]
        [MaxLength(3)]
        public string Jedinica { get; set; }

        [DisplayName("Farmaceutski oblik")]
        public int SifFarmOblik { get; set; }

        [DisplayName("Vrsta lijeka")]
        public int SifVrstaLijek { get; set; }

        [DisplayName("Nositelj odobrenja")]
        public int SifNositelj { get; set; }

        [DisplayName("Proizvođač")]
        public int SifProizvodjac { get; set; }

        [DisplayName("Djelatna tvar")]
        public int SifDjelatnaTvar { get; set; }

        [DisplayName("Farmaceutski oblik")]
        public virtual FarmaceutskiOblik SifFarmOblikNavigation { get; set; }
        [DisplayName("Nositelj odobrenja")]
        public virtual NositeljOdobrenja SifNositeljNavigation { get; set; }
        [DisplayName("Proizvođač")]
        public virtual Proizvodjac SifProizvodjacNavigation { get; set; }
        [DisplayName("Vrsta lijeka")]
        public virtual VrstaLijek SifVrstaLijekNavigation { get; set; }
        [DisplayName("Djelatna tvar")]
        public virtual DjelatnaTvar SifDjelatnaTvarNavigation { get; set; }
        public virtual ICollection<LijekLjekarna> LijekLjekarna { get; set; }
    }
}

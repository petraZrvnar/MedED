using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedEd.Models
{
    public partial class MedicinskiProizvod
    {
        public MedicinskiProizvod()
        {
            MedProizvodLjekarna = new HashSet<MedProizvodLjekarna>();
        }

        [Key]
        public int SifraMedProizvod { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Namjena")]
        public string Namjena { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Medicinski proizvod nema sliku")]
        [DisplayName("Slika proizvoda")]
        public string SlikaMedProizvod { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Kataloški broj")]
        public string KataloskiBroj { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Naziv proizvoda")]
        public string NazivMedProizvod { get; set; }

        [DisplayName("Proizvođač")]
        public int SifProizvodjac { get; set; }

        [DisplayName("Klasa rizika")]
        public int SifKlasa { get; set; }

        [DisplayName("Klasa rizika")]
        public virtual KlasaRizik SifKlasaNavigation { get; set; }
        [DisplayName("Proizvođač")]
        public virtual Proizvodjac SifProizvodjacNavigation { get; set; }
        public virtual ICollection<MedProizvodLjekarna> MedProizvodLjekarna { get; set; }
    }
}

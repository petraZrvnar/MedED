using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedEd.Models
{
    public partial class Ljekarna
    {
        public Ljekarna()
        {
            LijekLjekarna = new HashSet<LijekLjekarna>();
            MedProizvodLjekarna = new HashSet<MedProizvodLjekarna>();
            RegistriraniKorisnik = new HashSet<RegistriraniKorisnik>();
        }

        [Key]
        public int SifraLjekarna { get; set; }

        [DisplayName("Naziv")]
        [Required]
        [MaxLength(255)]
        public string NazivLjekarna { get; set; }

        [DisplayName("Adresa")]
        [MaxLength(255)]
        [Required]
        public string AdresaLjekarna { get; set; }

        [DisplayName("Kontakt")]
        [MaxLength(255)]
        [Required]
        public string KontaktBroj { get; set; }

        [DisplayName("E-Mail")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Ljekarna nema službeni e-mail")]
        [MaxLength(255)]
        [EmailAddress]
        public string EmailLjekarna { get; set; }

        [DisplayName("Radno Vrijeme")]
        [Required]
        public string RadnoVrijeme { get; set; }

        [DisplayName("Mjesto")]
        public int SifMjesto { get; set; }

        [DisplayName("Vrsta ljekarne")]
        public int SifVrstaLjekarna { get; set; }

        [DisplayName("Mjesto")]
        public virtual Mjesto SifMjestoNavigation { get; set; }
        [DisplayName("Vrsta ljekarne")]
        public virtual VrstaLjekarna SifVrstaLjekarnaNavigation { get; set; }
        public virtual ICollection<LijekLjekarna> LijekLjekarna { get; set; }
        public virtual ICollection<MedProizvodLjekarna> MedProizvodLjekarna { get; set; }
        public virtual ICollection<RegistriraniKorisnik> RegistriraniKorisnik { get; set; }
    }
}

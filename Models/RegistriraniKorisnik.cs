using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedEd.Models
{
    public partial class RegistriraniKorisnik
    {
        [Key]
        public int SifraKorisnik { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Ime")]
        public string Ime { get; set; }

        [MaxLength(255)]
        [Required]
        [DisplayName("Prezime")]
        public string Prezime { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Potrebno je unjeti ispravnu e-mail adresu")]
        [MaxLength(255)]
        [DisplayName("E-mail")]
        public string EmailKorisnik { get; set; }

        [Required]
        [DisplayName("Lozinka")]
        [MaxLength(255)]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [DisplayName("Ljekarna")]
        public int? SifLjekarna { get; set; }

        [DisplayName("Uloga")]
        public int SifUloga { get; set; }

        [DisplayName("Ljekarna")]
        public virtual Ljekarna SifLjekarnaNavigation { get; set; }
        [DisplayName("Uloga")]
        public virtual UlogaKorisnik SifUlogaNavigation { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedEd.Models
{
    public partial class MedProizvodLjekarna
    {
        [DisplayName("Medicinski proizvod")]
        [Required]
        public int SifMedProizvod { get; set; }

        [DisplayName("Ljekarna")]
        [Required]
        public int SifLjekarna { get; set; }

        [Required]
        [DisplayName("Cijena")]
        [Range(0, 20000)]
        public decimal CijenaMedProizvod { get; set; }

        [Required]
        [DisplayName("Dostupnost")]
        public string DostupnostMedProizvod { get; set; }

        [DisplayName("Ljekarna")]
        public virtual Ljekarna SifLjekarnaNavigation { get; set; }
        [DisplayName("Medicinski proizvod")]
        public virtual MedicinskiProizvod SifMedProizvodNavigation { get; set; }
    }
}

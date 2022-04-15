using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedEd.Models
{
    public partial class LijekLjekarna
    {
        [Required]
        [DisplayName("Lijek")]
        public int SifLijek { get; set; }

        [Required]
        [DisplayName("Ljekarna")]
        public int SifLjekarna { get; set; }

        [Required]
        [DisplayName("Cijena")]
        [Range(0, 20000)]
        public decimal CijenaLijek { get; set; }

        [Required]
        [DisplayName("Dostupnost")]
        public string DostupnostLijek { get; set; }

        [DisplayName("Lijek")]
        public virtual Lijek SifLijekNavigation { get; set; }

        [DisplayName("Ljekarna")]
        public virtual Ljekarna SifLjekarnaNavigation { get; set; }
    }
}

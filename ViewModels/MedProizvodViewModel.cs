using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class MedProizvodViewModel
    {
        public IEnumerable<MedProViewModel> MedProizvodi { get; set; }
        public bool BezRezultata { get; set; } = false;
        public PagingInfo PagingInfo { get; set; }
        public string NazivProizvoda { get; set; }
        public string Namjena { get; set; }
        public string KlasaRizika { get; set; } = "Sve klase";
        public string KataloskiBroj { get; set; }
        public string Proizvodjac { get; set; } = "Svi proizvođači";
    }
}

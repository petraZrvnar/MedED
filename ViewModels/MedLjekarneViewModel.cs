using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class MedLjekarneViewModel
    {
        public IEnumerable<MedLjekarnaViewModel> Proizvodi;
        public PagingInfo PagingInfo { get; set; }
        public int Proizvod { get; set; }
        public bool BezRezultata { get; set; } = false;
        public string Mjesto { get; set; }
        public string Naziv { get; set; }
        public string Cijena { get; set; }
        public bool BezCijene { get; set; }
        public string NazivProizvod { get; set; }
        public string Namjena { get; set; }
        public string KlasaRizika { get; set; } = "Sve klase";
        public string KataloskiBroj { get; set; }
        public string Proizvodjac { get; set; } = "Svi proizvođači";
    }
}

using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class LijekoviLjekarnaViewModel
    {
        public IEnumerable<LijekLjekarnaViewModel> Lijekovi;
        public PagingInfo PagingInfo { get; set; }
        public int Lijek { get; set; }
        public bool BezRezultata { get; set; } = false;
        public string Mjesto { get; set; }
        public string Naziv { get; set; }
        public string Cijena { get; set; }
        public bool BezCijene { get; set; }
        public string NazivLijek { get; set; }
    }
}

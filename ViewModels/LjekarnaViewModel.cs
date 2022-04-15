using MedEd.Models;
using System.Collections.Generic;

namespace MedEd.ViewModels
{
    // Klasa za rukovanje ljekarnama prilikom pretrage
    public class LjekarnaViewModel
    {
        public IEnumerable<LjekarnaAdminViewModel> Ljekarne { get; set; }
        public bool BezRezultata { get; set; } = false;
        public PagingInfo PagingInfo { get; set; }
        public string NazivLjekarne { get; set; }
        public string MjestoLjekarne { get; set; } = "Sva mjesta";
        public string VrstaLjekarne { get; set; } = "Sve vrste";
    }
}

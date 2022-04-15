using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class LijekSearchViewModel
    {
        public IEnumerable<LijekViewModel> Lijekovi { get; set; }
        public bool BezRezultata { get; set; } = false;
        public PagingInfo PagingInfo { get; set; }
        public string Naziv;
        public string Vrsta;
        public string Br;
        public string Tvar;
        public string Jedinica;
        public int? Kolicina;
        public string Pro;
        public string Oblik;
        public int? God;
    }
}

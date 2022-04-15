using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class LijekoviViewModel
    {
        public IEnumerable<LijekViewModel> Lijekovi;
        public bool Greska = false;
        public bool Dodano = false;
        public bool Obrisano = false;
        public bool Azurirano = false;
        public bool BezRezultata = false;
        public string Poruka;
        public string Exc;
        public bool GreskaDodavanje;
        public string PorukaDodavanje;
        public bool GreskaEdit;
        public PagingInfo PagingInfo { get; set; }
        public string Naziv;
        public string Vrsta;
        public string Br;
        public string Tvar;
        public string Jedinica;
        public int? Kolicina;
        public string Pro;
        public string Oblik;
        public string Nositelj;
        public int? God;
    }
}

using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class LjekarneAdminViewModel
    {
        public IEnumerable<LjekarnaAdminViewModel> Ljekarne;
        public bool Greska = false;
        public bool Dodano = false;
        public bool Obrisano = false;
        public bool Azurirano = false;
        public bool BezRezultata = false;
        public string Poruka;
        public string Exc;
        public string Naziv;
        public string Vrsta;
        public string Mjesto;
        public string Adresa;
        public string Kontakt;
        public PagingInfo PagingInfo { get; set; }
        public bool GreskaDodavanje;
        public string PorukaDodavanje;
        public bool GreskaEdit;
    }
}

using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class LijekLjekarnaAdminViewModel
    {
        public IEnumerable<LijekLjekarnaViewModel> Lijekovi { get; set; }
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
        public string Br;
        public string Tvar;
        public string Vrsta;
        public string Dostupnost;
        public string Cijena;
        public int Ljekarna;
        public string NazivLjekarna;
    }
}

using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class MedProAdminViewModel
    {
        public IEnumerable<MedProViewModel> Proizvodi;
        public bool Greska = false;
        public bool Dodano = false;
        public bool Obrisano = false;
        public bool Azurirano = false;
        public bool BezRezultata = false;
        public bool GreskaDodavanje;
        public string PorukaDodavanje;
        public bool GreskaEdit;
        public string Poruka;
        public string Exc;
        public string KatBr;
        public string Klasa;
        public string Naziv;
        public string Pro;
        public string Namjena;
        public PagingInfo PagingInfo { get; set; }
    }
}

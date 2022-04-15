using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedEd.ViewModels
{
    public class MedProLjekarnaAdminViewModel
    {
        public IEnumerable<MedLjekarnaViewModel> Proizvodi { get; set; }
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
        public string KatBr;
        public string Namjena;
        public string Klasa;
        public string Dostupnost;
        public string Cijena;
        public int Ljekarna;
        public string NazivLjekarna;
    }
}

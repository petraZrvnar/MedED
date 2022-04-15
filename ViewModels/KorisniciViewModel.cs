using System.Collections.Generic;

namespace MedEd.ViewModels
{
    public class KorisniciViewModel
    {
        public IEnumerable<KorisnikViewModel> Korisnici;
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
        public string Ime;
        public string Prezime;
        public string Email;
        public string KIme;
        public string Uloga;
    }
}

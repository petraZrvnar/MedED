using System;

namespace MedEd
{
    [Serializable]
    public class UserSessionData
    {
        public int SifraKorisnik { get; set; }
        public string KorisnickoIme { get; set; }
        public string EmailKorisnik { get; set; }
        public string Ljekarna { get; set; }
        public int SifLjekarna { get; set; }
        public string Uloga { get; set; }
        public int SifUloga { get; set; }
    }
}

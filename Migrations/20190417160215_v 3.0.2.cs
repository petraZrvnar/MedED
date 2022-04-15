using Microsoft.EntityFrameworkCore.Migrations;

namespace MedEd.Migrations
{
    public partial class v302 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DjelatnaTvar",
                columns: table => new
                {
                    SifraDjelatnaTvar = table.Column<int>(nullable: false),
                    NazivDjelatnaTvar = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Djelatna__732F2CE9384AD92D", x => x.SifraDjelatnaTvar);
                });

            migrationBuilder.CreateTable(
                name: "Drzava",
                columns: table => new
                {
                    SifraDrzava = table.Column<int>(nullable: false),
                    NazivDrzava = table.Column<string>(maxLength: 255, nullable: false),
                    OznakaDrzava = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Drzava__C864F3027BD41F92", x => x.SifraDrzava);
                });

            migrationBuilder.CreateTable(
                name: "FarmaceutskiOblik",
                columns: table => new
                {
                    SifraFarmOblik = table.Column<int>(nullable: false),
                    OpisFarmOblik = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Farmaceu__8B0661D31560794E", x => x.SifraFarmOblik);
                });

            migrationBuilder.CreateTable(
                name: "KlasaRizik",
                columns: table => new
                {
                    SifraKlasa = table.Column<int>(nullable: false),
                    OznKlasaRizika = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KlasaRiz__2CA47A614FD16FBF", x => x.SifraKlasa);
                });

            migrationBuilder.CreateTable(
                name: "NositeljOdobrenja",
                columns: table => new
                {
                    SifraNositelj = table.Column<int>(nullable: false),
                    NazivNositelj = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Nositelj__F4FBB3AE820BDE5E", x => x.SifraNositelj);
                });

            migrationBuilder.CreateTable(
                name: "UlogaKorisnik",
                columns: table => new
                {
                    SifraUloga = table.Column<int>(nullable: false),
                    OpisUloga = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UlogaKor__053C874A26871E15", x => x.SifraUloga);
                });

            migrationBuilder.CreateTable(
                name: "VrstaLijek",
                columns: table => new
                {
                    SifraVrstaLijek = table.Column<int>(nullable: false),
                    OpisVrstaLijek = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VrstaLij__504259ABB19826F9", x => x.SifraVrstaLijek);
                });

            migrationBuilder.CreateTable(
                name: "VrstaLjekarna",
                columns: table => new
                {
                    SifraVrstaLjekarna = table.Column<int>(nullable: false),
                    OpisVrstaLjekarna = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VrstaLje__B3D47E1961176E7D", x => x.SifraVrstaLjekarna);
                });

            migrationBuilder.CreateTable(
                name: "Mjesto",
                columns: table => new
                {
                    SifraMjesto = table.Column<int>(nullable: false),
                    NazivMjesto = table.Column<string>(maxLength: 255, nullable: false),
                    PostanskiBroj = table.Column<string>(maxLength: 5, nullable: false),
                    SifDrzava = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Mjesto__265435AF79D43942", x => x.SifraMjesto);
                    table.ForeignKey(
                        name: "FK_Mjesto_Drzava",
                        column: x => x.SifDrzava,
                        principalTable: "Drzava",
                        principalColumn: "SifraDrzava",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ljekarna",
                columns: table => new
                {
                    SifraLjekarna = table.Column<int>(nullable: false),
                    NazivLjekarna = table.Column<string>(maxLength: 255, nullable: false),
                    AdresaLjekarna = table.Column<string>(maxLength: 255, nullable: false),
                    KontaktBroj = table.Column<string>(maxLength: 255, nullable: false),
                    EmailLjekarna = table.Column<string>(maxLength: 255, nullable: true),
                    RadnoVrijeme = table.Column<string>(type: "ntext", nullable: false),
                    SifMjesto = table.Column<int>(nullable: false),
                    SifVrstaLjekarna = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ljekarna__B46640D9DA6F5DA0", x => x.SifraLjekarna);
                    table.ForeignKey(
                        name: "FK_Ljekarna_Mjesto",
                        column: x => x.SifMjesto,
                        principalTable: "Mjesto",
                        principalColumn: "SifraMjesto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ljekarna_VrstaLjekarna",
                        column: x => x.SifVrstaLjekarna,
                        principalTable: "VrstaLjekarna",
                        principalColumn: "SifraVrstaLjekarna",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Proizvodjac",
                columns: table => new
                {
                    SifraProizvodjac = table.Column<int>(nullable: false),
                    NazivProizvodjac = table.Column<string>(maxLength: 255, nullable: false),
                    Sjediste = table.Column<string>(maxLength: 255, nullable: false),
                    EmailProizvodjac = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    WebSiteProizvodjac = table.Column<string>(type: "ntext", nullable: false),
                    SifMjesto = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proizvod__7E494B3C5EF6AB5F", x => x.SifraProizvodjac);
                    table.ForeignKey(
                        name: "FK_Proizvodjac_Mjesto",
                        column: x => x.SifMjesto,
                        principalTable: "Mjesto",
                        principalColumn: "SifraMjesto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistriraniKorisnik",
                columns: table => new
                {
                    SifraKorisnik = table.Column<int>(nullable: false),
                    Ime = table.Column<string>(maxLength: 255, nullable: false),
                    Prezime = table.Column<string>(maxLength: 255, nullable: false),
                    KorisnickoIme = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    EmailKorisnik = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Lozinka = table.Column<string>(maxLength: 255, nullable: false),
                    SifLjekarna = table.Column<int>(nullable: true),
                    SifUloga = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registri__4BD80AEB9210DF3E", x => x.SifraKorisnik);
                    table.ForeignKey(
                        name: "FK_RegistriraniKorisnik_Ljekarna",
                        column: x => x.SifLjekarna,
                        principalTable: "Ljekarna",
                        principalColumn: "SifraLjekarna",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistriraniKorisnik_UlogaKorisnik",
                        column: x => x.SifUloga,
                        principalTable: "UlogaKorisnik",
                        principalColumn: "SifraUloga",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lijek",
                columns: table => new
                {
                    SifraLijek = table.Column<int>(nullable: false),
                    NazivLijek = table.Column<string>(maxLength: 255, nullable: false),
                    BrojOdobrenja = table.Column<string>(maxLength: 14, nullable: false),
                    GodStavljanjaTrziste = table.Column<int>(nullable: false),
                    SlikaLijek = table.Column<string>(type: "text", nullable: true),
                    NacinIzdavanja = table.Column<string>(maxLength: 255, nullable: false),
                    NaTrzistu = table.Column<short>(nullable: false),
                    UputeOLijeku = table.Column<string>(type: "text", nullable: true),
                    SifFarmOblik = table.Column<int>(nullable: false),
                    SifVrstaLijek = table.Column<int>(nullable: false),
                    SifNositelj = table.Column<int>(nullable: false),
                    SifProizvodjac = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lijek__A6B41DF6DCC03B3B", x => x.SifraLijek);
                    table.ForeignKey(
                        name: "FK_Lijek_FarmaceutskiOblik",
                        column: x => x.SifFarmOblik,
                        principalTable: "FarmaceutskiOblik",
                        principalColumn: "SifraFarmOblik",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lijek_NositeljOdobrenja",
                        column: x => x.SifNositelj,
                        principalTable: "NositeljOdobrenja",
                        principalColumn: "SifraNositelj",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lijek_Proizvodjac",
                        column: x => x.SifProizvodjac,
                        principalTable: "Proizvodjac",
                        principalColumn: "SifraProizvodjac",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lijek_VrstaLijek",
                        column: x => x.SifVrstaLijek,
                        principalTable: "VrstaLijek",
                        principalColumn: "SifraVrstaLijek",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicinskiProizvod",
                columns: table => new
                {
                    SifraMedProizvod = table.Column<int>(nullable: false),
                    Namjena = table.Column<string>(type: "text", nullable: false),
                    SlikaMedProizvod = table.Column<string>(type: "text", nullable: true),
                    KataloskiBroj = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    NazivMedProizvod = table.Column<string>(maxLength: 255, nullable: false),
                    SifProizvodjac = table.Column<int>(nullable: false),
                    SifKlasa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medicins__359B78707C4785D1", x => x.SifraMedProizvod);
                    table.ForeignKey(
                        name: "FK_MedicinskiProizvod_KlasaRizik",
                        column: x => x.SifKlasa,
                        principalTable: "KlasaRizik",
                        principalColumn: "SifraKlasa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicinskiProizvod_Proizvodjac",
                        column: x => x.SifProizvodjac,
                        principalTable: "Proizvodjac",
                        principalColumn: "SifraProizvodjac",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LijekDjelatnaTvar",
                columns: table => new
                {
                    SifLijek = table.Column<int>(nullable: false),
                    SifDjelatnaTvar = table.Column<int>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    Jedinica = table.Column<string>(unicode: false, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LijekDje__B30DAA027D4D2860", x => new { x.SifLijek, x.SifDjelatnaTvar });
                    table.ForeignKey(
                        name: "FK_LijekDjelatnaTvar_DjelatnaTvar",
                        column: x => x.SifDjelatnaTvar,
                        principalTable: "DjelatnaTvar",
                        principalColumn: "SifraDjelatnaTvar",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LijekDjelatnaTvar_Lijek",
                        column: x => x.SifLijek,
                        principalTable: "Lijek",
                        principalColumn: "SifraLijek",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LijekLjekarna",
                columns: table => new
                {
                    SifLijek = table.Column<int>(nullable: false),
                    SifLjekarna = table.Column<int>(nullable: false),
                    CijenaLijek = table.Column<decimal>(type: "numeric(8, 2)", nullable: false),
                    DostupnostLijek = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LijekLje__9D85421431102C79", x => new { x.SifLijek, x.SifLjekarna });
                    table.ForeignKey(
                        name: "FK_LijekLjekarna_Lijek",
                        column: x => x.SifLijek,
                        principalTable: "Lijek",
                        principalColumn: "SifraLijek",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LijekLjekarna_Ljekarna",
                        column: x => x.SifLjekarna,
                        principalTable: "Ljekarna",
                        principalColumn: "SifraLjekarna",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedProizvodLjekarna",
                columns: table => new
                {
                    SifMedProizvod = table.Column<int>(nullable: false),
                    SifLjekarna = table.Column<int>(nullable: false),
                    CijenaMedProizvod = table.Column<decimal>(type: "numeric(8, 2)", nullable: false),
                    DostupnostMedProizvod = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedProiz__F252A2308E79A610", x => new { x.SifMedProizvod, x.SifLjekarna });
                    table.ForeignKey(
                        name: "FK_MedProizvodLjekarna_Ljekarna",
                        column: x => x.SifLjekarna,
                        principalTable: "Ljekarna",
                        principalColumn: "SifraLjekarna",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedProizvodLjekarna_MedicinskiProizvod",
                        column: x => x.SifMedProizvod,
                        principalTable: "MedicinskiProizvod",
                        principalColumn: "SifraMedProizvod",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "AK_Lijek_BrojOdobrenja",
                table: "Lijek",
                column: "BrojOdobrenja",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lijek_SifFarmOblik",
                table: "Lijek",
                column: "SifFarmOblik");

            migrationBuilder.CreateIndex(
                name: "IX_Lijek_SifNositelj",
                table: "Lijek",
                column: "SifNositelj");

            migrationBuilder.CreateIndex(
                name: "IX_Lijek_SifProizvodjac",
                table: "Lijek",
                column: "SifProizvodjac");

            migrationBuilder.CreateIndex(
                name: "IX_Lijek_SifVrstaLijek",
                table: "Lijek",
                column: "SifVrstaLijek");

            migrationBuilder.CreateIndex(
                name: "IX_LijekDjelatnaTvar_SifDjelatnaTvar",
                table: "LijekDjelatnaTvar",
                column: "SifDjelatnaTvar");

            migrationBuilder.CreateIndex(
                name: "IX_LijekLjekarna_SifLjekarna",
                table: "LijekLjekarna",
                column: "SifLjekarna");

            migrationBuilder.CreateIndex(
                name: "IX_Ljekarna_SifMjesto",
                table: "Ljekarna",
                column: "SifMjesto");

            migrationBuilder.CreateIndex(
                name: "IX_Ljekarna_SifVrstaLjekarna",
                table: "Ljekarna",
                column: "SifVrstaLjekarna");

            migrationBuilder.CreateIndex(
                name: "AK_MedicinskiProizvod_KataloskiBroj",
                table: "MedicinskiProizvod",
                column: "KataloskiBroj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicinskiProizvod_SifKlasa",
                table: "MedicinskiProizvod",
                column: "SifKlasa");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinskiProizvod_SifProizvodjac",
                table: "MedicinskiProizvod",
                column: "SifProizvodjac");

            migrationBuilder.CreateIndex(
                name: "IX_MedProizvodLjekarna_SifLjekarna",
                table: "MedProizvodLjekarna",
                column: "SifLjekarna");

            migrationBuilder.CreateIndex(
                name: "AK_Mjesto_PostanskiBroj",
                table: "Mjesto",
                column: "PostanskiBroj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mjesto_SifDrzava",
                table: "Mjesto",
                column: "SifDrzava");

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodjac_SifMjesto",
                table: "Proizvodjac",
                column: "SifMjesto");

            migrationBuilder.CreateIndex(
                name: "AK_RegistriraniKorisnik_Column",
                table: "RegistriraniKorisnik",
                column: "EmailKorisnik",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_RegistriraniKorisnik_KorisnickoIme",
                table: "RegistriraniKorisnik",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistriraniKorisnik_SifLjekarna",
                table: "RegistriraniKorisnik",
                column: "SifLjekarna");

            migrationBuilder.CreateIndex(
                name: "IX_RegistriraniKorisnik_SifUloga",
                table: "RegistriraniKorisnik",
                column: "SifUloga");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LijekDjelatnaTvar");

            migrationBuilder.DropTable(
                name: "LijekLjekarna");

            migrationBuilder.DropTable(
                name: "MedProizvodLjekarna");

            migrationBuilder.DropTable(
                name: "RegistriraniKorisnik");

            migrationBuilder.DropTable(
                name: "DjelatnaTvar");

            migrationBuilder.DropTable(
                name: "Lijek");

            migrationBuilder.DropTable(
                name: "MedicinskiProizvod");

            migrationBuilder.DropTable(
                name: "Ljekarna");

            migrationBuilder.DropTable(
                name: "UlogaKorisnik");

            migrationBuilder.DropTable(
                name: "FarmaceutskiOblik");

            migrationBuilder.DropTable(
                name: "NositeljOdobrenja");

            migrationBuilder.DropTable(
                name: "VrstaLijek");

            migrationBuilder.DropTable(
                name: "KlasaRizik");

            migrationBuilder.DropTable(
                name: "Proizvodjac");

            migrationBuilder.DropTable(
                name: "VrstaLjekarna");

            migrationBuilder.DropTable(
                name: "Mjesto");

            migrationBuilder.DropTable(
                name: "Drzava");
        }
    }
}

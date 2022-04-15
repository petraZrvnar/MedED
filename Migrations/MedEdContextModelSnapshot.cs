﻿// <auto-generated />
using System;
using MedEd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedEd.Migrations
{
    [DbContext(typeof(MedEdContext))]
    partial class MedEdContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MedEd.Models.DjelatnaTvar", b =>
                {
                    b.Property<int>("SifraDjelatnaTvar");

                    b.Property<string>("NazivDjelatnaTvar")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SifraDjelatnaTvar")
                        .HasName("PK__Djelatna__732F2CE9384AD92D");

                    b.ToTable("DjelatnaTvar");
                });

            modelBuilder.Entity("MedEd.Models.Drzava", b =>
                {
                    b.Property<int>("SifraDrzava");

                    b.Property<string>("NazivDrzava")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("OznakaDrzava")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("SifraDrzava")
                        .HasName("PK__Drzava__C864F3027BD41F92");

                    b.ToTable("Drzava");
                });

            modelBuilder.Entity("MedEd.Models.FarmaceutskiOblik", b =>
                {
                    b.Property<int>("SifraFarmOblik");

                    b.Property<string>("OpisFarmOblik")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SifraFarmOblik")
                        .HasName("PK__Farmaceu__8B0661D31560794E");

                    b.ToTable("FarmaceutskiOblik");
                });

            modelBuilder.Entity("MedEd.Models.KlasaRizik", b =>
                {
                    b.Property<int>("SifraKlasa");

                    b.Property<string>("OznKlasaRizika")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SifraKlasa")
                        .HasName("PK__KlasaRiz__2CA47A614FD16FBF");

                    b.ToTable("KlasaRizik");
                });

            modelBuilder.Entity("MedEd.Models.Lijek", b =>
                {
                    b.Property<int>("SifraLijek");

                    b.Property<string>("BrojOdobrenja")
                        .IsRequired()
                        .HasMaxLength(14);

                    b.Property<int>("GodStavljanjaTrziste");

                    b.Property<short>("NaTrzistu");

                    b.Property<string>("NacinIzdavanja")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("NazivLijek")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("SifFarmOblik");

                    b.Property<int>("SifNositelj");

                    b.Property<int>("SifProizvodjac");

                    b.Property<int>("SifVrstaLijek");

                    b.Property<string>("SlikaLijek")
                        .HasColumnType("text");

                    b.Property<string>("UputeOlijeku")
                        .HasColumnName("UputeOLijeku")
                        .HasColumnType("text");

                    b.HasKey("SifraLijek")
                        .HasName("PK__Lijek__A6B41DF6DCC03B3B");

                    b.HasIndex("BrojOdobrenja")
                        .IsUnique()
                        .HasName("AK_Lijek_BrojOdobrenja");

                    b.HasIndex("SifFarmOblik");

                    b.HasIndex("SifNositelj");

                    b.HasIndex("SifProizvodjac");

                    b.HasIndex("SifVrstaLijek");

                    b.ToTable("Lijek");
                });

            modelBuilder.Entity("MedEd.Models.LijekDjelatnaTvar", b =>
                {
                    b.Property<int>("SifLijek");

                    b.Property<int>("SifDjelatnaTvar");

                    b.Property<string>("Jedinica")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false);

                    b.Property<int>("Kolicina");

                    b.HasKey("SifLijek", "SifDjelatnaTvar")
                        .HasName("PK__LijekDje__B30DAA027D4D2860");

                    b.HasIndex("SifDjelatnaTvar");

                    b.ToTable("LijekDjelatnaTvar");
                });

            modelBuilder.Entity("MedEd.Models.LijekLjekarna", b =>
                {
                    b.Property<int>("SifLijek");

                    b.Property<int>("SifLjekarna");

                    b.Property<decimal>("CijenaLijek")
                        .HasColumnType("numeric(8, 2)");

                    b.Property<short>("DostupnostLijek");

                    b.HasKey("SifLijek", "SifLjekarna")
                        .HasName("PK__LijekLje__9D85421431102C79");

                    b.HasIndex("SifLjekarna");

                    b.ToTable("LijekLjekarna");
                });

            modelBuilder.Entity("MedEd.Models.Ljekarna", b =>
                {
                    b.Property<int>("SifraLjekarna")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdresaLjekarna")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("EmailLjekarna")
                        .HasMaxLength(255);

                    b.Property<string>("KontaktBroj")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("NazivLjekarna")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("RadnoVrijeme")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<int>("SifMjesto");

                    b.Property<int>("SifVrstaLjekarna");

                    b.HasKey("SifraLjekarna")
                        .HasName("PK__Ljekarna__B46640D9DA6F5DA0");

                    b.HasIndex("SifMjesto");

                    b.HasIndex("SifVrstaLjekarna");

                    b.ToTable("Ljekarna");
                });

            modelBuilder.Entity("MedEd.Models.MedProizvodLjekarna", b =>
                {
                    b.Property<int>("SifMedProizvod");

                    b.Property<int>("SifLjekarna");

                    b.Property<decimal>("CijenaMedProizvod")
                        .HasColumnType("numeric(8, 2)");

                    b.Property<string>("DostupnostMedProizvod");

                    b.HasKey("SifMedProizvod", "SifLjekarna")
                        .HasName("PK__MedProiz__F252A2308E79A610");

                    b.HasIndex("SifLjekarna");

                    b.ToTable("MedProizvodLjekarna");
                });

            modelBuilder.Entity("MedEd.Models.MedicinskiProizvod", b =>
                {
                    b.Property<int>("SifraMedProizvod");

                    b.Property<string>("KataloskiBroj")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<string>("Namjena")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NazivMedProizvod")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("SifKlasa");

                    b.Property<int>("SifProizvodjac");

                    b.Property<string>("SlikaMedProizvod")
                        .HasColumnType("text");

                    b.HasKey("SifraMedProizvod")
                        .HasName("PK__Medicins__359B78707C4785D1");

                    b.HasIndex("KataloskiBroj")
                        .IsUnique()
                        .HasName("AK_MedicinskiProizvod_KataloskiBroj");

                    b.HasIndex("SifKlasa");

                    b.HasIndex("SifProizvodjac");

                    b.ToTable("MedicinskiProizvod");
                });

            modelBuilder.Entity("MedEd.Models.Mjesto", b =>
                {
                    b.Property<int>("SifraMjesto");

                    b.Property<string>("NazivMjesto")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("PostanskiBroj")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<int>("SifDrzava");

                    b.HasKey("SifraMjesto")
                        .HasName("PK__Mjesto__265435AF79D43942");

                    b.HasIndex("PostanskiBroj")
                        .IsUnique()
                        .HasName("AK_Mjesto_PostanskiBroj");

                    b.HasIndex("SifDrzava");

                    b.ToTable("Mjesto");
                });

            modelBuilder.Entity("MedEd.Models.NositeljOdobrenja", b =>
                {
                    b.Property<int>("SifraNositelj");

                    b.Property<string>("NazivNositelj")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SifraNositelj")
                        .HasName("PK__Nositelj__F4FBB3AE820BDE5E");

                    b.ToTable("NositeljOdobrenja");
                });

            modelBuilder.Entity("MedEd.Models.Proizvodjac", b =>
                {
                    b.Property<int>("SifraProizvodjac");

                    b.Property<string>("EmailProizvodjac")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("NazivProizvodjac")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("SifMjesto");

                    b.Property<string>("Sjediste")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("WebSiteProizvodjac")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.HasKey("SifraProizvodjac")
                        .HasName("PK__Proizvod__7E494B3C5EF6AB5F");

                    b.HasIndex("SifMjesto");

                    b.ToTable("Proizvodjac");
                });

            modelBuilder.Entity("MedEd.Models.RegistriraniKorisnik", b =>
                {
                    b.Property<int>("SifraKorisnik");

                    b.Property<string>("EmailKorisnik")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("KorisnickoIme")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Lozinka")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int?>("SifLjekarna");

                    b.Property<int>("SifUloga");

                    b.HasKey("SifraKorisnik")
                        .HasName("PK__Registri__4BD80AEB9210DF3E");

                    b.HasIndex("EmailKorisnik")
                        .IsUnique()
                        .HasName("AK_RegistriraniKorisnik_Column");

                    b.HasIndex("KorisnickoIme")
                        .IsUnique()
                        .HasName("AK_RegistriraniKorisnik_KorisnickoIme");

                    b.HasIndex("SifLjekarna");

                    b.HasIndex("SifUloga");

                    b.ToTable("RegistriraniKorisnik");
                });

            modelBuilder.Entity("MedEd.Models.UlogaKorisnik", b =>
                {
                    b.Property<int>("SifraUloga");

                    b.Property<string>("OpisUloga")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SifraUloga")
                        .HasName("PK__UlogaKor__053C874A26871E15");

                    b.ToTable("UlogaKorisnik");
                });

            modelBuilder.Entity("MedEd.Models.VrstaLijek", b =>
                {
                    b.Property<int>("SifraVrstaLijek");

                    b.Property<string>("OpisVrstaLijek")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SifraVrstaLijek")
                        .HasName("PK__VrstaLij__504259ABB19826F9");

                    b.ToTable("VrstaLijek");
                });

            modelBuilder.Entity("MedEd.Models.VrstaLjekarna", b =>
                {
                    b.Property<int>("SifraVrstaLjekarna");

                    b.Property<string>("OpisVrstaLjekarna")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SifraVrstaLjekarna")
                        .HasName("PK__VrstaLje__B3D47E1961176E7D");

                    b.ToTable("VrstaLjekarna");
                });

            modelBuilder.Entity("MedEd.Models.Lijek", b =>
                {
                    b.HasOne("MedEd.Models.FarmaceutskiOblik", "SifFarmOblikNavigation")
                        .WithMany("Lijek")
                        .HasForeignKey("SifFarmOblik")
                        .HasConstraintName("FK_Lijek_FarmaceutskiOblik");

                    b.HasOne("MedEd.Models.NositeljOdobrenja", "SifNositeljNavigation")
                        .WithMany("Lijek")
                        .HasForeignKey("SifNositelj")
                        .HasConstraintName("FK_Lijek_NositeljOdobrenja");

                    b.HasOne("MedEd.Models.Proizvodjac", "SifProizvodjacNavigation")
                        .WithMany("Lijek")
                        .HasForeignKey("SifProizvodjac")
                        .HasConstraintName("FK_Lijek_Proizvodjac");

                    b.HasOne("MedEd.Models.VrstaLijek", "SifVrstaLijekNavigation")
                        .WithMany("Lijek")
                        .HasForeignKey("SifVrstaLijek")
                        .HasConstraintName("FK_Lijek_VrstaLijek");
                });

            modelBuilder.Entity("MedEd.Models.LijekDjelatnaTvar", b =>
                {
                    b.HasOne("MedEd.Models.DjelatnaTvar", "SifDjelatnaTvarNavigation")
                        .WithMany("LijekDjelatnaTvar")
                        .HasForeignKey("SifDjelatnaTvar")
                        .HasConstraintName("FK_LijekDjelatnaTvar_DjelatnaTvar");

                    b.HasOne("MedEd.Models.Lijek", "SifLijekNavigation")
                        .WithMany("LijekDjelatnaTvar")
                        .HasForeignKey("SifLijek")
                        .HasConstraintName("FK_LijekDjelatnaTvar_Lijek");
                });

            modelBuilder.Entity("MedEd.Models.LijekLjekarna", b =>
                {
                    b.HasOne("MedEd.Models.Lijek", "SifLijekNavigation")
                        .WithMany("LijekLjekarna")
                        .HasForeignKey("SifLijek")
                        .HasConstraintName("FK_LijekLjekarna_Lijek");

                    b.HasOne("MedEd.Models.Ljekarna", "SifLjekarnaNavigation")
                        .WithMany("LijekLjekarna")
                        .HasForeignKey("SifLjekarna")
                        .HasConstraintName("FK_LijekLjekarna_Ljekarna");
                });

            modelBuilder.Entity("MedEd.Models.Ljekarna", b =>
                {
                    b.HasOne("MedEd.Models.Mjesto", "SifMjestoNavigation")
                        .WithMany("Ljekarna")
                        .HasForeignKey("SifMjesto")
                        .HasConstraintName("FK_Ljekarna_Mjesto");

                    b.HasOne("MedEd.Models.VrstaLjekarna", "SifVrstaLjekarnaNavigation")
                        .WithMany("Ljekarna")
                        .HasForeignKey("SifVrstaLjekarna")
                        .HasConstraintName("FK_Ljekarna_VrstaLjekarna");
                });

            modelBuilder.Entity("MedEd.Models.MedProizvodLjekarna", b =>
                {
                    b.HasOne("MedEd.Models.Ljekarna", "SifLjekarnaNavigation")
                        .WithMany("MedProizvodLjekarna")
                        .HasForeignKey("SifLjekarna")
                        .HasConstraintName("FK_MedProizvodLjekarna_Ljekarna");

                    b.HasOne("MedEd.Models.MedicinskiProizvod", "SifMedProizvodNavigation")
                        .WithMany("MedProizvodLjekarna")
                        .HasForeignKey("SifMedProizvod")
                        .HasConstraintName("FK_MedProizvodLjekarna_MedicinskiProizvod");
                });

            modelBuilder.Entity("MedEd.Models.MedicinskiProizvod", b =>
                {
                    b.HasOne("MedEd.Models.KlasaRizik", "SifKlasaNavigation")
                        .WithMany("MedicinskiProizvod")
                        .HasForeignKey("SifKlasa")
                        .HasConstraintName("FK_MedicinskiProizvod_KlasaRizik");

                    b.HasOne("MedEd.Models.Proizvodjac", "SifProizvodjacNavigation")
                        .WithMany("MedicinskiProizvod")
                        .HasForeignKey("SifProizvodjac")
                        .HasConstraintName("FK_MedicinskiProizvod_Proizvodjac");
                });

            modelBuilder.Entity("MedEd.Models.Mjesto", b =>
                {
                    b.HasOne("MedEd.Models.Drzava", "SifDrzavaNavigation")
                        .WithMany("Mjesto")
                        .HasForeignKey("SifDrzava")
                        .HasConstraintName("FK_Mjesto_Drzava");
                });

            modelBuilder.Entity("MedEd.Models.Proizvodjac", b =>
                {
                    b.HasOne("MedEd.Models.Mjesto", "SifMjestoNavigation")
                        .WithMany("Proizvodjac")
                        .HasForeignKey("SifMjesto")
                        .HasConstraintName("FK_Proizvodjac_Mjesto");
                });

            modelBuilder.Entity("MedEd.Models.RegistriraniKorisnik", b =>
                {
                    b.HasOne("MedEd.Models.Ljekarna", "SifLjekarnaNavigation")
                        .WithMany("RegistriraniKorisnik")
                        .HasForeignKey("SifLjekarna")
                        .HasConstraintName("FK_RegistriraniKorisnik_Ljekarna");

                    b.HasOne("MedEd.Models.UlogaKorisnik", "SifUlogaNavigation")
                        .WithMany("RegistriraniKorisnik")
                        .HasForeignKey("SifUloga")
                        .HasConstraintName("FK_RegistriraniKorisnik_UlogaKorisnik");
                });
#pragma warning restore 612, 618
        }
    }
}
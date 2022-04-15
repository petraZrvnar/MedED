using Microsoft.EntityFrameworkCore;

namespace MedEd.Models
{
    public partial class MedEdContext : DbContext
    {
        public MedEdContext()
        {
        }

        public MedEdContext(DbContextOptions<MedEdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DjelatnaTvar> DjelatnaTvar { get; set; }
        public virtual DbSet<Drzava> Drzava { get; set; }
        public virtual DbSet<FarmaceutskiOblik> FarmaceutskiOblik { get; set; }
        public virtual DbSet<KlasaRizik> KlasaRizik { get; set; }
        public virtual DbSet<Lijek> Lijek { get; set; }
        public virtual DbSet<LijekLjekarna> LijekLjekarna { get; set; }
        public virtual DbSet<Ljekarna> Ljekarna { get; set; }
        public virtual DbSet<MedProizvodLjekarna> MedProizvodLjekarna { get; set; }
        public virtual DbSet<MedicinskiProizvod> MedicinskiProizvod { get; set; }
        public virtual DbSet<Mjesto> Mjesto { get; set; }
        public virtual DbSet<NositeljOdobrenja> NositeljOdobrenja { get; set; }
        public virtual DbSet<Proizvodjac> Proizvodjac { get; set; }
        public virtual DbSet<RegistriraniKorisnik> RegistriraniKorisnik { get; set; }
        public virtual DbSet<UlogaKorisnik> UlogaKorisnik { get; set; }
        public virtual DbSet<VrstaLijek> VrstaLijek { get; set; }
        public virtual DbSet<VrstaLjekarna> VrstaLjekarna { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Korisnik\\Documents\\MedEdDb.mdf;Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<DjelatnaTvar>(entity =>
            {
                entity.HasKey(e => e.SifraDjelatnaTvar)
                    .HasName("PK__Djelatna__732F2CE9384AD92D");

                entity.Property(e => e.SifraDjelatnaTvar).ValueGeneratedNever();

                entity.Property(e => e.NazivDjelatnaTvar)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Drzava>(entity =>
            {
                entity.HasKey(e => e.SifraDrzava)
                    .HasName("PK__Drzava__C864F3027BD41F92");

                entity.Property(e => e.SifraDrzava).ValueGeneratedNever();

                entity.Property(e => e.NazivDrzava)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OznakaDrzava)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<FarmaceutskiOblik>(entity =>
            {
                entity.HasKey(e => e.SifraFarmOblik)
                    .HasName("PK__Farmaceu__8B0661D31560794E");

                entity.Property(e => e.SifraFarmOblik).ValueGeneratedNever();

                entity.Property(e => e.OpisFarmOblik)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<KlasaRizik>(entity =>
            {
                entity.HasKey(e => e.SifraKlasa)
                    .HasName("PK__KlasaRiz__2CA47A614FD16FBF");

                entity.Property(e => e.SifraKlasa).ValueGeneratedNever();

                entity.Property(e => e.OznKlasaRizika)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Lijek>(entity =>
            {
                entity.HasKey(e => e.SifraLijek)
                    .HasName("PK__Lijek__A6B41DF6DCC03B3B");

                entity.HasIndex(e => e.BrojOdobrenja)
                    .HasName("AK_Lijek_BrojOdobrenja")
                    .IsUnique();

                entity.Property(e => e.SifraLijek).ValueGeneratedOnAdd();

                entity.Property(e => e.BrojOdobrenja)
                    .IsRequired()
                    .HasMaxLength(14);

                entity.Property(e => e.NacinIzdavanja)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.NaTrzistu)
                   .IsRequired()
                   .HasMaxLength(50);

                entity.Property(e => e.NazivLijek)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SlikaLijek).HasColumnType("text");

                entity.Property(e => e.Jedinica)
                    .HasColumnName("Jedinica")
                    .HasMaxLength(3);

                entity.HasOne(d => d.SifFarmOblikNavigation)
                    .WithMany(p => p.Lijek)
                    .HasForeignKey(d => d.SifFarmOblik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lijek_FarmaceutskiOblik");

                entity.HasOne(d => d.SifNositeljNavigation)
                    .WithMany(p => p.Lijek)
                    .HasForeignKey(d => d.SifNositelj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lijek_NositeljOdobrenja");

                entity.HasOne(d => d.SifProizvodjacNavigation)
                    .WithMany(p => p.Lijek)
                    .HasForeignKey(d => d.SifProizvodjac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lijek_Proizvodjac");

                entity.HasOne(d => d.SifVrstaLijekNavigation)
                    .WithMany(p => p.Lijek)
                    .HasForeignKey(d => d.SifVrstaLijek)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lijek_VrstaLijek");

                entity.HasOne(d => d.SifDjelatnaTvarNavigation)
                   .WithMany(p => p.Lijek)
                   .HasForeignKey(d => d.SifDjelatnaTvar)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Lijek_DjelatnaTvar");
            });

            modelBuilder.Entity<LijekLjekarna>(entity =>
            {
                entity.HasKey(e => new { e.SifLijek, e.SifLjekarna })
                    .HasName("PK__LijekLje__9D85421431102C79");

                entity.Property(e => e.CijenaLijek).HasColumnType("numeric(8, 2)");

                entity.HasOne(d => d.SifLijekNavigation)
                    .WithMany(p => p.LijekLjekarna)
                    .HasForeignKey(d => d.SifLijek)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LijekLjekarna_Lijek");

                entity.HasOne(d => d.SifLjekarnaNavigation)
                    .WithMany(p => p.LijekLjekarna)
                    .HasForeignKey(d => d.SifLjekarna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LijekLjekarna_Ljekarna");
            });

            modelBuilder.Entity<Ljekarna>(entity =>
            {
                entity.HasKey(e => e.SifraLjekarna)
                    .HasName("PK__Ljekarna__B46640D9DA6F5DA0");

                entity.Property(e => e.SifraLjekarna).ValueGeneratedOnAdd();

                entity.Property(e => e.AdresaLjekarna)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.EmailLjekarna).HasMaxLength(255);

                entity.Property(e => e.KontaktBroj)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.NazivLjekarna)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RadnoVrijeme)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.SifMjestoNavigation)
                    .WithMany(p => p.Ljekarna)
                    .HasForeignKey(d => d.SifMjesto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ljekarna_Mjesto");

                entity.HasOne(d => d.SifVrstaLjekarnaNavigation)
                    .WithMany(p => p.Ljekarna)
                    .HasForeignKey(d => d.SifVrstaLjekarna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ljekarna_VrstaLjekarna");
            });

            modelBuilder.Entity<MedProizvodLjekarna>(entity =>
            {
                entity.HasKey(e => new { e.SifMedProizvod, e.SifLjekarna })
                    .HasName("PK__MedProiz__F252A2308E79A610");

                entity.Property(e => e.CijenaMedProizvod).HasColumnType("numeric(8, 2)");

                entity.HasOne(d => d.SifLjekarnaNavigation)
                    .WithMany(p => p.MedProizvodLjekarna)
                    .HasForeignKey(d => d.SifLjekarna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedProizvodLjekarna_Ljekarna");

                entity.HasOne(d => d.SifMedProizvodNavigation)
                    .WithMany(p => p.MedProizvodLjekarna)
                    .HasForeignKey(d => d.SifMedProizvod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedProizvodLjekarna_MedicinskiProizvod");
            });

            modelBuilder.Entity<MedicinskiProizvod>(entity =>
            {
                entity.HasKey(e => e.SifraMedProizvod)
                    .HasName("PK__Medicins__359B78707C4785D1");

                entity.HasIndex(e => e.KataloskiBroj)
                    .HasName("AK_MedicinskiProizvod_KataloskiBroj")
                    .IsUnique();

                entity.Property(e => e.SifraMedProizvod).ValueGeneratedOnAdd();

                entity.Property(e => e.KataloskiBroj)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Namjena)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.NazivMedProizvod)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SlikaMedProizvod).HasColumnType("text");

                entity.HasOne(d => d.SifKlasaNavigation)
                    .WithMany(p => p.MedicinskiProizvod)
                    .HasForeignKey(d => d.SifKlasa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicinskiProizvod_KlasaRizik");

                entity.HasOne(d => d.SifProizvodjacNavigation)
                    .WithMany(p => p.MedicinskiProizvod)
                    .HasForeignKey(d => d.SifProizvodjac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicinskiProizvod_Proizvodjac");
            });

            modelBuilder.Entity<Mjesto>(entity =>
            {
                entity.HasKey(e => e.SifraMjesto)
                    .HasName("PK__Mjesto__265435AF79D43942");

                entity.HasIndex(e => e.PostanskiBroj)
                    .HasName("AK_Mjesto_PostanskiBroj")
                    .IsUnique();

                entity.Property(e => e.SifraMjesto).ValueGeneratedOnAdd();

                entity.Property(e => e.NazivMjesto)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PostanskiBroj)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.SifDrzavaNavigation)
                    .WithMany(p => p.Mjesto)
                    .HasForeignKey(d => d.SifDrzava)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mjesto_Drzava");
            });

            modelBuilder.Entity<NositeljOdobrenja>(entity =>
            {
                entity.HasKey(e => e.SifraNositelj)
                    .HasName("PK__Nositelj__F4FBB3AE820BDE5E");

                entity.Property(e => e.SifraNositelj).ValueGeneratedNever();

                entity.Property(e => e.NazivNositelj)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Proizvodjac>(entity =>
            {
                entity.HasKey(e => e.SifraProizvodjac)
                    .HasName("PK__Proizvod__7E494B3C5EF6AB5F");

                entity.Property(e => e.SifraProizvodjac).ValueGeneratedNever();

                entity.Property(e => e.EmailProizvodjac)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NazivProizvodjac)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Sjediste)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.WebSiteProizvodjac)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.SifMjestoNavigation)
                    .WithMany(p => p.Proizvodjac)
                    .HasForeignKey(d => d.SifMjesto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proizvodjac_Mjesto");
            });

            modelBuilder.Entity<RegistriraniKorisnik>(entity =>
            {
                entity.HasKey(e => e.SifraKorisnik)
                    .HasName("PK__Registri__4BD80AEB9210DF3E");

                entity.HasIndex(e => e.EmailKorisnik)
                    .HasName("AK_RegistriraniKorisnik_Column")
                    .IsUnique();

                entity.HasIndex(e => e.KorisnickoIme)
                    .HasName("AK_RegistriraniKorisnik_KorisnickoIme")
                    .IsUnique();

                entity.Property(e => e.SifraKorisnik).ValueGeneratedOnAdd();

                entity.Property(e => e.EmailKorisnik)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Ime)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.KorisnickoIme)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Lozinka)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Prezime)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.SifLjekarnaNavigation)
                    .WithMany(p => p.RegistriraniKorisnik)
                    .HasForeignKey(d => d.SifLjekarna)
                    .HasConstraintName("FK_RegistriraniKorisnik_Ljekarna");

                entity.HasOne(d => d.SifUlogaNavigation)
                    .WithMany(p => p.RegistriraniKorisnik)
                    .HasForeignKey(d => d.SifUloga)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistriraniKorisnik_UlogaKorisnik");
            });

            modelBuilder.Entity<UlogaKorisnik>(entity =>
            {
                entity.HasKey(e => e.SifraUloga)
                    .HasName("PK__UlogaKor__053C874A26871E15");

                entity.Property(e => e.SifraUloga).ValueGeneratedNever();

                entity.Property(e => e.OpisUloga)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<VrstaLijek>(entity =>
            {
                entity.HasKey(e => e.SifraVrstaLijek)
                    .HasName("PK__VrstaLij__504259ABB19826F9");

                entity.Property(e => e.SifraVrstaLijek).ValueGeneratedNever();

                entity.Property(e => e.OpisVrstaLijek)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<VrstaLjekarna>(entity =>
            {
                entity.HasKey(e => e.SifraVrstaLjekarna)
                    .HasName("PK__VrstaLje__B3D47E1961176E7D");

                entity.Property(e => e.SifraVrstaLjekarna).ValueGeneratedNever();

                entity.Property(e => e.OpisVrstaLjekarna)
                    .IsRequired()
                    .HasMaxLength(255);
            });
        }
    }
}

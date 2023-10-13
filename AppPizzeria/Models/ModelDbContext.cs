using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AppPizzeria.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<DettaglioOrdini> DettaglioOrdini { get; set; }
        public virtual DbSet<Ordine> Ordine { get; set; }
        public virtual DbSet<Prodotto> Prodotto { get; set; }
        public virtual DbSet<Utente> Utente { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordine>()
                .Property(e => e.CostoTotale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.Nota)
                .IsUnicode(false);

            modelBuilder.Entity<Ordine>()
                .HasMany(e => e.DettaglioOrdini)
                .WithRequired(e => e.Ordine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Prodotto>()
                .Property(e => e.Prezzo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Prodotto>()
                .Property(e => e.Ingredienti)
                .IsUnicode(false);

            modelBuilder.Entity<Prodotto>()
                .HasMany(e => e.DettaglioOrdini)
                .WithRequired(e => e.Prodotto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Ordine)
                .WithRequired(e => e.Utente)
                .WillCascadeOnDelete(false);
        }
    }
}

namespace AppPizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Prodotto")]
    public partial class Prodotto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prodotto()
        {
            DettaglioOrdini = new HashSet<DettaglioOrdini>();
        }

        [Key]
        public int IdProdotto { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        [NotMapped]
        public HttpPostedFileBase FotoFile { get; set;}

        [Column(TypeName = "money")]
        public decimal Prezzo { get; set; }

        [Required]
        [StringLength(10)]
        public string TempoConsegna { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Ingredienti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettaglioOrdini> DettaglioOrdini { get; set; }
    }
}

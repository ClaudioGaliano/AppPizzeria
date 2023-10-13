namespace AppPizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordine()
        {
            DettaglioOrdini = new HashSet<DettaglioOrdini>();
        }

        [Key]
        public int IdOrdine { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataOrdine { get; set; }

        [Column(TypeName = "money")]
        public decimal CostoTotale { get; set; }

        [Column(TypeName = "text")]
        public string Nota { get; set; }

        [Required]
        [StringLength(50)]
        public string Indirizzo { get; set; }

        public bool IsEvaso { get; set; }

        public int IdUtente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettaglioOrdini> DettaglioOrdini { get; set; }

        public virtual Utente Utente { get; set; }
    }
}

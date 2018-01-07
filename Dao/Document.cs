using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsManagment.Dao
{
    [Table("DOCUMENTS")]
    public class Document
    {
        [Display(Name = "Document ID")]
        public int DocumentId { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string Nom { get; set; }
        [Required, MinLength(1), MaxLength(20)]
        public string Format { get; set; }
        [Required, Range(0.1, double.MaxValue)]
        public double Taille { get; set; }
        public long FamilleID { set; get; }
        public virtual Famille Famille { set; get; }
    }
}

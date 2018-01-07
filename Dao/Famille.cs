using DocumentsManagment.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsManagment.Dao
{
    [Table("FAMILLES")]
    public class Famille
    {
        [Key]
        public long FamilleID { set; get; }
        [Required]
        [StringLength(50)]
        public string Nom { set; get; }
        public string Description { set; get; }
        public virtual ICollection<Document> Documents { set; get; }
        public Famille()
        {

        }
    }
}

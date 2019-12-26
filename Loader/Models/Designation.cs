using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Loader.Models
{
    [Table("Designation",Schema = "LG")]
    public class Designation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DesignationId")]
        public int DGId { get; set; }
        [Display(Name = "Parent Designation")]
        [Required(ErrorMessage = "Parent Designation Is Required")]
        public int PDGId { get; set; }
        [Display(Name = "Designation Name")]
        [Required(ErrorMessage = "Designation Name Is Required"), MaxLength(100), Column(TypeName = "nvarchar")]
        public string DGName { get; set; }
        public int PostedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PostedOn { get; set; }
    }

}
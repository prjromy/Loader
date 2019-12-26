using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("MenuTemplate",Schema = "LG")]
    public class MenuTemplate
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MenuTemplateId")]
        public int MTId { get; set; }
        [Display(Name = "MenuTemplate Name")]
        [Required(ErrorMessage = "MenuTemplate Name Is Required"), MaxLength(100), Column(TypeName = "nvarchar")]
        public string MTName { get; set; }
        [Required(ErrorMessage = "Designation Name Is Required")]
        public int DesignationId { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public virtual ICollection<MenuVsTemplate> MenuVSTemplate { get; set; }
    }
}
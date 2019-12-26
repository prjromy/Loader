using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("LayoutMenuMain", Schema = "LG")]
    public class LayoutMenuMain
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int LMenuId { get; set; }
        [Display(Name = "Layout Menu Name")]
        [Required(ErrorMessage = "Menu Name Is Required")]
        public string LMenuName { get; set; }
        public int ParentId { get; set; }
        [Display(Name = "Controller")]
        [Required(ErrorMessage = "Controller Name Is Required")]
        public string Controler { get; set; }
        [Display(Name = "Action")]
        [Required(ErrorMessage = "Action Name Is Required")]
        public string Acton { get; set; }
        public bool IsGroup { get; set; }
    }
}
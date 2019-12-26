using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loader.Models
{
    [Table("Menu",Schema = "LG")]
    public class Menu
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MenuId")]
        public int MenuId { get; set; }
        [Display(Name ="Menu Caption")]
        [Required(ErrorMessage ="Menu Caption Is Required"), MaxLength(100), Column(TypeName ="nvarchar")]
        public string MenuCaption { get; set; }
        [Display(Name ="Parent Menu")]
        [Required(ErrorMessage ="Parent Menu Is Required")]
        public int PMenuId { get; set; }
        public byte[] Image { get; set; }

        [Column("MenuOrder"), Required(ErrorMessage ="Menu Order Is Required.")]
        public int Order { get; set; }
        [Display(Name ="Is Group?")]
        public bool IsGroup { get; set; }
        [Display(Name ="Is Enable?")]
        public bool IsEnable { get; set; }
        [Display(Name ="Is ContextMenu?")]
        public bool IsContextMenu { get; set; }
        [Display(Name ="Controller")]      
        [Column(TypeName ="varchar"), MaxLength(200, ErrorMessage ="Field lenght cannot be grater than 100")]
        
        public string Controler { get; set; }
        [Display(Name ="Action")]        
        [Column(TypeName = "varchar"), MaxLength(200, ErrorMessage = "Field lenght cannot be grater than 100")]
        public string Acton { get; set; }
    
    }
}